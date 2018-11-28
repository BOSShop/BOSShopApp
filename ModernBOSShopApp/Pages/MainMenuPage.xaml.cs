using System.Windows;

namespace ModernBOSShopApp.Pages
{
    public partial class MainMenuPage : BasePage
    {
        public MainMenuPage()
        {
            InitializeComponent();
        }

        private void ProductsButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.LoadPageAsync("ProductsPage");
        }

        private void SellButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.LoadPageAsync("SellPage");
        }
    }
}