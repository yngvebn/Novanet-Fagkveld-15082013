using System.Collections.Generic;

namespace ExternalApi.API.Rest.Models
{
    public class CustomerWithOrders
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }

        public List<Order> Orders { get; set; }
    }

    public class Order
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
    }
}