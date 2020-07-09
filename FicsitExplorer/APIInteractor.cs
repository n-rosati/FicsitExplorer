using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using SAHB.GraphQLClient.Executor;

namespace FicsitExplorer
{
    public class APIInteractor
    {
        private const string APIUrl = "https://api.ficsit.app/v2/query";

        private string MakeQuery(string query)
        {
            IGraphQLHttpExecutor executor = new GraphQLHttpExecutor();
            string returnString;
            try
            {
                returnString = JObject.Parse(executor.ExecuteQuery(query, APIUrl, System.Net.Http.HttpMethod.Post).Result.Response).SelectToken("data", false)!.ToString();
            }
            catch
            {
                returnString = null;
            }

            return returnString;
        }

        /**
         * Gets details about a specific mod.
         * id: Mod ID to get details about
         */
        public string GetModDetails(string id)
        {
            return MakeQuery(
                $"{{\"query\":\"query {{getMod(modId:{id}){{id name short_description full_description logo downloads updated_at }}}}\"}}");
        }

        /**
         * Gets a list of all mods on the website
         */
        public List<JToken> GetModList()
        {
            int modCount = GetModsCount();
            List<JToken> mods = new List<JToken>();
            for (int i = 0; i < (modCount / 100) + 1; i++)
            {
                string response =
                    MakeQuery(
                        $"{{\"query\":\"query {{getMods (filter: {{limit: 100 offset: {i * 100}}}){{count mods {{name short_description downloads id logo}}}}}}\"}}");
                try
                {
                    mods.AddRange(JObject.Parse(response)["getMods"]!["mods"]!.ToList());
                }
                catch (NullReferenceException)
                {
                    throw new Exception(
                        $"Could not find \"getMods\" or \"mods\" fields in server response. Server sent:\n{response}");
                }
            }

            return mods;
        }

        /**
         * Gets the number of mods available on the platform
         */
        private int GetModsCount()
        {
            string response = MakeQuery("{\"query\":\"query {getMods {count}}\"}");
            int modCount;
            try
            {
                modCount = Microsoft.VisualBasic.CompilerServices.IntegerType.FromString(JObject.Parse(response)["getMods"]!["count"]!.ToString());
            }
            catch (NullReferenceException)
            {
                throw new Exception(
                    $"Could not find \"getMods\" or \"count\" fields in server response. Server sent:\n{response}");
            }

            return modCount;
        }
    }
}
