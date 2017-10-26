using System.Web;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;

namespace MyShop.Core.Contracts
{
    public interface IBasketService
    {
        bool AddToBasket(HttpContextBase httpContext, string productId, int quantity);
        bool RemoveFromBasket(HttpContextBase httpContext, string productId);
        Basket GetBasket(HttpContextBase httpContext, bool createIfNull);
        BasketSummaryViewModel BasketSummary(HttpContextBase httpContext);
    }
}