using System.Collections.Generic;
using AutoMapper;
using Castle.Core.Internal;

namespace ExternalApi.API.Rest.Infrastructure.Mapping
{
    public class OurMapper : IMapper
    {
        private readonly IList<IMappingConfiguration> _configurations;

        public OurMapper(IList<IMappingConfiguration> configurations)
        {
            _configurations = configurations;

            _configurations.ForEach(configuration => configuration.Configure());
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return Mapper.Map<TSource, TDestination>(source);
        }

        public void Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            Mapper.Map(source, destination);
        }

        public TDestination Map<TDestination>(object source)
        {
            return Mapper.Map<TDestination>(source);
        }
    }
}