using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using MyShop.Core.Models;
using System.Linq;

namespace MyShop.DataAccess.InMemory
{
    public class ProductRepository
    {
        ObjectCache cache = MemoryCache.Default;
        List<Product> products;

        public ProductRepository()
        {
            products = cache["products"] as List<Product>;
            if (products == null)
            {
                products = new List<Product>();
            }
        }

        public void Commit()
        {
            cache["products"] = products;
        }

        public void Insert(Product p) {
            products.Add(p);
        }

        public void Update(Product product) {
            Product productToUpdate = products.Find(p => p.Id == p.Id); //find the product we want tp update in the list

            if (productToUpdate != null)
            {
                productToUpdate = product;                
            }
            else {
                throw new Exception("Product with Id " + product.Id + " Not found!");
            }
        }

        public Product Find(string Id) {
            Product product = products.Find(p => p.Id == Id);
            if (product != null)
            {
                return product;
            }
            else
            {
                throw new Exception("Product with Id " + Id + " Not found!");
            }
        }

        public IQueryable<Product> Collection() {
            return  products.AsQueryable();
        }

        public void Delete(string Id) {
            Product product = products.Find(p => p.Id == Id);
            if (product != null)
            {
                products.Remove(product);
            }
            else
            {
                throw new Exception("Product with Id " + Id + " Not found!");
            }
        }

        
    }
}
