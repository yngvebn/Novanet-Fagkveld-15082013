using AutoMapper;

namespace CustomerService.API.Rest
{
    public static class MappingConfig
    {
        public static void Configure()
        {
            Mapper.CreateMap<Domain.Customer, Models.Customer>();
        }
    }
}