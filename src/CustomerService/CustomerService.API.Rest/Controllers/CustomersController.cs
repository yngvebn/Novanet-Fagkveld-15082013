using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AttributeRouting.Web.Http;
using AutoMapper;
using CustomerService.API.Rest.Models;

namespace CustomerService.API.Rest.Controllers
{
    public class CustomersController : ApiController
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomersController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [GET("api/v1/customers")]
        public IEnumerable<Customer> Get()
        {
            return Mapper.Map<List<Customer>>(_customerRepository.All());
        }

        [GET("api/v1/customers/{customerId}")]
        public Customer Get(int customerId)
        {
            return Mapper.Map<Customer>(_customerRepository.Find(customerId));
        }

    }
}