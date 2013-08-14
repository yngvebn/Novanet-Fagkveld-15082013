using AutoMapper;

namespace ProductService.API.Rest.App_Start
{
    public static class MappingConfig
    {
        public static void Configure()
        {
            Mapper.CreateMap<Domain.Product, Models.Product>();
        }
    }
}