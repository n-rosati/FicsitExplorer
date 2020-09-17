using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using FicsitExplorer.Properties;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace FicsitExplorer
{
    //Singleton class
    public class ModManager
    {
        public List<Mod> ModList { get; }
        private static ModManager _instance;
        public readonly APIInteractor APIInteractor;

        public string DownloadPath
        {
            get => Settings.Default.DownloadLocation;
            set => Settings.Default.DownloadLocation = value;
        }

        private ModManager()
        {
            ModList = new List<Mod>();
            APIInteractor = new APIInteractor();

            /*TODO: Test. I wrote this tired.
                    -I think the best way is to default to "" and if Settings.Default.DownloadLocation == "" then prompt to set location, else use Settings.Default.DownloadLocation*/
            if (Directory.Exists(Settings.Default.DownloadLocation))
            {
                DownloadPath = Settings.Default.DownloadLocation;
            }
            else
            {
                string homeFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) +
                                    Path.DirectorySeparatorChar;
                DownloadPath =
                    Directory.Exists(homeFolder)
                        ? homeFolder + "Downloads"
                        : homeFolder; //Download path is downloads folder or home folder as fallback
            }
        }

        /**
         * Gets the instance of a ModManager (or creates one if none exists)
         */
        public static ModManager GetInstance()
        {
            return _instance ??= new ModManager();
        }

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
            JObject parsedData   = JObject.Parse(info);
            
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
            {
                mod.DownloadURL = $"https://api.ficsit.app{parsedData["versions"]!.First!["link"]!}";
            }
            return mod;
        }

        /**
         * Downloads a Mod to the user's Downloads directory (if it exists, else user home directory)
         * Returns true on success, false otherwise
         */
        [SuppressMessage("ReSharper.DPA", "DPA0001: Memory allocation issues")]
        public void DownloadMod(string url)
        {
            //TODO: Run the downloader on another thread to prevent pausing on the main thread
            IRestResponse response = APIInteractor.Client.Get(new RestRequest(url));
			if (!response.IsSuccessful) throw new Exception("Download failed.");
            File.WriteAllBytes($"{DownloadPath}\\{response.Headers[3].Value!.ToString()!.Split('/')[2]}", response.RawBytes);
        }
    }
}