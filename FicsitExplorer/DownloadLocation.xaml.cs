using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace FicsitExplorer
{
    public partial class DownloadLocation : Window
    {
        public DownloadLocation()
        {
            InitializeComponent();
            //Sets the text box content with the currently set download location, defaulting to downloads folder
            ((TextBox) ((Grid) Content).Children[0]).Text = ModManager.GetInstance().DownloadPath;
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            string path = ((TextBox) ((Grid) Content).Children[0]).Text;

            if (!Directory.Exists(path))
            {
                MessageBox.Show("Invalid path.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                ModManager.GetInstance().DownloadPath = path;
            }
        
            Hide();
        }
    }
}