namespace ModernBOSShopApp.ProductLogic
{
    public class ProductDiscount : Discounter
    {
        public string productName;

        public ProductDiscount(string productName, int discount) : base(discount)
        {
            this.productName = productName;
        }

        public override bool IsInDiscount(Product product)
        {
            return product.Name == productName;
        }

        public override string GetDiscountText()
        {
            return "\"" + productName + "\" ist um " + discount + "% billiger";
        }
    }
}