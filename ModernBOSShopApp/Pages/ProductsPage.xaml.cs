using ModernBOSShopApp.ProductLogic;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace ModernBOSShopApp.Pages
{
    public partial class ProductsPage : BasePage
    {
        public ProductsPage()
        {
            InitializeComponent();

            ProductsDataGrid.Columns.Add(new DataGridTextColumn { Header = "Produkt Name", Binding = new Binding("Name"), MinWidth = 120 });
            ProductsDataGrid.Columns.Add(new DataGridTextColumn { Header = "Scan Text", Binding = new Binding("ScanText"), MinWidth = 120 });
            ProductsDataGrid.Columns.Add(new DataGridTextColumn { Header = "Kategorie", Binding = new Binding("Category"), MinWidth = 120 });
            ProductsDataGrid.Columns.Add(new DataGridTextColumn { Header = "Anzahl", Binding = new Binding("Count"), MinWidth = 60 });

            Binding priceBinding = new Binding("Price");

            priceBinding.StringFormat = "C2";
            priceBinding.ConverterCulture = new CultureInfo("de-DE");

            ProductsDataGrid.Columns.Add(new DataGridTextColumn { Header = "Preis", Binding = priceBinding, MinWidth = 60 });

            ProductsDataGrid.ItemsSource = ProductManager.Instance.products;

            ProductManager.Instance.ProductsChangedEvent += ProductsChanged;
        }

        private void ProductsChanged()
        {
            try
            {
                ProductsDataGrid.Items.Refresh();
            }
            catch
            {

            }
        }

        public override void OnLoad()
        {
            try
            {
                ProductsDataGrid.Items.Refresh();
            }
            catch
            {

            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.LoadPageAsync("MainMenuPage");
        }

        private void ProductsAddButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.LoadPageAsync("AddProductPage");
        }

        private void ProductsDataGrid_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == DataGrid.DeleteCommand)
            {
                if (!(MessageBox.Show("Bist du dir sicher ?", "Bitte Bestätige.", MessageBoxButton.YesNo) == MessageBoxResult.Yes))
                {
                    e.Handled = true;
                }
            }

            ProductsDataGrid.Dispatcher.BeginInvoke(new Action(() =>
            {
                ProductManager.Instance.ProductChanged();
            }), DispatcherPriority.Background);
        }

        private void ProductsDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            ProductsDataGrid.Dispatcher.BeginInvoke(new Action(() =>
            {
                ProductManager.Instance.ProductChanged();
            }), DispatcherPriority.Background);
        }

        private void ProductsDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            DataGridRow row = e.Row;
            Product product = row.DataContext as Product;

            if (product.Count <= 0)
                row.Background = new SolidColorBrush(Colors.Red);
            else if(product.Count <= 15)
                row.Background = new SolidColorBrush(Colors.Orange);
            else
                row.Background = new SolidColorBrush(Colors.LightGreen);
        }
    }
}