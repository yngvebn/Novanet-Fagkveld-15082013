﻿namespace ExternalApi.Contracts.Orders
{
    public class Order
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
    }
}