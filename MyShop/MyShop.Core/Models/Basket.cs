using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.Models
{
    public class Basket : BaseEntity
    {
        public virtual ICollection<BasketItem> BasketItems { get; set; }

        public Basket()
        {
            this.BasketItems = new List<BasketItem>();
        }

        public decimal BasketTotal()
        {
            decimal? total = (from item in BasketItems
                              select (int?)item.Quantity * item.Price).Sum();

            return total ?? decimal.Zero;
        }

        public int BasketItemCount()
        {
            int? total = (from item in BasketItems
                                  select item.Quantity).Sum();

            return total ?? 0;
        }
    }
}
