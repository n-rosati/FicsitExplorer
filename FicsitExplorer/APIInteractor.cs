using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace FicsitExplorer
{
    public class APIInteractor
    {
        private readonly RestClient _client;

        private readonly RestRequest _requestTemplate;

        public APIInteractor()
        {
            //Sets up the REST client and a template request (if readonly works the way I think it does)
            _client = new RestClient("https://api.ficsit.app/v2/query");
            /*To use:
                1. Make a local copy of the request
                2. Add the parameter 
                    ("application/json", <query>, ParameterType.RequestBody)
                3. client.Execute() the request*/
            _requestTemplate = new RestRequest(Method.POST);
            _requestTemplate.AddHeader("Accept", "application/json");
            _requestTemplate.AddHeader("Content-Type", "application/json");
        }

        /**
         * Gets details about a specific mod.
         * id: Mod ID to get details about
         */
        public string GetModDetails(string id)
        {
            RestRequest request = _requestTemplate;
            request.AddParameter("application/json",$"{{\"query\":\"query {{getMod(modId:{id}){{name short_description downloads id logo}}}}\"}}", ParameterType.RequestBody);
            return GetDataFromJSON(_client.Execute(request).Content);
        }

        /**
         * Gets a list of all mods on the website
         */
        public List<JToken> GetModList()
        {
            int modCount = GetModsCount();
            RestRequest request = _requestTemplate;
            List<JToken> mods = new List<JToken>();
            for (int i = 0; i < (modCount / 100) + 1; i++)
            {
                request.AddOrUpdateParameter("application/json",$"{{\"query\":\"query {{getMods (filter: {{limit: 100 offset: {i * 100}}}){{count mods {{name short_description downloads id logo}}}}}}\"}}", ParameterType.RequestBody);
                string response = GetDataFromJSON(_client.Execute(request).Content);
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
            RestRequest request = _requestTemplate;
            request.AddParameter("application/json", "{\"query\":\"query {getMods {count}}\"}", ParameterType.RequestBody);
            string response = GetDataFromJSON(_client.Execute(request).Content);
            
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