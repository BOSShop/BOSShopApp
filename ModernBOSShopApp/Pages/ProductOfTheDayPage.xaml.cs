using ModernBOSShopApp.Animation;
using ModernBOSShopApp.ProductLogic;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Timers;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ModernBOSShopApp.Pages
{
    public partial class ProductOfTheDayPage : BasePage
    {
        public ProductOfTheDayPage()
        {
            animation = PageTransitionAnimation.SlideTop;

            InitializeComponent();
        }

        public override void OnLoad()
        {
            if (ProductManager.Instance.products.Count <= 0)
            {
                MainWindow.Instance.LoadLastPage();
                return;
            }

            List<Product> products = (from p in ProductManager.Instance.products where p.Count >= 5 select p).ToList();

            Random random = new Random();
            int productIndex = random.Next(0, products.Count);
            int percentage = random.Next(5, 26);

            Product product = products[productIndex];

            ProductPreviewImage.BeginInit();

            string imagesPath = MainWindow.Instance.fileManager.GetPath("Images");

            string productName = product.Name.Replace(' ', '_');

            if (File.Exists(Path.Combine(imagesPath, productName+".png")))
            {
                ProductPreviewImage.Source = new BitmapImage(new Uri(Path.Combine(imagesPath, productName+".png"), UriKind.Absolute));
            }
            else
            {
                if (!string.IsNullOrEmpty(product.Category))
                {
                    string productCategory = product.Category.Replace(' ', '_');

                    if (File.Exists(Path.Combine(imagesPath, productCategory+".png")))
                    {
                        ProductPreviewImage.Source = new BitmapImage(new Uri(Path.Combine(imagesPath, productCategory+".png"), UriKind.Absolute));
                    }
                    else
                    {
                        ProductPreviewImage.Source = new BitmapImage(new Uri("/Icons/ProductPlaceholderIcon.png", UriKind.Relative));
                    }
                }
                else
                {
                    ProductPreviewImage.Source = new BitmapImage(new Uri("/Icons/ProductPlaceholderIcon.png", UriKind.Relative));
                }
            }

            ProductPreviewImage.EndInit();

            if (!string.IsNullOrEmpty(product.Category))
            {
                ProductOfTheDayTextBlock.Text = "Produkte der Kategorie \"" + product.Category + "\" erhalten einen Rabatt von " + percentage + "%.";
                ProductManager.Instance.discount = new CategoryDiscount(product.Category, percentage);
            }
            else
            {
                ProductOfTheDayTextBlock.Text = "\"" + product.Name + "\" erhält einen Rabatt von " + percentage + "%.";
                ProductManager.Instance.discount = new ProductDiscount(product.Name, percentage);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.LoadLastPage();
        }
    }
}