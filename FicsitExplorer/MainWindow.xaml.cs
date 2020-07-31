using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace FicsitExplorer
{
    public partial class MainWindow : Window
    {
        public static MainWindow WindowPane { get; set; }
        private ModManager Manager;
        public MainWindow()
        {
            InitializeComponent();
            Manager = ModManager.GetInstance();
            Manager.PopulateMods();
            
            LvMods.ItemsSource = Manager.ModList;
            //TODO: Allow user to set sorting method
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(LvMods.ItemsSource);
            view.SortDescriptions.Add(new SortDescription("Downloads", ListSortDirection.Descending));
            
            WindowPane = this;
        }

        /**
         * Downloads the selected mod to the user's Downloads folder
         */
        private void DownloadMod(object sender, RoutedEventArgs e)
        {
            try
            {
                Manager.DownloadMod((LvMods.SelectedItem as Mod).DownloadURL);
            }
            catch
            {
                //TODO: Alert popup instead
                Console.WriteLine("Could not download mod.");
            }
        }

        //Event handlers

        private void SetModDetails(object sender, SelectionChangedEventArgs e)
        {
            Mod mod = (Mod) (((ListView) sender).SelectedItem);
            
            LogoImage.Source = new BitmapImage(new Uri(mod.LogoURL));
            ModDescription.Text = mod.ShortDescription;
            DownloadButton.IsEnabled = true;
        }
        
        private void SetDownloadLocation_OnClick(object sender, RoutedEventArgs e)
        {
            new DownloadLocation().ShowDialog();
        }

        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Help_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
            //TODO: Show a popup on how to use the program
        }
    }
}
