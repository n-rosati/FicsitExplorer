using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FicsitExplorer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            List<Mod> mods = new List<Mod>();
            mods.Add(new Mod() {Logo = "logoURL", Name = "testName", Description = "testDesc", Downloads = "123456789"});
            lvMods.ItemsSource = mods;
        }
    }

    public class Mod
    {
        public string Logo { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Downloads { get; set; }
    }
}
