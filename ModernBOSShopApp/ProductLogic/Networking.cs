using Germanen.GUNet.Attributes;
using Germanen.GUNet.Attributes.Client;
using Germanen.GUNet.Attributes.Default;
using Germanen.GUNet.Client;
using Germanen.GUNet.Settings.Client;
using Germanen.GUNet.Utils;
using Germanen.GUNet.Utils.Client;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
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
            try
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
            catch
            {

            }
        }

        public void CloseConnection()
        {
            Client.DisconnectFromServer();
            Client = null;
        }

        [OnConnectionEstablished]
        public void OnConnectionEstablished()
        {
            string downloadPath = MainWindow.Instance.fileManager.GetPath("Download");

            if (Directory.Exists(downloadPath))
            {
                Directory.Delete(downloadPath, true);
                Client.Send("CUpdateFinished");
            }
        }


        [OnPackage("SUpdate")]
        public void OnUpdate(PackageFromServerData data)
        {
            data.Respond(Client, "CUpdateStarted");

            WebClient client = new WebClient();

            string downloadPath = MainWindow.Instance.fileManager.GetPath("Download");

            MainWindow.Instance.fileManager.CreateFolderIfNotExists(downloadPath);

            string zipFile = MainWindow.Instance.fileManager.GetPath("Update.zip");

            client.DownloadFile("http://127.0.0.1:8889/Update.zip", zipFile);

            ZipFile.ExtractToDirectory(zipFile, downloadPath);

            File.Delete(zipFile);

            string[] executables = Directory.GetFiles(downloadPath, "*.exe", SearchOption.AllDirectories);

            if (executables.Length == 1)
            {
                Process.Start(executables[0]);
                Environment.Exit(0);
            }
        }
    }
}