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
            JObject parsedData = JObject.Parse(info);
            mod.Name = parsedData["name"]!.ToString();
            mod.ShortDescription = parsedData["short_description"]!.ToString();
            //TODO: Figure out why full_description breaks the JSON parsing
            mod.Downloads = (long)parsedData["downloads"]!;
            mod.ID = parsedData["id"]!.ToString();
            mod.LogoURL = parsedData["logo"]!.ToString();
            mod.LastUpdated = parsedData["updated_at"]!.ToString();
            
            //TODO: This should be a list of versions, for version selection
            if (parsedData["versions"]!.Count() != 0) mod.DownloadURL = $"https://api.ficsit.app{parsedData["versions"]![0]!["link"]!}";
            return mod;
        }

        /**
         * Downloads a Mod to the user's Downloads directory (if it exists, else user home directory)
         * Returns true on success, false otherwise
         */
        public bool DownloadMod(string url)
        {
            /*
             *     - download mod using given URL
             *     - save file to disk at download URL
             */
            
            
            
            
            return false;
        }
    }
}