using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json.Linq;
using SAHB.GraphQLClient.Executor;

namespace FicsitExplorer
{
    public class APIInteractor
    {
        private readonly IGraphQLHttpExecutor _executor;
        private const string APIUrl = "https://api.ficsit.app/v2/query";

        public APIInteractor()
        {
            _executor = new GraphQLHttpExecutor();
        }

        /**
         * Gets details about a specific mod.
         * id: Mod ID to get details about
         */
        public string GetModDetails(string id)
        {
            return _executor.ExecuteQuery(
                $"{{\"query\":\"query {{getMod(modId:{id}){{id name short_description full_description logo downloads updated_at }}}}\"}}",
                APIUrl, 
                HttpMethod.Post).Result.Response;
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
                string response = _executor.ExecuteQuery(
                    $"{{\"query\":\"query {{getMods (filter: {{limit: 100 offset: {i * 100}}}){{count mods {{name short_description downloads id logo}}}}}}\"}}",
                    APIUrl, 
                    HttpMethod.Post).Result.Response;
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
            string response = GetDataFromJSON(_executor.ExecuteQuery(
                "{\"query\":\"query {getMods {count}}\"}",
                APIUrl, 
                HttpMethod.Post).Result.Response);
            
            int modCount;
            try
            {
                modCount = IntegerType.FromString(JObject.Parse(response)["getMods"]!["count"]!.ToString());
            }
            catch (NullReferenceException)
            {
                throw new Exception($"Could not find \"getMods\" or \"count\" fields in server response. Server sent:\n{response}");
            }

            return modCount;
        }
        
        /**
         * Just returns the "data" token from an inputted JSON string.
         * Returns null if parsing failed.
         */
        private string GetDataFromJSON(string input)
        {
            string returnString;
            try
            {
                returnString = JObject.Parse(input).SelectToken("data", false).ToString();
            }
            catch
            {
                returnString = null;
            }
            return returnString;
        }
    }
}