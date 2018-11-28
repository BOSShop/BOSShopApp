using ModernBOSShopApp.ProductLogic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace ModernBOSShopApp.Pages
{
    public partial class SellPage : BasePage
    {
        public ObservableCollection<CardProduct> card;

        public SellPage()
        {
            card = new ObservableCollection<CardProduct>();

            InitializeComponent();

            CalculateCompletePrice();

            ProductsChanged();

            ProductManager.Instance.ProductsChangedEvent += ProductsChanged;

            CardDataGrid.Columns.Add(new DataGridTextColumn { Header = "Produkt Name", Binding = new Binding("Name"), MinWidth = 210, IsReadOnly = true });
            CardDataGrid.Columns.Add(new DataGridTextColumn { Header = "Anzahl", Binding = new Binding("Count"), MinWidth = 60 });

            Binding priceBinding = new Binding("Price");
            priceBinding.StringFormat = "C2";
            priceBinding.ConverterCulture = new CultureInfo("de-DE");
            CardDataGrid.Columns.Add(new DataGridTextColumn { Header = "Preis (Normal)", Binding = priceBinding, MinWidth = 90, IsReadOnly = true });

            Binding currentPriceBinding = new Binding("CurrentPrice");
            currentPriceBinding.StringFormat = "C2";
            currentPriceBinding.ConverterCulture = new CultureInfo("de-DE");
            CardDataGrid.Columns.Add(new DataGridTextColumn { Header = "Preis (Aktuell)", Binding = currentPriceBinding, MinWidth = 90, IsReadOnly = true });

            Binding wholePriceBinding = new Binding("WholePrice");
            wholePriceBinding.StringFormat = "C2";
            wholePriceBinding.ConverterCulture = new CultureInfo("de-DE");
            CardDataGrid.Columns.Add(new DataGridTextColumn { Header = "Preis (Alles)", Binding = wholePriceBinding, MinWidth = 90 , IsReadOnly = true});

            CardDataGrid.ItemsSource = card;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.LoadPageAsync("MainMenuPage");
        }

        public void ProductsChanged()
        {
            List<string> products = (from product in ProductManager.Instance.products select product.Name).ToList();

            CardAddProductComboBox.ItemsSource = products;

            Refresh();
        }

        private void CardAddProductComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                if(CardAddProductComboBox.Text == "SECRET")
                {
                    MainWindow.Instance.LoadPageAsync("SecretGamePage", false);
                    CardAddProductComboBox.Text = "";
                    return;
                }

                TryAddToCard();
            }
        }

        private void CardAddProductButton_Click(object sender, RoutedEventArgs e)
        {
            TryAddToCard();
        }

        private void TryAddToCard()
        {
            CardProduct found = ProductManager.Instance.GetCardProduct(CardAddProductComboBox.Text);

            if(found != null)
            {
                bool exists = false;

                foreach(CardProduct product in card)
                {
                    if(product.Name == found.Name)
                    {
                        product.Count += 1;
                        product.CalculateWholePrice();
                        exists = true;
                        break;
                    }
                }

                if (!exists)
                    card.Add(found);

                Refresh();
            }

            CardAddProductComboBox.Text = "";
        }

        private void CardDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            CardDataGrid.Dispatcher.BeginInvoke(new Action(() =>
            {
                Refresh();
            }), DispatcherPriority.Background);
        }

        public void CalculateCompletePrice()
        {
            decimal price = 0m;

            foreach(CardProduct product in card)
            {
                product.CalculateWholePrice();
                price += product.WholePrice;
            }

            if (WorkerCheckBox.IsChecked == true)
                price *= 0.75m;

            ToPayTextBlock.Text = "Zu Bezahlen: " + price.ToString("C2", new CultureInfo("de-DE"));
        }

        private void WorkerCheckBox_Click(object sender, RoutedEventArgs e)
        {
            CalculateCompletePrice();
        }

        private void CardDataGrid_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == DataGrid.DeleteCommand)
            {
                CardDataGrid.Dispatcher.BeginInvoke(new Action(() =>
                {
                    Refresh();
                }), DispatcherPriority.Background);
            }
        }

        private void SellButton_Click(object sender, RoutedEventArgs e)
        {
            bool canSell = true;

            foreach (CardProduct product in card)
            {
                Product p = ProductManager.Instance.GetProduct(product.Name);

                if (p != null)
                {
                    if (product.Count > p.Count)
                    {
                        canSell = false;
                    }
                }
                else
                {
                    canSell = false;
                }
            }

            if (!canSell)
                return;

            foreach (CardProduct product in card)
            {
                Product p = ProductManager.Instance.GetProduct(product.Name);

                if (p != null)
                {
                    p.Count -= product.Count;
                }
            }

            card.Clear();
            WorkerCheckBox.IsChecked = false;
            CalculateCompletePrice();
            ProductManager.Instance.ProductChanged();
        }

        private void Refresh()
        {
            CalculateCompletePrice();

            try
            {
                CardDataGrid.Items.Refresh();
            }
            catch
            {

            }
        }

        private void CardDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            DataGridRow row = e.Row;
            CardProduct product = row.DataContext as CardProduct;

            if (product.Count > ProductManager.Instance.GetProduct(product.Name).Count)
                row.Background = new SolidColorBrush(Colors.Red);
            else
                if (product.CurrentPrice != product.Price)
                row.Background = new SolidColorBrush(Colors.Orange);
        }

        private void CardClearButton_Click(object sender, RoutedEventArgs e)
        {
            card.Clear();
            Refresh();
        }
    }
}