using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace FicsitExplorer
{
    //Singleton class
    public class ModManager
    {
        public List<Mod> ModList { get; }
        private static ModManager _instance;
        private readonly APIInteractor _apiInteractor;
        public string DownloadPath { get; set; }

        private ModManager()
        {
            ModList = new List<Mod>();
            _apiInteractor = new APIInteractor();

            string homeFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + Path.DirectorySeparatorChar;
            DownloadPath = Directory.Exists(homeFolder) ? homeFolder + "Downloads" : homeFolder; //Download path is downloads folder or home folder as fallback
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
            List<JToken> mods = _apiInteractor.GetModList();
            foreach (JToken token in mods)
            {
                ModList.Add(CreateModFromJSON(token.ToString()));
            }
        }
        
        /**
         * Creates a mod given inputted JSON describing it
         */
        private Mod CreateModFromJSON(string info)
        {
            Mod mod = new Mod();
            JObject parsedData   = JObject.Parse(info);
            
            mod.Name             = (string)parsedData["name"]!;
            mod.ShortDescription = (string)parsedData["short_description"]!;
            // mod.FullDescription  = (string)parsedData["full_description"]!;
            //TODO: Figure out why full_description breaks the JSON parsing
            //Might be because of the JSON parser used, I don't think I'm using the Newtonsoft JSON parser and that _could_ fix it
            
            mod.Downloads        = (long)parsedData["downloads"]!;
            mod.ID               = (string)parsedData["id"]!;
            mod.LogoURL          = (string)parsedData["logo"]!;
            mod.LastUpdated      = (string)parsedData["updated_at"]!;
            
            //TODO: This should be a list of versions, for version selection
            // if (parsedData["versions"]!.Any()) mod.DownloadURL = $"https://api.ficsit.app{parsedData["versions"]![0]!["link"]!}";
            if (parsedData["versions"]!.Any())
            {
                mod.DownloadURL = $"https://api.ficsit.app{parsedData["versions"]![0]!["link"]!}";
            }
            return mod;
        }

        /**
         * Downloads a Mod to the user's Downloads directory (if it exists, else user home directory)
         * Returns true on success, false otherwise
         */
        public void DownloadMod(string url)
        {
            //TODO: Run the downloader on another thread to prevent pausing on the main UI
            ModFile modFile = _apiInteractor.DownloadMod(url);
            File.WriteAllBytes($"{DownloadPath}\\{modFile.FileName}", modFile.Data);
        }
    }
}