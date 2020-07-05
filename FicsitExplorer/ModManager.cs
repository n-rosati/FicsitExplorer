using System.Collections.Generic;
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json.Linq;

namespace FicsitExplorer
{
    //Singleton class
    public class ModManager
    {
        public List<Mod> ModList { get; }
        private static ModManager _instance;
        private readonly APIInteractor _apiInteractor;

        private ModManager()
        {
            ModList = new List<Mod>();
            _apiInteractor = new APIInteractor();
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
            mod.Description = parsedData["short_description"]!.ToString();
            mod.Downloads = LongType.FromString(parsedData["downloads"]!.ToString());
            mod.ID = parsedData["id"]!.ToString();
            mod.LogoURL = parsedData["logo"]!.ToString();
            return mod;
        }
    }
}