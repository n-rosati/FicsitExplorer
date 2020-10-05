using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Windows;
using FicsitExplorer.Properties;
using Newtonsoft.Json.Linq;
using Ookii.Dialogs.Wpf;
using RestSharp;

namespace FicsitExplorer
{
    //Singleton class
    public class ModManager
    {
        public List<Mod> ModList { get; }
        private static ModManager _instance;
        public readonly APIInteractor APIInteractor;

        public static string DownloadPath
        {
            get => Settings.Default.DownloadLocation;
            set => Settings.Default.DownloadLocation = value;
        }

        private ModManager()
        {
            ModList = new List<Mod>();
            APIInteractor = new APIInteractor();

            if (Directory.Exists(DownloadPath)) return;
            
            //Alert user and ask for new directory
            MessageBox.Show("Download location that was set is invalid. Please select a new, valid one.", "Invalid Download Path", MessageBoxButton.OK);
            SetDownloadLocation();
        }

        /**
         * Asks the user for a download location and sets it
         */
        internal static void SetDownloadLocation()
        {
            VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog
            {
                Description = "Select a download location.", UseDescriptionForTitle = true
            };
            dialog.ShowDialog();
            DownloadPath = dialog.SelectedPath;
        }

        /**
         * Gets the instance of a ModManager (or creates one if none exists)
         */
        public static ModManager GetInstance() => _instance ??= new ModManager();

        /**
         * Populates the mods list with all the mods available on the platform
         */
        public void PopulateMods()
        {
            IEnumerable<JToken> mods = APIInteractor.GetModList();
            foreach (JToken token in mods)
            {
                ModList.Add(CreateModFromJSON(token.ToString()));
            }
        }
        
        /**
         * Creates a mod given inputted JSON describing it
         */
        private static Mod CreateModFromJSON(string info)
        {
            Mod mod = new Mod();
            JObject parsedData = JObject.Parse(info);
            
            mod.Name             = (string)parsedData["name"]!;
            mod.ShortDescription = (string)parsedData["short_description"]!;
            mod.FullDescription  = (string)parsedData["full_description"]!;
            mod.Downloads        = (long)parsedData["downloads"]!;
            mod.ID               = (string)parsedData["id"]!;
            mod.LogoURL          = (string)parsedData["logo"]!;
            mod.LastUpdated      = (string)parsedData["updated_at"]!;
            
            //TODO: This should be a list of versions, for version selection
            // if (parsedData["versions"]!.Any()) mod.DownloadURL = $"https://api.ficsit.app{parsedData["versions"]![0]!["link"]!}";
            if (parsedData["versions"]!.Any())
                mod.DownloadURL = $"https://api.ficsit.app{parsedData["versions"]!.First!["link"]!}";
            return mod;
        }

        /**
         * Downloads a Mod to the user's Downloads directory (if it exists, else user home directory)
         * Returns true on success, false otherwise
         */
        [SuppressMessage("ReSharper.DPA", "DPA0001: Memory allocation issues")]
        public static async void DownloadMod(string downloadURL, string modName)
        {
            IRestResponse response = await APIInteractor.Client.ExecuteAsync(new RestRequest(downloadURL));
            if (response.IsSuccessful)
            {
                await File.WriteAllBytesAsync($"{DownloadPath}\\{response.Headers[3].Value!.ToString()!.Split('/')[2]}", response.RawBytes);
            }
            else
            {
                MessageBox.Show($"Could not download mod: {modName}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}