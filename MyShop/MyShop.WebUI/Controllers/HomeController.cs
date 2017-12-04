using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class HomeController : Controller
    {
        IRepository<Product> context;
        IRepository<ProductCategory> productCategories;

        public HomeController(IRepository<Product> productContext, IRepository<ProductCategory> productCategoryContext)
        {
            this.context = productContext;
            this.productCategories = productCategoryContext;
        }

        public ActionResult Admin() {
            return View();
        }

        // GET: ProductManager
        public ActionResult Index(string Category=null)
        {
            List<Product> products;
            List<ProductCategory> categories = productCategories.Collection().ToList();

           

            if (Category == null)
            {
                products = context.Collection().ToList();
            }
            else {
                products = context.Collection().Where(p=>p.Category==Category).ToList();
            }

            ProductListViewModel model = new ProductListViewModel();
            model.ProductCategories = categories;
            model.Products = products;

            return View(model);
        }

        public ActionResult Details(string Id) {
            Product product = context.Find(Id);
            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(product);
            }
        }
    }
}