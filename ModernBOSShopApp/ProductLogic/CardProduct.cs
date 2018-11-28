namespace ModernBOSShopApp.ProductLogic
{
    public class CardProduct
    {
        public string Name { get; private set; }
        public int Count { get; set; }
        public decimal Price { get; private set; }
        public decimal CurrentPrice { get; private set; }
        public decimal WholePrice { get; private set; }

        public CardProduct(string name, int count, decimal price, decimal currentPrice)
        {
            Name = name;
            Count = count;
            Price = price;
            CurrentPrice = currentPrice;

            CalculateWholePrice();
        }

        public void CalculateWholePrice()
        {
            WholePrice = (CurrentPrice * Count);
        }
    }
}