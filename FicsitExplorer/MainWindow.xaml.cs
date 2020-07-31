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
        private readonly ModManager _manager;
        public MainWindow()
        {
            InitializeComponent();
            _manager = ModManager.GetInstance();
            _manager.PopulateMods();
            
            LvMods.ItemsSource = _manager.ModList;
            //TODO: Allow user to set sorting method
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(LvMods.ItemsSource);
            view.SortDescriptions.Add(new SortDescription("Downloads", ListSortDirection.Descending));
        }

        /**
         * Downloads the selected mod to the user's Downloads folder
         */
        private void DownloadMod(object sender, RoutedEventArgs e)
        {
            try
            {
                _manager.DownloadMod((LvMods.SelectedItem as Mod).DownloadURL);
            }
            catch
            {
                MessageBox.Show($"Could not download mod.\n\n{e}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //Event handlers

        private void SetModDetails(object sender, SelectionChangedEventArgs e)
        {
            Mod mod = (Mod)((ListView)sender).SelectedItem;
            
            LogoImage.Source = new BitmapImage(new Uri(mod.LogoURL));
            ModDescription.Text = mod.ShortDescription;
            DownloadButton.IsEnabled = true;
        }
        
        private void SetDownloadLocation_OnClick(object sender, RoutedEventArgs e)
        {
            //I want this to be a native folder picker but don't know how to make it
            new DownloadLocation().ShowDialog();
        }

        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
