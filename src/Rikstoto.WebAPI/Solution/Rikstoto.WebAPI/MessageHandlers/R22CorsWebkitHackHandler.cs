using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Rikstoto.WebAPI.MessageHandlers
{
    /// <summary>
    /// This is hack needed for older web kit browsers(Android and IOS 4) where the browser does not send origin-header as part of a CORS GET, but still needs to get allow-origin back to allow CORS. Can be removed when all users of the api use new and shiny browsers.. :) IOS4+ and Android 3+(??)
    /// See details: http://code.google.com/p/chromium/issues/detail?id=57836, http://trac.webkit.org/changeset/77680/
    /// </summary>
    public class R22CorsWebkitHackHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                               CancellationToken cancellationToken)
        {
            bool isAffectedByBug = request.Method == HttpMethod.Get;
            
            if(isAffectedByBug == false)
                return base.SendAsync(request, cancellationToken);
            
            return base.SendAsync(request, cancellationToken).ContinueWith(
                innerTask =>
                    {
                        var response = innerTask.Result;
                        SetOrUpdateHeader(response, "Access-Control-Allow-Origin", "*");
                        SetOrUpdateHeader(response, "Access-Control-Allow-Credentials", "true");
                        return response;
                    });
        }

        private void SetOrUpdateHeader(HttpResponseMessage response, string headerName, string headerValue)
        {
            if(response == null)
                return;

            if(response.Headers.Contains(headerName))
                response.Headers.Remove(headerName);

            response.Headers.Add(headerName, headerValue);
        }
    }
}
