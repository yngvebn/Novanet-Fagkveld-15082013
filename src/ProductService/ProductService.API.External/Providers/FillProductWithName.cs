using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExternalApi.Contracts.Orders;
using Rikstoto.ExternalApi.Contracts;

namespace ProductService.API.External.Providers
{
    public class FillProductWithName: IProvideDataFor<CustomerWithOrders>
    {
        private readonly IProductRepository _productRepository;

        public FillProductWithName(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }


        public void Fill(CustomerWithOrders model)
        {
            foreach (var order in model.Orders)
            {
                order.ProductName = _productRepository.Find(order.ProductId).Name;
            }
        }
    }
}
