using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace ModernBOSShopApp.ProductLogic
{
    public class ProductManager
    {
        public static ProductManager Instance { private set; get; }

        public ObservableCollection<Product> products;

        public Action ProductsChangedEvent;

        public Discounter discount;

        public ProductManager()
        {
            LoadProducts();

            Instance = this;
        }

        public string GetSaveFilePath()
        {
            return MainWindow.Instance.fileManager.GetPath("Save.json");
        }

        public void LoadProducts()
        {
            string saveFileContent = File.ReadAllText(GetSaveFilePath());

            products = JsonConvert.DeserializeObject<ObservableCollection<Product>>(saveFileContent);

            if (products == null)
                products = new ObservableCollection<Product>();

            if (ProductsChangedEvent != null)
                ProductsChangedEvent.Invoke();
        }

        public Product GetProduct(string name)
        {
            name = name.ToLower();

            List<Product> found = (from product in products where product.Name.ToLower() == name || (product.ScanText != null && product.ScanText.ToLower() == name) select product).ToList();

            if (found.Count != 1)
                return null;
            else
                return found[0];
        }

        public void SetProduct(Product product)
        {
            for(int i = 0; i < products.Count; i++)
            {
                if(products[i].Name == product.Name && products[i].ScanText == product.ScanText)
                {
                    products[i] = product;
                    return;
                }
            }

            ProductChanged();
        }

        public CardProduct GetCardProduct(string name)
        {
            Product product = GetProduct(name);

            if (product == null)
                return null;

            if (discount == null)
                return new CardProduct(product.Name, 1, product.Price, product.Price);

            return new CardProduct(product.Name, 1, product.Price, discount.GetWithDiscount(product));
        }

        public void ProductChanged()
        {
            if (ProductsChangedEvent != null)
                ProductsChangedEvent.Invoke();

            SaveProducts();
        }

        public void SaveProducts()
        {
            File.WriteAllText(GetSaveFilePath(), JsonConvert.SerializeObject(products));
        }
    }
}