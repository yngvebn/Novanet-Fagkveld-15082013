namespace ExternalApi.API.Rest.Infrastructure
{
    public interface IManageDataProvidersFor<T>
    {
        void FillModelFromProviders(T model);
    }
}