using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Rikstoto.WebAPI.Extentions;
using Rikstoto.WebAPI.Logging;
using log4net;

namespace Rikstoto.WebAPI.MessageHandlers
{
    // Borrowed from 
    // http://www.forkcan.com/viewcode/1210/ASPNET-MVC-40-Web-API-Message-logger-for-Net-40
    // and 
    // http://www.forkcan.com/viewcode/1209/ASPNET-MVC-40-Web-API-Message-logger-for-Net-45
    public class LogMessageHandler : DelegatingHandler
    {
        private readonly LogLevel _logLevel;
        private static readonly ILog Logger = LogManager.GetLogger(typeof(LogMessageHandler));

        public LogMessageHandler(LogLevel logLevel) 
        {
            _logLevel = logLevel;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if(_logLevel == LogLevel.None)
                return base.SendAsync(request, cancellationToken);
            
            var readRequestBodyAsync = request.Content.ReadAsByteArrayAsync();
            var sendAsync = base.SendAsync(request, cancellationToken);
            var ct = cancellationToken;
            return readRequestBodyAsync
                .ContinueWith(t => sendAsync
                                       .ContinueWith(t2 =>
                                           {
                                               var requestBody = t.Result;
                                               var response = t2.Result;
                                               var requestHeaders = request.GetAllHeadersFormatted();
                                               var responseHeaders = response.GetAllHeadersFormatted();
                                               if (response.Content == null)
                                                   LogMessageAsync(new HttpRequestResponsePair(request, requestHeaders,
                                                                                               requestBody, response,
                                                                                               responseHeaders,
                                                                                               new byte[0]));
                                               else
                                                   response.Content.ReadAsByteArrayAsync()
                                                           .ContinueWith(
                                                               t3 =>
                                                               LogMessageAsync(new HttpRequestResponsePair(request,
                                                                                                           requestHeaders,
                                                                                                           requestBody,
                                                                                                           response,
                                                                                                           responseHeaders,
                                                                                                           t3.Result)),
                                                               ct);
                                               return t2.Result;
                                           }, ct),
                              ct)
                .Unwrap();
        }
        
        private Task LogMessageAsync(HttpRequestResponsePair requestResponse)
        {   
            bool shouldLog = (_logLevel == LogLevel.FailureOnly && requestResponse.Result.IsSuccessStatusCode == false) ||
                             _logLevel == LogLevel.All;

            if(shouldLog)
            {
                var logEntry = RequestResultLogEntryFactory.CreateLogEntry(requestResponse);
                Logger.Info(logEntry);
            }

            var tcs = new TaskCompletionSource<object>();
            tcs.SetResult(null);
            return tcs.Task;
        }
    }
}