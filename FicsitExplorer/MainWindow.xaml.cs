using System;
using System.ComponentModel;
using System.IO;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
    public partial class MainWindow : Window
    {
        private readonly ModManager _manager;

        public MainWindow()
        {
            InitializeComponent();

            if (internetState())
            {
                _manager = ModManager.GetInstance();
                _manager.PopulateMods();

                LvMods.ItemsSource = _manager.ModList;
                //TODO: Allow user to set sorting method
                CollectionView view = (CollectionView) CollectionViewSource.GetDefaultView(LvMods.ItemsSource);
                view.SortDescriptions.Add(new SortDescription("Downloads", ListSortDirection.Descending));
            }
            else
            {
                MessageBox.Show(
                    "Could not connect to API server, please check connection to ficsit.app. Application will now exit",
                    "Network Error", MessageBoxButton.OK);
                    Application.Current.Shutdown();
            }
        }

        bool internetState()
        {
            bool ping = false;
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
            try
            {
                _manager.DownloadMod((LvMods.SelectedItem as Mod)!.DownloadURL);
            }
            catch
            {
                MessageBox.Show($"Could not download mod.\n\n{e}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //Event handlers

        /**
         * Sets the mod description to a markdown formatted document
         */
        private void SetModDetails(object sender, SelectionChangedEventArgs e)
        {
            Mod mod = (Mod) ((ListView) sender).SelectedItem;

            if (mod.LogoURL == "")
            {
                LogoImage.Source = (ImageSource) new ImageSourceConverter().ConvertFrom(Properties.Resources.NotFound);
            }
            else
            {
                LogoImage.Source = new BitmapImage(new Uri(mod.LogoURL));
            }
            
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
            //TODO: I want this to be a native folder picker but don't know how to make it
            new DownloadLocation().ShowDialog();
        }

        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}