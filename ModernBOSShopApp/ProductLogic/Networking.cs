using Germanen.GUNet.Attributes;
using Germanen.GUNet.Attributes.Default;
using Germanen.GUNet.Client;
using Germanen.GUNet.Settings.Client;
using Germanen.GUNet.Utils;
using System.Net;
using System.Threading;

namespace ModernBOSShopApp.ProductLogic
{
    public class Networking
    {
        public GUNetClient Client { private set; get; }
        public IAttributeManager AttributeManager { private set; get; }

        public Networking()
        {
            GUN.Init();

            AttributeManager = new AttributeManager();

            Client = new GUNetClient(new GUNClientSettings(AttributeManager, "1.0", IPAddress.Parse("127.0.0.1"), 8888, false, true));

            AttributeManager.AddClass(this);

            new Thread(() =>
            {
                while (Client != null)
                {
                    try
                    {
                        if (!Client.Connected)
                        {
                            if (!Client.Connecting)
                            {
                                try
                                {
                                    Client.ConnectToServer();
                                }
                                catch
                                {

                                }
                            }
                        }
                        else
                        {
                            Client.HandleNetworkingData();
                        }

                        MainWindow.Instance.Dispatcher.Invoke(() =>
                        {
                            if (Client != null && Client.Connected)
                                MainWindow.Instance.Title = "BOSShop - Verbunden";
                            else
                                MainWindow.Instance.Title = "BOSShop";
                        });
                    }
                    catch
                    {

                    }
                    Thread.Sleep(1);
                }
            }).Start();
        }

        public void CloseConnection()
        {
            Client.DisconnectFromServer();
            Client = null;
        }
    }
}