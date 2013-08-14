using System.Collections.Generic;
using System.Linq;
using OrderService.Domain;

namespace OrderService
{
    public interface IOrderRepository
    {
        IList<Order> All();
        Order Find(int orderId);
    }

    public class OrderRepository : IOrderRepository
    {
        public IList<Order> All()
        {
            return new List<Order>()
                {
                    new Order(){ CustomerId = 1, Discount = 0, OrderId = 1, ProductSku = 3, Quantity = 2},
                    new Order(){ CustomerId = 2, Discount = 0, OrderId = 2, ProductSku = 1, Quantity = 1},
                    new Order(){ CustomerId = 1, Discount = 0, OrderId = 3, ProductSku = 5, Quantity = 4},
                    new Order(){ CustomerId = 4, Discount = 10, OrderId = 4, ProductSku = 3, Quantity = 1},
                    new Order(){ CustomerId = 5, Discount = 0, OrderId = 5, ProductSku = 2, Quantity = 1},
                    new Order(){ CustomerId = 3, Discount = 0, OrderId = 5, ProductSku = 6, Quantity = 21},
                    new Order(){ CustomerId = 5, Discount = 15, OrderId = 5, ProductSku = 7, Quantity = 1},
                };
        }

        public Order Find(int orderId)
        {
            return All().SingleOrDefault(order => order.OrderId == orderId);
        }
    }
}