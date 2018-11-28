using ModernBOSShopApp.Animation;
using ModernBOSShopApp.ProductLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ModernBOSShopApp.Pages
{
    /// <summary>
    /// Interaction logic for AddProductPage.xaml
    /// </summary>
    public partial class AddProductPage : BasePage
    {
        public AddProductPage()
        {
            animation = PageTransitionAnimation.SlideTop;

            InitializeComponent();
        }

        public override void OnLoad()
        {
            ProductNameTextBox.Text = "";
            ProductScanTextTextBox.Text = "";
            ProductCategoryTextBox.Text = "";
            ProductCountTextBox.Text = "";
            ProductPriceTextBox.Text = "";
        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            string name = ProductNameTextBox.Text;
            string scanText = ProductScanTextTextBox.Text;
            string category = ProductCategoryTextBox.Text;

            int count;

            bool couldParseCount = int.TryParse(ProductCountTextBox.Text, out count);

            decimal price;

            bool couldParsePrice = decimal.TryParse(ProductPriceTextBox.Text, out price);

            if(!string.IsNullOrEmpty(name) && couldParseCount && couldParsePrice)
            {
                ProductManager.Instance.products.Add(new Product(name, scanText, category, count, price));
                ProductManager.Instance.ProductChanged();

                MainWindow.Instance.LoadPageAsync("ProductsPage");
            }
            else
            {
                if (string.IsNullOrEmpty(name))
                    HighlightErrorOnTextBoxAsync(ProductNameTextBox);

                if (!couldParseCount)
                    HighlightErrorOnTextBoxAsync(ProductCountTextBox);

                if (!couldParsePrice)
                    HighlightErrorOnTextBoxAsync(ProductPriceTextBox);
            }
        }

        private async void HighlightErrorOnTextBoxAsync(TextBox textBox)
        {
            if (!(textBox.BorderBrush is SolidColorBrush))
                return;
            
            textBox.BorderBrush = new SolidColorBrush(Colors.Red);

            await Task.Delay((int)(animationDuration * 1000));

            textBox.BorderBrush = new SolidColorBrush(Colors.White);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.LoadPageAsync("ProductsPage");
        }
    }
}
