using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class OrderManagerController : Controller
    {
        IOrderService orderService;

        public OrderManagerController(IOrderService OrderService) {
            this.orderService = OrderService;
        }
        // GET: OrderManager
        public ActionResult Index()
        {
            List<Order> orders = orderService.GetOrderList();
            return View(orders);
        }

        public ActionResult UpdateOrder(string Id) {
            ViewBag.Statuses = new List<string>() {
                "Order Created",
                "Payment Processed",
                "Order Shipped",
                "Order Complete"
            };
            Order order = orderService.GetOrder(Id);
            return View(order);
        }

        [HttpPost]
        public ActionResult UpdateOrder(Order updatedOrder, string Id)
        {
            //get the original order
            Order order = orderService.GetOrder(Id);

            //update it with whatever we want to update it with
            order.OrderStatus = updatedOrder.OrderStatus;
            orderService.UpdateOrder(order);

            return RedirectToAction("Index");
        }
    }
}