using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExternalApi.Contracts.Orders;
using Rikstoto.ExternalApi.Contracts;

namespace OrderService.API.External.Providers
{
    public class FillCustomerOrders: IProvideDataFor<CustomerWithOrders>, IMustProvideDataFirst
    {
        private readonly IOrderRepository _orderRepository;

        public FillCustomerOrders(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public void Fill(CustomerWithOrders model)
        {
            model.Orders = _orderRepository.All().Where(c => c.CustomerId == model.CustomerId).Select(o => new Order()
                {
                    OrderId = o.OrderId,
                    ProductId = o.ProductSku
                }).ToList();
        }
    }
}
