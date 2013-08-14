using System.Net.Http;

namespace Rikstoto.WebAPI.Logging
{
    public class HttpRequestResponsePair
    {
        public HttpRequestResponsePair(HttpRequestMessage request, string requestHeaders, byte[] requestBody, HttpResponseMessage response, string responseHeaders, byte[] responseBody)
        {
            Request = request;
            RequestBody = requestBody;
            RequestHeaders = requestHeaders;

            Result = response;
            ResultBody = responseBody; 
            ResponseHeaders = responseHeaders;
        }

        public HttpRequestMessage Request { get; set; }
        public byte[] RequestBody { get; set; }
        public string RequestHeaders { get; set; }

        public HttpResponseMessage Result { get; set; }
        public byte[] ResultBody { get; set; }
        public string ResponseHeaders { get; set; }
    }
}