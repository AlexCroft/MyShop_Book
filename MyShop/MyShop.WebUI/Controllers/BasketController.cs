using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class BasketController : Controller
    {
        IBasketService basketService;
        IOrderService orderService;

        public BasketController(IBasketService BasketService, IOrderService OrderService) {
            this.basketService = BasketService;
            this.orderService = OrderService;
        }
        // GET: Basket
        public ActionResult Index()
        {
            var model = basketService.GetBasketItems(this.HttpContext);

            return View(model);
        }

        public ActionResult AddToBasket(string id)
        {
            basketService.AddToBasket(this.HttpContext, id);//always add one to the basket

            return RedirectToAction("Index");
        }

        public ActionResult RemoveFromBasket(string id)
        {
            basketService.RemoveFromBasket(this.HttpContext, id);//always add one to the basket

            return RedirectToAction("Index");
        }

        public ActionResult Checkout() {
            return View();
        }

        [HttpPost]
        public ActionResult Checkout(Order order) {
            var basketItems = basketService.GetBasketItems(this.HttpContext);

            orderService.CreateOrder(order, basketItems);
            basketService.ClearBasket(this.HttpContext);

            return RedirectToAction("ThankYou", new { OrderId = order.Id });
        }

        public ActionResult ThankYou(string OrderId) {
            ViewBag.OrderId = OrderId;
            return View();
        }


        public PartialViewResult BasketSummary() {
            var basketSummary = basketService.GetBasketSummary(this.HttpContext);
            return PartialView("BasketSummary", basketSummary);
        }
    }
}