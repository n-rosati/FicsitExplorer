using System.Windows;

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
            
            //For debugging
            APIInteractor interactor = new APIInteractor();
        }
    }
}
