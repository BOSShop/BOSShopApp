namespace ModernBOSShopApp.ProductLogic
{
    public class Product
    {
        public string Name { get; set; }
        public string ScanText { get; set; }
        public string Category { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }

        public Product(string name, string scanText, string category, int count, decimal price)
        {
            Name = name;
            ScanText = scanText;
            Category = category;
            Count = count;
            Price = price;
        }
    }
}