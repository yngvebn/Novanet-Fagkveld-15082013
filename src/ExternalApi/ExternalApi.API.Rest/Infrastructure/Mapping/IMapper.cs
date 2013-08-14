namespace ExternalApi.API.Rest.Infrastructure.Mapping
{
    public interface IMapper
    {
        TDestination Map<TSource, TDestination>(TSource source);
        TDestination Map<TDestination>(object source);
        void Map<TSource, TDestination>(TSource source, TDestination destination);
    }


}