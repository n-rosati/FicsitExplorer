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

        public static ModManager GetInstance()
        {
            return _instance ??= new ModManager();
        }

        public void AddMod(string id)
        {
            ModList.Add(CreateMod(id));
        }

        private static Mod CreateMod(string id)
        {
            
            return null;
        }
    }
}