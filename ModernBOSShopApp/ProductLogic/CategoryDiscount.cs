namespace ModernBOSShopApp.ProductLogic
{
    public class CategoryDiscount : Discounter
    {
        public string productCategory;

        public CategoryDiscount(string productCategory, int discount) : base(discount)
        {
            this.productCategory = productCategory;
        }

        public override bool IsInDiscount(Product product)
        {
            return product.Category == productCategory;
        }

        public override string GetDiscountText()
        {
            return "Produkte der Kategorie \"" + productCategory + "\" sind um " + discount + "% billiger";
        }
    }
}