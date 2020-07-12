using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
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

        //Event handlers
        private void SetModDetails(object sender, MouseButtonEventArgs e)
        {
            Mod mod = (Mod)((ListViewItem) sender).Content;
            
            LogoImage.Source = new BitmapImage(new Uri(mod.LogoURL));
            ModDescription.Text = mod.Description;
            DownloadButton.IsEnabled = true;
        }

        private void DownloadMod(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Downloading not yet implemented");
        }
    }
}
