using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Services
{
    public class OrderService
    {
        IRepository<Order> Orders;
        IRepository<Product> Products;
        public void CreateOrder(Order baseOrder, Basket basket) {
            //copy the basketItems to the Order Items

        }
    }
}
