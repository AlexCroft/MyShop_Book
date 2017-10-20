using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc5;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.DataAccess.SQL;

namespace MyShop.WebUI
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();
            
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            container.RegisterType<IRepository<Product>, SQLRepository<Product>>();
            container.RegisterType<IRepository<ProductCategory>, SQLRepository<ProductCategory>>();
        }
    }
}