using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyShop.WebUI.Controllers;
using MyShop.Core.Contracts;
using MyShop.Services;
using MyShop.Core.Models;
using System.Web.Mvc;
using MyShop.Core.ViewModels;
using System.Web;
using System.Linq;
using MyShop.WebUI.Tests.Mocks;
using System.Collections.Generic;
using System.Security.Principal;

namespace MyShop.WebUI.Tests.Controllers
{
    [TestClass]
    public class BasketControllerTest
    {


        [TestMethod]
        public void CanAddBasketItem()
        {
            //Setup
            IRepository<Basket> baskets = new MockRepository<Basket>();
            IRepository<Product> products = new MockRepository<Product>();
            IRepository<Order> orders = new MockRepository<Order>();
            IRepository<Customer> customers = new MockRepository<Customer>();

            IBasketService basketService = new BasketService(products, baskets);
            IOrderService orderService = new OrderService(orders);
            var controller = new BasketController(basketService, orderService, customers);

            var httpContext = new MockHttpContext();
            controller.ControllerContext = new ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);

            //Act
            //to test the service directly we would do this
            //basketService.AddToBasket(httpContext, "1");
            //to test the service via the controller we do this
            controller.AddToBasket("1");
            Basket basket = baskets.Collection().FirstOrDefault();

            //Assert
            Assert.IsNotNull(basket);
            Assert.AreEqual(1, basket.BasketItems.Count);
            Assert.AreEqual("1", basket.BasketItems.ToList().FirstOrDefault().ProductId);
        }

        [TestMethod]
        public void CanRemoveBasketItem()
        {
            //Setup
            IRepository<Basket> baskets = new MockRepository<Basket>();
            IRepository<Product> products = new MockRepository<Product>();
            IRepository<Order> orders = new MockRepository<Order>();
            IRepository<Customer> customers = new MockRepository<Customer>();

            IBasketService basketService = new BasketService(products, baskets);
            IOrderService orderService = new OrderService(orders);
            var controller = new BasketController(basketService, orderService, customers);

            Basket basket = new Basket();
            BasketItem basketItem = new BasketItem() { ProductId = "1", Quantity = 1 };
            basket.BasketItems.Add(basketItem);
            baskets.Insert(basket);

            var httpContext = new MockHttpContext();
            httpContext.Request.Cookies.Add(new HttpCookie("eCommerceBasket") { Value = basket.Id });
            controller.ControllerContext = new ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);

            //Act
            //to test the service directly we would do this
            //basketService.RemoveFromBasket(httpContext, basketItem.Id);
            //to test the service via the controller we do this
            controller.RemoveFromBasket(basketItem.Id);

            //Assert
            Assert.AreEqual(0, basket.BasketItems.Count);
        }

        [TestMethod]
        public void CanGetBasketItems()
        {
            //Setup
            IRepository<Product> products = new MockRepository<Product>();
            products.Insert(new Product() { Id = "1", Price = 10.00m });
            products.Insert(new Product() { Id = "2", Price = 5.00m });

            IRepository<Basket> baskets = new MockRepository<Basket>();
            Basket basket = new Basket();
            basket.BasketItems.Add(new BasketItem() { ProductId = "1", Quantity = 2, BasketId = basket.Id });
            basket.BasketItems.Add(new BasketItem() { ProductId = "2", Quantity = 1, BasketId = basket.Id });
            baskets.Insert(basket);

            IRepository<Customer> customers = new MockRepository<Customer>();

            IBasketService basketService = new BasketService(products, baskets);
            IRepository<Order> orders = new MockRepository<Order>();
            IOrderService orderService = new OrderService(orders);
            var controller = new BasketController(basketService, orderService, customers);
            var httpContext = new MockHttpContext();
            httpContext.Request.Cookies.Add(new HttpCookie("eCommerceBasket") { Value = basket.Id });

            controller.ControllerContext = new ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);

            //Act
            //to test the service directly we would do this
            //var basketList = basketService.GetBasketItems(httpContext);
            //to test the service via the controller we do this
            var result = controller.Index() as ViewResult;
            var basketList = (List<BasketItemViewModel>)result.ViewData.Model;

            //Assert
            Assert.AreEqual(2, basketList.Count);

        }

        [TestMethod]
        public void CanGetSummaryParitalView()
        {
            //Setup
            IRepository<Product> products = new MockRepository<Product>();
            products.Insert(new Product() { Id = "1", Price = 10.00m });
            products.Insert(new Product() { Id = "2", Price = 5.00m });

            IRepository<Basket> baskets = new MockRepository<Basket>();
            Basket basket = new Basket();
            basket.BasketItems.Add(new BasketItem() { ProductId = "1", Quantity = 2 });
            basket.BasketItems.Add(new BasketItem() { ProductId = "2", Quantity = 1 });
            baskets.Insert(basket);

            IBasketService basketService = new BasketService(products, baskets);
            IRepository<Order> orders = new MockRepository<Order>();
            IRepository<Customer> customers = new MockRepository<Customer>();

            IOrderService orderService = new OrderService(orders);
            var controller = new BasketController(basketService, orderService, customers);
            var httpContext = new MockHttpContext();
            httpContext.Request.Cookies.Add(new HttpCookie("eCommerceBasket") { Value = basket.Id });

            controller.ControllerContext = new ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);

            //Act
            //if we were testing the service directly we would do this
            //var basketSummary = basketService.GetBasketSummary(httpContext);
            //to test the servce via the controller we do this
            var result = controller.BasketSummary() as PartialViewResult;
            var basketSummary = (BasketSummaryViewModel)result.ViewData.Model;

            //Assert
            Assert.AreEqual(3, basketSummary.BasketCount);
            Assert.AreEqual(25.00m, basketSummary.BasketTotal);

        }

        [TestMethod]
        public void CanCheckoutAndCreateOrder()
        {
            //Setup
            IRepository<Product> products = new MockRepository<Product>();
            products.Insert(new Product() { Id = "1", Price = 10.00m });
            products.Insert(new Product() { Id = "2", Price = 5.00m });

            IRepository<Basket> baskets = new MockRepository<Basket>();
            Basket basket = new Basket();
            basket.BasketItems.Add(new BasketItem() { ProductId = "1", Quantity = 2, BasketId = basket.Id });
            basket.BasketItems.Add(new BasketItem() { ProductId = "2", Quantity = 1, BasketId = basket.Id });
            baskets.Insert(basket);

            IBasketService basketService = new BasketService(products, baskets);
            IRepository<Customer> customers = new MockRepository<Customer>();
            customers.Insert(new Customer() { Id = "1", Email = "brett@completecoder.net", ZipCode="90210" });

            IRepository<Order> orders = new MockRepository<Order>();
            IOrderService orderService = new OrderService(orders);
            var controller = new BasketController(basketService, orderService, customers);

            var httpContext = new MockHttpContext();
            httpContext.Request.Cookies.Add(new HttpCookie("eCommerceBasket") { Value = basket.Id });

            IPrincipal FakeUser = new GenericPrincipal(new GenericIdentity("brett@completecoder.net", "Forms"), null);
            httpContext.User = FakeUser;

            controller.ControllerContext = new ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);
            Order order = new Order();
            

            //Act
            controller.Checkout(order);
            

            //Assert
            //Test our actual order object
            Assert.AreEqual(2, order.orderItems.Count);
            Assert.AreEqual(0, basket.BasketItems.Count);
            //then confirm the db has also been updated.
            Order orderInRepo = orders.Find(order.Id);
            Assert.AreEqual(2, orderInRepo.orderItems.Count);
            

        }


    }

}