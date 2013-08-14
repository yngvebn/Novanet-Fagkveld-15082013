using System.Collections.Generic;
using System.Web.Http;
using AttributeRouting.Web.Http;
using AutoMapper;
using ProductService.API.Rest.Models;

namespace ProductService.API.Rest.Controllers
{
    public class ProductsController : ApiController
    {
        private readonly IProductRepository _ProductRepository;

        public ProductsController(IProductRepository ProductRepository)
        {
            _ProductRepository = ProductRepository;
        }

        [GET("api/v1/Products")]
        public IEnumerable<Product> Get()
        {
            return Mapper.Map<List<Product>>(_ProductRepository.All());
        }

        [GET("api/v1/Products/{productSku}")]
        public Product Get(int productSku)
        {
            return Mapper.Map<Product>(_ProductRepository.Find(productSku));
        }

    }
}