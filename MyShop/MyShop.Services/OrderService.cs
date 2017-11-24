using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using System.Collections.Generic;

namespace MyShop.Services
{
    public class OrderService : IOrderService
    {
        IRepository<Order> orderContext;

        public OrderService(IRepository<Order> OrderContext) {
            this.orderContext = OrderContext;
        }
        public void CreateOrder(Order baseOrder, List<BasketItemViewModel> basketItems) {
            //The base order contains the user information such as address
            //so now we need to copy across all the product detaisl from the basket into the order.

            foreach (var item in basketItems) {
                baseOrder.orderItems.Add(new OrderItem()
                {
                    ProductId = item.Id,
                    ProductName = item.ProductName,
                    Image = item.Image,
                    Price = item.Price,
                    Quantity = item.Quantity
                });
            }
            orderContext.Insert(baseOrder);
            orderContext.Commit(); 
        }
    }
}
