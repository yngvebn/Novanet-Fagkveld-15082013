using AutoMapper;
using ExternalApi.API.Rest.Infrastructure.Mapping;

namespace ExternalApi.API.Rest.Mappings
{
    public class CustomerWithOrders: IMappingConfiguration
    {
        public void Configure()
        {
            Mapper.CreateMap<Contracts.Orders.CustomerWithOrders, Models.CustomerWithOrders>();
        }
    }
}