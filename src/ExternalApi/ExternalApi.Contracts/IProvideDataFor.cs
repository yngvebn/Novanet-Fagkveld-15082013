namespace Rikstoto.ExternalApi.Contracts
{
    public interface IProvideDataFor<T>: IProvideDataFor
    {
        void Fill(T model);
    }

    public interface IProvideDataFor
    {
        
    }
}