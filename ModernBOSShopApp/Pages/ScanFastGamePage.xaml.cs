using ModernBOSShopApp.Animation;
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
    /// Interaction logic for ScanFastGamePage.xaml
    /// </summary>
    public partial class ScanFastGamePage : BasePage
    {
        private bool isInGame = false;
        private bool gameRunning = false;

        private DateTime gameStarted;

        private int currentProgress;
        private int requiredProgress;

        private string currentName;

        private ObservableCollection<FastScanResult> results;

        public ScanFastGamePage()
        {
            animation = PageTransitionAnimation.SlideLeft;

            results = new ObservableCollection<FastScanResult>();

            InitializeComponent();

            ResultDataGrid.Columns.Add(new DataGridTextColumn { Header = "Name", Binding = new Binding("Name"), IsReadOnly = true, MinWidth = 120 });
            ResultDataGrid.Columns.Add(new DataGridTextColumn { Header = "Dauer", Binding = new Binding("TimeTook"), IsReadOnly = true, MinWidth = 150 });

            ProductsChanged();

            ProductManager.Instance.ProductsChangedEvent += ProductsChanged;

            VerifyInput();
        }

        private void ProductsChanged()
        {
            List<string> products = (from product in ProductManager.Instance.products select product.Name).ToList();

            ProductsComboBox.ItemsSource = products;
        }

        public override void OnLoad()
        {
            isInGame = false;
            gameRunning = false;

            ProductsComboBox.Text = "";
            ScanAmountTextBox.Text = "100";

            CurrentNameTextBox.Text = "";

            InputTextBox.Text = "";
            ProgressTextBlock.Text = "";
            TimeElapsedTextBlock.Text = "";
            ResultDataGrid.ItemsSource = results;

            results.Clear();

            HandleUI();

            VerifyInput();
        }

        private void ProductsComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            VerifyInput();
        }
        
        private void ScanAmountTextBox_TextInput(object sender, TextCompositionEventArgs e)
        {
            VerifyInput();
        }

        private void ScanAmountTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                VerifyInput();
            }
            catch
            {

            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (VerifyInput())
            {
                currentProgress = 0;
                requiredProgress = int.Parse(ScanAmountTextBox.Text);

                isInGame = true;

                HandleUI();
            }
        }

        private bool VerifyInput()
        {
            bool result = true;

            if (MainWindow.Instance.productManager != null)
                if (MainWindow.Instance.productManager.products != null)
                    if (MainWindow.Instance.productManager.GetProduct(ProductsComboBox.Text) == null)
                        result = false;

            if (ScanAmountTextBox != null)
                if (ScanAmountTextBox.Text != null)
                    if (!int.TryParse(ScanAmountTextBox.Text, out int i))
                        result = false;

            Dispatcher.Invoke(() =>
            {
                StartButton.IsEnabled = result && !isInGame;
            });

            return result;
        }

        private void HandleUI()
        {
            ProductsComboBox.IsEnabled = !isInGame && !gameRunning;
            ScanAmountTextBox.IsEnabled = !isInGame && !gameRunning;
            StartButton.IsEnabled = !isInGame && !gameRunning;

            CurrentNameTextBox.IsEnabled = isInGame && !gameRunning;

            InputTextBox.IsEnabled = isInGame && gameRunning;
            ProgressTextBlock.IsEnabled = isInGame && gameRunning;
            TimeElapsedTextBlock.IsEnabled = isInGame && gameRunning;
            ResultDataGrid.IsEnabled = isInGame;
        }

        public override void OnUnload()
        {
            isInGame = false;
            gameRunning = false;
        }

        private void GameThread()
        {
            while (isInGame && gameRunning)
            {
                try
                {
                    Dispatcher.Invoke(() =>
                    {
                        ProgressTextBlock.Text = currentProgress + "/" + requiredProgress;

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

        private void InputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                if (InputTextBox.Text == ProductsComboBox.Text)
                    currentProgress++;

                InputTextBox.Text = "";

                if(currentProgress == requiredProgress)
                {
                    gameRunning = false;

                    Dispatcher.Invoke(() =>
                    {
                        ProgressTextBlock.Text = "";

                        TimeElapsedTextBlock.Text = "";
                    });

                    currentProgress = 0;

                    TimeSpan difference = DateTime.Now - gameStarted;

                    results.Add(new FastScanResult(currentName, difference.Minutes.ToString("D2") + ":" + difference.Seconds.ToString("D2")));

                    Dispatcher.Invoke(() =>
                    {
                        CurrentNameTextBox.Text = "";
                    });

                    HandleUI();
                }
            }
        }

        private void CurrentNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                currentName = CurrentNameTextBox.Text;

                gameRunning = true;

                gameStarted = DateTime.Now;

                new Thread(GameThread).Start();

                HandleUI();

                InputTextBox.Focus();
            }
        }

        private void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            OnLoad();
        }

        private void BackToMenuButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.LoadPageAsync("MainMenuPage");
        }
    }
}