using System;
using System.Web.Http;
using AttributeRouting.Web.Http;
using ExternalApi.API.Rest.Infrastructure;
using ExternalApi.API.Rest.Infrastructure.Mapping;
using ExternalApi.API.Rest.Models;
using Rikstoto.ExternalApi.Contracts;

namespace ExternalApi.API.Rest.Controllers
{
    

    public class CustomerOrdersController: ApiController
    {
        private readonly IManageDataProviders _dataProviders;
        private readonly IMapper _mapper;

        public CustomerOrdersController(IManageDataProviders dataProviders, IMapper mapper)
        {
            _dataProviders = dataProviders;
            _mapper = mapper;
        }

        [GET("api/v1/customers/{customerId}/orders"), HttpGet]
        public CustomerWithOrders CustomerWithOrders(int customerId)
        {
            var customerWithOrders = new Contracts.Orders.CustomerWithOrders()
                {
                    CustomerId = customerId
                };

            _dataProviders.FillModelFromProviders(customerWithOrders);

            var response = _mapper.Map<CustomerWithOrders>(customerWithOrders);

            return response;
        }
    }
}