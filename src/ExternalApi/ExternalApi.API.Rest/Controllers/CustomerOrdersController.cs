using System;
using System.Web.Http;
using AttributeRouting.Web.Http;
using ExternalApi.API.Rest.Infrastructure;
using ExternalApi.API.Rest.Infrastructure.Mapping;
using ExternalApi.API.Rest.Models;

namespace ExternalApi.API.Rest.Controllers
{
    public class CustomerOrdersController: ApiController
    {
        [GET("api/v1/customers/{customerId}/orders"), HttpGet]
        public CustomerWithOrders CustomerWithOrders(int customerId)
        {
            throw new NotImplementedException();
        }
    }
}