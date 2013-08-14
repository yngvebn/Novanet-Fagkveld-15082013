using System.Collections.Specialized;

namespace Rikstoto.WebAPI.Extentions
{
    public static class NameValueCollectionExtentions
    {
        public static bool HasKey(this NameValueCollection collection, string key)
        {
            return collection[key] != null;
        }
    }
}