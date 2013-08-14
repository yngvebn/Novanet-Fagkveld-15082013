using System.Collections.Generic;

namespace ExternalApi.Contracts.Orders
{
    public class CustomerWithOrders
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }

        public List<Order> Orders { get; set; }
    }
}