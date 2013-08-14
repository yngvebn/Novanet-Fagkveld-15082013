using System;

namespace OrderService.Domain
{
    public class Order
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int ProductSku { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
    }
}