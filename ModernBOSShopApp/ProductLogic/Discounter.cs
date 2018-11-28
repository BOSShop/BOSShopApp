using System;

namespace ModernBOSShopApp.ProductLogic
{
    public class Discounter
    {
        public int discount;

        public Discounter(int discount)
        {
            this.discount = discount;
        }

        public virtual bool IsInDiscount(Product product)
        {
            throw new NotImplementedException();
        }

        public virtual decimal GetWithDiscount(Product product)
        {
            if (!IsInDiscount(product))
            {
                return product.Price;
            }
            else
            {
                decimal multiplier = discount / 100m;

                return product.Price * (1m - multiplier);
            }
        }

        public virtual string GetDiscountText()
        {
            return "";
        }
    }
}