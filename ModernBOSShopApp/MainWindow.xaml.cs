using ModernBOSShopApp.Animation;
using ModernBOSShopApp.Pages;
using ModernBOSShopApp.ProductLogic;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;

namespace ModernBOSShopApp
{
    public partial class MainWindow : Window
    {
        public static MainWindow Instance { private set; get; }

        public FileManager fileManager;

        public ProductManager productManager;

        public Dictionary<string, BasePage> pages;
        public List<string> history;
        public BasePage currentPage;

        private bool hasSetDay;
        private DayOfWeek lastDay;

        private Networking networking;

        public MainWindow()
        {
            Instance = this;

            fileManager = new FileManager();
            fileManager.Initialize();

            productManager = new ProductManager();

            pages = new Dictionary<string, BasePage>();
            history = new List<string>();

            AddPage(new MainMenuPage());
            AddPage(new ProductsPage());
            AddPage(new SellPage());
            AddPage(new AddProductPage());
            AddPage(new ProductOfTheDayPage());
            AddPage(new SecretGamePage());
            AddPage(new ScanFastGamePage());
            AddPage(new ScanListGamePage());

            InitializeComponent();

            LoadPageAsync("MainMenuPage");

            networking = new Networking();

            Timer updater = new Timer
            {
                Interval = 1000
            };
            updater.Elapsed += UpdateTimer_Tick;
            updater.Start();
        }

        private void AddPage(BasePage page)
        {
            pages.Add(page.PageName, page);
        }

        private void UpdateTimer_Tick(object sender, ElapsedEventArgs e)
        {
            DayOfWeek day = DateTime.Now.DayOfWeek;

            if(day != lastDay || !hasSetDay)
            {
                lastDay = day;
                hasSetDay = true;

                Dispatcher.Invoke(() =>
                {
                    LoadProductOfTheDay();
                });
            }

            Dispatcher.Invoke(() =>
            {
                ProductOfTheDayTextBlock.Text = productManager.discount != null ? productManager.discount.GetDiscountText() : "";
            });
        }

        public void LoadLastPage(bool addToHistory = true)
        {
            if (history.Count > 1)
                LoadPageAsync(history[1], addToHistory);
        }

        public BasePage GetLastPage()
        {
            if (history.Count > 1)
                return pages[history[1]];
            else
                return null;
        }

        public async void LoadPageAsync(string pageName, bool addToHistory = true)
        {
            await LoadPage(pageName, addToHistory);
        }

        public async Task LoadPage(string pageName, bool addToHistory = true)
        {
            if (pages.ContainsKey(pageName))
            {
                if (pages[pageName] != null)
                {
                    if (currentPage != null)
                    {
                        PageTransitionAnimation animation = pages[pageName].animation.GetOpposite();
                        await currentPage.StartAnimation(false, animation);
                        currentPage.OnUnload();
                    }

                    currentPage = pages[pageName];
                    PageFrame.Content = pages[pageName];

                    if (addToHistory)
                    {
                        if (history.Count >= 5)
                        {
                            history.RemoveAt(history.Count - 1);
                        }

                        history.Insert(0, pageName);
                    }

                    pages[pageName].OnLoad();
                    await pages[pageName].StartAnimation(true);
                }
            }
        }

        public void LoadProductOfTheDay()
        {
            LoadPageAsync("ProductOfTheDayPage", currentPage.PageName != "ProductOfTheDayPage");
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed && e.ChangedButton == MouseButton.Right)
            {
                LoadProductOfTheDay();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
            networking.CloseConnection();
        }
    }
}