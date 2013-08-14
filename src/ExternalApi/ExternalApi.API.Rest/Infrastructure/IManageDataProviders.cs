namespace ExternalApi.API.Rest.Infrastructure
{
    public interface IManageDataProviders
    {
        void FillModelFromProviders<T>(T model);
    }

    
}