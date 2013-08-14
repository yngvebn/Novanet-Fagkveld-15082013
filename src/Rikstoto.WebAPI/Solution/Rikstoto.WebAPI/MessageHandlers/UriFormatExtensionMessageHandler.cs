using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace Rikstoto.WebAPI.MessageHandlers
{
    public class UriFormatExtensionMessageHandler : DelegatingHandler
    {
        private bool _shouldForceJson;
        private bool _shouldForceXml;
        public void CheckFormats(HttpRequestMessage request)
        {
            string query = request.RequestUri.Query;
            string mediaType = HttpUtility.ParseQueryString(query).Get("mediaType");
            if (mediaType != null)
            {
                switch (mediaType.ToLower())
                {
                    case "json":
                        _shouldForceJson = true;
                        break;
                    case "xml":
                        _shouldForceXml = true;
                        break;
                }
            }
        }

        protected override System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        System.Threading.CancellationToken cancellationToken)
        {
            CheckFormats(request);
            if (_shouldForceJson)
            {
                request.Headers.Accept.Clear();
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                _shouldForceJson = false;
            }

            if (_shouldForceXml)
            {
                request.Headers.Accept.Clear();
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                _shouldForceXml = false;
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}
