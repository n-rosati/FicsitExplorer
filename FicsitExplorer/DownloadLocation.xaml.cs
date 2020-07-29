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
            ((TextBox)((Grid)Content).Children[0]).Text = ModManager.GetInstance().DownloadPath;
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            //TODO: Path validation
            ModManager.GetInstance().DownloadPath = ((TextBox)((Grid)Content).Children[0]).Text;
            Hide();
        }
    }
}