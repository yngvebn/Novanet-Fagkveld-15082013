using System.Net.Http;

namespace Rikstoto.WebAPI.Extentions
{
    public static class HttpResponseMessageExtensions
    {
        public static string GetAllHeadersFormatted(this HttpResponseMessage response)
        {
            var headers = response.Headers.ToString();

            if (response.Content != null)
                headers += response.Content.Headers.ToString();

            return headers;
        }
    }
}