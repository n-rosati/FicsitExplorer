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
        
        public string GetModDetails(string id)
        {
            RestRequest request = _requestTemplate;
            request.AddParameter("application/json",
                //"{\"query\":\"query {\\r\\ngetMod(modId:\\\" + id + \\\"){\\r\\nname\\r\\nshort_description\\r\\ndownloads\\r\\nid\\r\\nlogo\\r\\n}\\r\\n}\"}"
                $@"{{""query"":""query {{
	                        getMod(modId:{id}){{
                                name
                                short_description
                                downloads
                                id
                                logo
                            }}
                        }}""
                    }}",
                ParameterType.RequestBody);
            return _client.Execute(request).Content;
        }

        public string GetModList()
        {
            /*TODO: Get a list of all mods on the site.
                        - Will need to query the site more than once, since the API only gives 100 at a time
                        -Return JSON string*/
            return null;
        }
    }
}