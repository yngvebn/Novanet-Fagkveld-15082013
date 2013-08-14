using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Rikstoto.WebAPI.Extentions;
using Rikstoto.WebAPI.Helpers;

namespace Rikstoto.WebAPI.MessageHandlers
{
    public class NotAcceptableMessageHandler : DelegatingHandler
    {
        private const string allMediaTypesRange = "*/*";
        
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var acceptHeader = request.Headers.Accept;

            if (!IsRequestedMediaTypeAccepted(acceptHeader))
                return Task<HttpResponseMessage>.Factory.StartNew(() => request.CreateNotAcceptableResponseMessage("The requested content type specified in the Accept header is not valid.", ErrorTagGenerator.NewErrorTag(), ""));

            return base.SendAsync(request, cancellationToken);
        }

        private static bool IsMediaTypeAccepted(HttpRequestMessage request)
        {
            var contentNegotiator = new DefaultContentNegotiator();
            var formatter = contentNegotiator.Negotiate(typeof(JsonMediaTypeFormatter),request, GlobalConfiguration.Configuration.Formatters);
            return formatter != null;
        }

        private static bool IsRequestedMediaTypeAccepted(HttpHeaderValueCollection<MediaTypeWithQualityHeaderValue> acceptHeader)
        {
            return Enumerable.Any<MediaTypeFormatter>(GlobalConfiguration
                                             .Configuration
                                             .Formatters, formatter => acceptHeader.Any(mediaType => FormatterSuportsMediaType(mediaType, formatter)));
            
        }

        private static bool FormatterSuportsMediaType(MediaTypeWithQualityHeaderValue mediaType, MediaTypeFormatter formatter)
        {
            var supportsMediaType = formatter.SupportedMediaTypes.Contains(mediaType);
            var supportsTypeGroup = formatter.SupportedMediaTypes.Any(mt =>
            {
                var splitMediaType = mt.MediaType.Split('/');
                var type = splitMediaType.First();
                return mediaType.MediaType.StartsWith(type);
            });

            var isTypeGroup = Enumerable.Last<string>(mediaType.MediaType.Split('/')) == "*";
            var isAllMediaType = mediaType.MediaType == allMediaTypesRange;


            return isAllMediaType || supportsMediaType || (isTypeGroup && supportsTypeGroup);
        }
    }
}
