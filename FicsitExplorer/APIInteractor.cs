using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json.Linq;
using RestSharp;
using SAHB.GraphQLClient.Executor;

namespace FicsitExplorer
{
    public class APIInteractor
    {
        private const string BaseURL = "https://api.ficsit.app";
        public static readonly IRestClient Client = new RestClient(BaseURL);

        private static string MakeQuery(string query)
        {
            IGraphQLHttpExecutor executor = new GraphQLHttpExecutor();
            string returnString;
            try
            {
                returnString =
                    JObject.Parse(executor.ExecuteQuery(query, $"{BaseURL}/v2/query", HttpMethod.Post).Result.Response)
                           .SelectToken("data", false)!.ToString();
            }
            catch
            {
                returnString = null;
            }

            return returnString;
        }

        /**
         * Gets a list of all mods on the website
         */
        public IEnumerable<JToken> GetModList()
        {
            int modCount = GetModsCount();
            List<JToken> mods = new List<JToken>();
            //The API only sends back 100 mods when you request getMods
            for (int i = 0; i < modCount / 100 + 1; i++)
            {
                string response = MakeQuery(
                    $"{{\"query\":\"query {{getMods (filter: {{limit: 100 offset: {i * 100}}}){{count mods {{id name short_description full_description logo downloads updated_at versions(filter: {{limit: 1}}){{link}}}}}}}}\"}}"
                );
                try
                {
                    mods.AddRange(JObject.Parse(response)["getMods"]!["mods"]!.ToList());
                }
                catch (NullReferenceException)
                {
                    throw new Exception($"Could not find \"getMods\" or \"mods\" fields in server response. Server sent:\n{response}");
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
                modCount = IntegerType.FromString(JObject.Parse(response)["getMods"]!["count"]!.ToString());
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