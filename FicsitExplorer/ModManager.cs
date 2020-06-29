using System.Collections.Generic;
using GraphQL;

namespace FicsitExplorer
{
    public class ModManager
    {
        public List<Mod> ModList { get; }
        private static ModManager _instance;

        //For singleton stuff
        private ModManager()
        {
            ModList = new List<Mod>();
        }
        
        public static ModManager GetInstance ()
        {
            if (_instance == null) _instance = new ModManager();
            return _instance;
        }
        
        public void AddMod(string id)
        {
            ModList.Add(GetModInfo(id));
        }
        
        private static Mod GetModInfo(string id)
        {
            /*TODO: Get info from GraphQL API https://api.ficsit.app/v2/query*/
            GraphQLRequest request = new GraphQLRequest
            {
                Query = "query{\ngetMod(modId:" + id + "){\nname\nshort_description\ndownloads\nid\nlogo\n}\n}"
            };
            
            return null;
        }
    }
}
