using AutoMapper;

namespace OrderService.API.Rest.App_Start
{
    public static class MappingConfig
    {
        public static void Configure()
        {
            Mapper.CreateMap<Domain.Order, Models.Order>();
        }
    }
}