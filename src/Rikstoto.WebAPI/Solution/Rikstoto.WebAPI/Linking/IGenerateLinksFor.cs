namespace Rikstoto.WebAPI.Linking
{
    public interface IGenerateLinksFor<in T> : IGenerateLinksFor
    {
        void Populate(T model);
    }

    public interface IGenerateLinksFor
    {
        
    }
}