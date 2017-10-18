using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using MyShop.Core.Models;

namespace MyShop.DataAccess.InMemory
{
    public class ProductCategoryRepository
    {
        ObjectCache cache = MemoryCache.Default;
        List<ProductCategory> categories;

        public ProductCategoryRepository()
        {
            categories = cache["productcategories"] as List<ProductCategory>;
            if (categories == null)
            {
                categories = new List<ProductCategory>();
            }
        }

        public void Commit()
        {
            cache["productcategories"] = categories;
        }

        public void Insert(ProductCategory c)
        {
            categories.Add(c);
        }

        public void Update(ProductCategory productCategory)
        {
            ProductCategory categroryToUpdate = categories.Find(p => p.Id == p.Id); //find the product we want tp update in the list

            if (categroryToUpdate != null)
            {
                categroryToUpdate = productCategory;
            }
            else
            {
                throw new Exception("Categrory with Id " + productCategory.Id + " Not found!");
            }
        }

        public ProductCategory Find(string Id)
        {
            ProductCategory categrory = categories.Find(p => p.Id == Id);
            if (categrory != null)
            {
                return categrory;
            }
            else
            {
                throw new Exception("Categrory with Id " + Id + " Not found!");
            }
        }

        public IQueryable<ProductCategory> Collection()
        {
            return categories.AsQueryable();
        }

        public void Delete(string Id)
        {
            ProductCategory categrory = categories.Find(p => p.Id == Id);
            if (categrory != null)
            {
                categories.Remove(categrory);
            }
            else
            {
                throw new Exception("Categrory with Id " + Id + " Not found!");
            }
        }
    }
}
