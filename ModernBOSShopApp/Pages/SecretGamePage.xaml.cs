using ModernBOSShopApp.Animation;
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

namespace ModernBOSShopApp.Pages
{
    /// <summary>
    /// Interaction logic for SecretGamePage.xaml
    /// </summary>
    public partial class SecretGamePage : BasePage
    {
        public SecretGamePage()
        {
            animation = PageTransitionAnimation.SlideBottom;

            InitializeComponent();
        }

        private void ScanItemGameButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.LoadPageAsync("ScanFastGamePage", false);
        }

        private void ScanDifferentItemsGameButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.LoadPageAsync("ScanListGamePage", false);
        }
    }
}
