using System;
using System.IO;
using System.Linq;

namespace ModernBOSShopApp.ProductLogic
{
    public class FileManager
    {
        public string GetBasePath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BOSShop");
        }

        public string GetPath(params string[] arguments)
        {
            return arguments.Aggregate(GetBasePath(), (acc, p) => Path.Combine(acc, p));
        }

        public void Initialize()
        {
            CreateFolderIfNotExists(GetBasePath());
            CreateFolderIfNotExists(GetPath("Images"));
            CreateFileIfNotExists(GetPath("Save.json"));
        }

        private void CreateFolderIfNotExists(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        private void CreateFileIfNotExists(string path)
        {
            if (!File.Exists(path))
                File.Create(path).Close();
        }
    }
}