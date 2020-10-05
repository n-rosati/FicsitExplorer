using System;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xaml;
using Markdig;
using Markdig.Wpf;
using Markdown = Markdig.Wpf.Markdown;
using XamlReader = System.Windows.Markup.XamlReader;

namespace FicsitExplorer
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            if (InternetState())
            {
                ModManager manager = ModManager.GetInstance();
                manager.PopulateMods();
                LvMods.ItemsSource = manager.ModList.OrderByDescending(mod => mod.Downloads).ToList();
            }
            else
            {
                MessageBox.Show(
                    "Could not connect to API server, please check connection to https://ficsit.app/. Application will now exit",
                    "Network Error", MessageBoxButton.OK);
                Application.Current.Shutdown();
            }
        }

        private static bool InternetState()
        {
            bool ping;
            try
            {
                ping = new Ping().Send("api.ficsit.app")!.Status == IPStatus.Success;
            }
            catch
            {
                ping = false;
            }

            return ping;
        }

        /**
         * Downloads the selected mod to the user's Downloads folder
         */
        private void DownloadMod(object sender, RoutedEventArgs e)
        {
            ModManager.DownloadMod((LvMods.SelectedItem as Mod)!.DownloadURL, (LvMods.SelectedItem as Mod)!.Name);
        }

        //Event handlers

        /**
         * Sets the mod description to a markdown formatted document
         */
        private void SetModDetails(object sender, SelectionChangedEventArgs e)
        {
            Mod mod = (Mod) ((DataGrid) sender).SelectedItem;

            if (mod.LogoURL == "")
                LogoImage.Source = (ImageSource) new ImageSourceConverter().ConvertFrom(Properties.Resources.NotFound);
            else
                LogoImage.Source = new BitmapImage(new Uri(mod.LogoURL));

            DownloadButton.IsEnabled = true;

            //Source: https://github.com/Kryptos-FR/markdig.wpf/blob/master/src/Markdig.Xaml.SampleApp/MainWindow.xaml.cs#L36
            //Sets the mod details view with the markdown rendered content
            using MemoryStream stream =
                new MemoryStream(Encoding.UTF8.GetBytes(
                                     Markdown.ToXaml(mod.FullDescription, new MarkdownPipelineBuilder().UseSupportedExtensions().Build())));
            using XamlXmlReader reader = new XamlXmlReader(stream, new XamlSchemaContext());
            if (XamlReader.Load(reader) is FlowDocument document) ModDescription.Document = document;
        }

        private void SetDownloadLocation_OnClick(object sender, RoutedEventArgs e)
        {
            ModManager.SetDownloadLocation();
        }

        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}