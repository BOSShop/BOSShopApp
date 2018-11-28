using ModernBOSShopApp.ProductLogic;
using ModernBOSShopApp.Secret;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for ScanListGamePage.xaml
    /// </summary>
    public partial class ScanListGamePage : BasePage
    {
        private string currentName;
        private bool isRunning = false;

        private int scanIndex;

        private List<Product> products = null;

        private ObservableCollection<FastScanResult> results;

        private DateTime gameStarted;

        private List<Product> left;

        public ScanListGamePage()
        {
            results = new ObservableCollection<FastScanResult>();

            InitializeComponent();

            ResultDataGrid.Columns.Add(new DataGridTextColumn { Header = "Name", Binding = new Binding("Name"), IsReadOnly = true, MinWidth = 120 });
            ResultDataGrid.Columns.Add(new DataGridTextColumn { Header = "Dauer", Binding = new Binding("TimeTook"), IsReadOnly = true, MinWidth = 150 });
        }

        public override void OnLoad()
        {
            isRunning = false;

            scanIndex = 0;

            results.Clear();

            HandleUI();

            CurrentNameTextBox.Text = "";

            InputTextBox.Text = "";
            ToScanTextBlock.Text = "";
            ProgressTextBlock.Text = "";
            TimeElapsedTextBlock.Text = "";
            ResultDataGrid.ItemsSource = results;
        }

        private void CurrentNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                currentName = CurrentNameTextBox.Text;

                products = new List<Product>();

                left = new List<Product>(ProductManager.Instance.products);

                int productCount = left.Count;

                Random rdm = new Random();

                for(int i = 0; i < (productCount); i++)
                {
                    int index = rdm.Next(0, left.Count);

                    products.Add(left[index]);

                    left.RemoveAt(index);
                }

                isRunning = true;

                scanIndex = 0;

                gameStarted = DateTime.Now;

                new Thread(GameThread).Start();

                HandleUI();

                InputTextBox.Focus();
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            OnLoad();
        }

        private void BackToMenuButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.LoadPageAsync("MainMenuPage");
        }

        private void InputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                if(InputTextBox.Text == products[scanIndex].Name || (!string.IsNullOrEmpty(products[scanIndex].ScanText) && InputTextBox.Text == products[scanIndex].ScanText))
                {
                    scanIndex++;

                    if(scanIndex == products.Count)
                    {
                        isRunning = false;

                        Dispatcher.Invoke(() =>
                        {
                            ToScanTextBlock.Text = "";
                            ProgressTextBlock.Text = "";
                            TimeElapsedTextBlock.Text = "";
                        });

                        scanIndex = 0;

                        TimeSpan difference = DateTime.Now - gameStarted;

                        results.Add(new FastScanResult(currentName, difference.Minutes.ToString("D2") + ":" + difference.Seconds.ToString("D2")));

                        Dispatcher.Invoke(() =>
                        {
                            CurrentNameTextBox.Text = "";
                        });

                        HandleUI();

                        Dispatcher.Invoke(() =>
                        {
                            ToScanTextBlock.Text = "";
                            ProgressTextBlock.Text = "";
                            TimeElapsedTextBlock.Text = "";
                        });
                    }
                }

                InputTextBox.Text = "";
            }
        }

        private void HandleUI()
        {
            CurrentNameTextBox.IsEnabled = !isRunning;

            InputTextBox.IsEnabled = isRunning;
            ToScanTextBlock.IsEnabled = isRunning;
            ProgressTextBlock.IsEnabled = isRunning;
            TimeElapsedTextBlock.IsEnabled = isRunning;
        }

        private void GameThread()
        {
            while (isRunning)
            {
                try
                {
                    Dispatcher.Invoke(() =>
                    {
                        ToScanTextBlock.Text = products[scanIndex].Name;

                        ProgressTextBlock.Text = scanIndex + "/" + products.Count;

                        TimeSpan difference = DateTime.Now - gameStarted;

                        TimeElapsedTextBlock.Text = difference.Minutes.ToString("D2") + ":" + difference.Seconds.ToString("D2");
                    });
                }
                catch
                {

                }

                Thread.Sleep(1);
            }
        }

        public override void OnUnload()
        {
            isRunning = false;
        }
    }
}
