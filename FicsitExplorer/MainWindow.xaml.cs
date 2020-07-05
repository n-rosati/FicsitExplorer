using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace FicsitExplorer
{
    public partial class MainWindow : Window
    {
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
            
            //PoC for how to set the image programatically
            
            /*
            Image image = new Image();
            image.Source = new BitmapImage(new Uri(mods[0].LogoURL));
            LogoImage.Source = image.Source;
            */
        }
    }
}
