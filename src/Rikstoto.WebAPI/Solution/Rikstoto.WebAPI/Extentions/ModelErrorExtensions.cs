using System.Web.Http.ModelBinding;

namespace Rikstoto.WebAPI.Extentions
{
    public static class ModelErrorExtensions
    {
        public static string GetErrorMessage(this ModelError input)
        {
            if(input == null)
                return string.Empty;

            if(!string.IsNullOrWhiteSpace(input.ErrorMessage))
                return input.ErrorMessage;

            if(input.Exception != null)
                return input.Exception.Message;

            return string.Empty;
        }
    }
}