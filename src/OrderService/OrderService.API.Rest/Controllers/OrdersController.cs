using System.Collections.Generic;
using System.Web.Http;
using AttributeRouting.Web.Http;
using AutoMapper;
using OrderService.API.Rest.Models;

namespace OrderService.API.Rest.Controllers
{
    public class OrdersController : ApiController
    {
        private readonly IOrderRepository _orderRepository;

        public OrdersController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [GET("api/v1/orders")]
        public IEnumerable<Order> Get()
        {
            return Mapper.Map<List<Order>>(_orderRepository.All());
        }

        [GET("api/v1/orders/{orderId}")]
        public Order Get(int orderId)
        {
            return Mapper.Map<Order>(_orderRepository.Find(orderId));
        }

    }
}