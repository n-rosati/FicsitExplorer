using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace FicsitExplorer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            //Function in need of refactoring for OOPness
            InitializeComponent();
            List<Mod> mods = new List<Mod>();
            mods.Add(new Mod()
            {
                Name = "MoarDevice", 
                Description = "The Tablet for MoarFactory Mod", 
                Downloads = "2555",
                ID = "4qDVwaRyK9Dvio",
                LogoURL = "https://storage.ficsit.app/file/smr-prod/images/mods/4qDVwaRyK9Dvio/logo.webp"
            });
            LvMods.ItemsSource = mods;
            
            //PoC for how to set the image programatically
            Image image = new Image();
            image.Source = new BitmapImage(new Uri(mods[0].LogoURL));
            LogoImage.Source = image.Source;
        }
    }

    public class Mod
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Downloads { get; set; }
        public string ID { get; set; }
        public string LogoURL { get; set; }
    }
}
