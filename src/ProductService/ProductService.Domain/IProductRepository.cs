using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductService.Domain;

namespace ProductService
{
    public interface IProductRepository
    {
        IList<Product> All();
        Product Find(int productSku);
    }

    public class ProductRepository : IProductRepository
    {
        public IList<Product> All()
        {
            return new List<Product>()
                {
                    new Product(){ Color = "Black", Manufacturer = "Logitech", Name = "Harmony One", ProductSku = 1, ListPrice = 99},
                    new Product(){ Color = "Gray", Manufacturer = "Leap", Name = "Leap Motion", ProductSku = 2, ListPrice = 80},
                    new Product(){ Color = "White", Manufacturer = "Avent", Name = "Babycall", ProductSku = 3, ListPrice = 119},
                    new Product(){ Color = "Black", Manufacturer = "Yamaha", Name = "P120", ProductSku = 4, ListPrice = 1100},
                    new Product(){ Color = "Sunburst", Manufacturer = "Fender", Name = "Stratocaster", ProductSku = 5, ListPrice = 2999},
                    new Product(){ Color = "Chocolate Brown", Manufacturer = "Nestlé", Name = "Lion bar", ProductSku = 6, ListPrice = 2},
                    new Product(){ Color = "White", Manufacturer = "Apple", Name = "iPad 3", ProductSku = 7, ListPrice = 899}
                };
        }

        public Product Find(int productSku)
        {
            return All().SingleOrDefault(product => productSku == productSku);
        }
    }
}
