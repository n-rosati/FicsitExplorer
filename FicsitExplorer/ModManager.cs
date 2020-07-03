using System.Collections.Generic;

namespace FicsitExplorer
{
    //Singleton class
    public class ModManager
    {
        public List<Mod> ModList { get; }
        private static ModManager _instance;
        private APIInteractor _apiInteractor;

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
         * Adds a mod to the mods list
         */
        public void AddMod(string id)
        {
            ModList.Add(CreateMod(id));
        }

        /**
         * Creates a new Mod given a mod id
         */
        private static Mod CreateMod(string id)
        {
            
            return null;
        }
    }
}