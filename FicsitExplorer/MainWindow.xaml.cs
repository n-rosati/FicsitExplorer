using System;
using System.Windows;
using RestSharp;

namespace FicsitExplorer
{
    public partial class MainWindow : Window
    {
        private ModManager Manager;
        public MainWindow()
        {
            InitializeComponent();
            Manager = ModManager.GetInstance();
            
            LvMods.ItemsSource = Manager.ModList;
            
            //PoC for how to set the image programatically
            
            /*
            Image image = new Image();
            image.Source = new BitmapImage(new Uri(mods[0].LogoURL));
            LogoImage.Source = image.Source;
            */


            graphqlexample();
        }
        
        //This isn't temporary, just here to show myself how this is done
        //Most of this was generated with Postman
        static void graphqlexample ()
        {
            RestClient client = new RestClient("https://api.ficsit.app/v2/query");
            client.Timeout = -1;
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            
            string query = "{\"query\":\"query{\\r\\ngetMod(modId:\\\"CjfhzfJT1f7HMk\\\"){\\r\\nname\\r\\nshort_description\\r\\ndownloads\\r\\nid\\r\\nlogo\\r\\n}\\r\\n}\"}";
            request.AddParameter("application/json", query, ParameterType.RequestBody);
            
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
        }
    }
}
