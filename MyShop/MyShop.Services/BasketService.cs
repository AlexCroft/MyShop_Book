using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using System;
using System.Linq;
using System.Web;

namespace MyShop.Services
{
    public class BasketService : IBasketService
    {
        IRepository<Product> productContext;
        IRepository<Basket> basketContext;
        public const string BasketSessionName = "eCommerceBasket";

        public BasketService(IRepository<Product> ProductContext, IRepository<Basket> BasketContext){
            this.productContext = ProductContext;
            this.basketContext = BasketContext;

        }

        private Basket CreateNewBasket(HttpContextBase httpContext)
        {
            //create a new basket.

            //first create a new cookie.
            HttpCookie cookie = new HttpCookie(BasketSessionName);
            //now create a new basket and set the creation date.
            Basket basket = new Basket();

            //add and persist in the dabase.
            basketContext.Insert(basket);
            basketContext.Commit();

            //add the basket id to a cookie
            cookie.Value = basket.Id;
            cookie.Expires = DateTime.Now.AddDays(1);
            httpContext.Response.Cookies.Add(cookie);

            return basket;
        }

        public bool AddToBasket(HttpContextBase httpContext, string productId, int quantity)
        {
            bool success = true;

            Product p = productContext.Find(productId);
            Basket basket = GetBasket(httpContext, true);
            BasketItem item = basket.BasketItems.FirstOrDefault(i => i.ProductId == productId);

            if (item == null)
            {
                item = new BasketItem()
                {
                    BasketId = basket.Id,
                    ProductId = productId,
                    Quantity = quantity,
                    ProductName=p.Name,
                    Price=p.Price
                };
                basket.BasketItems.Add(item);
            }
            else
            {
                item.Quantity = item.Quantity + quantity;
            }
            basketContext.Commit();

            return success;
        }

        public bool RemoveFromBasket(HttpContextBase httpContext, string itemId)
        {
            bool success = true;

            Basket basket = GetBasket(httpContext, true);
            BasketItem item = basket.BasketItems.FirstOrDefault(i => i.Id == itemId);

            if (item != null)
            {
                basket.BasketItems.Remove(item);
            }

            basketContext.Commit();

            return success;
        }

        public BasketSummaryViewModel BasketSummary(HttpContextBase httpContext) {
            
            Basket basket = GetBasket(httpContext, false);
            BasketSummaryViewModel model = new BasketSummaryViewModel(0, 0);
            if (basket != null)
            {
                model.BasketCount = basket.BasketItemCount();
                model.BasketTotal = basket.BasketTotal();
            }

            return model;
            
        }

        public Basket GetBasket(HttpContextBase httpContext, bool createIfNull = true)
        {
            HttpCookie cookie = httpContext.Request.Cookies.Get(BasketSessionName);
            
            Basket basket = new Basket();

            if (cookie != null)
            {
                string basketId = cookie.Value;
                if (!string.IsNullOrEmpty(basketId))
                {
                    basket = basketContext.Find(basketId);
                }
                else
                {
                    if (createIfNull)
                    {
                        basket = CreateNewBasket(httpContext);
                    }
                }
            }
            else
            {
                if (createIfNull)
                {
                    basket = CreateNewBasket(httpContext);
                }
            }

            return basket;
        }
    }
}
