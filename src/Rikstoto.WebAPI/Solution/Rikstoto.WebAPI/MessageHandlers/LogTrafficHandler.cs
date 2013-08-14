using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.SelfHost;
using Rikstoto.WebAPI.Extentions;
using log4net;

namespace Rikstoto.WebAPI.MessageHandlers
{
    public class LogTrafficHandler : DelegatingHandler
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(LogTrafficHandler));
        private readonly bool _log;
        private readonly string _serverName;

        public LogTrafficHandler(bool log, HttpConfiguration configuration)
        {
            _log = log;

            if (configuration as HttpSelfHostConfiguration != null)
                _serverName = ((HttpSelfHostConfiguration)(configuration)).BaseAddress.Host;
            else if (HttpContext.Current != null)
                _serverName = HttpContext.Current.Server.MachineName;
            else
                _serverName = "unkown";
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!_log)
                return base.SendAsync(request, cancellationToken);


            var sw = new Stopwatch();
            sw.Start();

            var response = base.SendAsync(request, cancellationToken);

            response.ContinueWith(responseMessage =>
                {
                    sw.Stop();
                    WriteLog(request, responseMessage.Result, sw.ElapsedMilliseconds);
                });

            return response;
        }

        private void WriteLog(HttpRequestMessage request, HttpResponseMessage response, long elapsedMilliseconds)
        {
            Logger.InfoFormat(
                    "[Client IP: {0}] [Request method: {1}] [RequestUri: {2}] [Request Http Version: {3}] [Response Statuscode: {4}] [Response Reason Phrase: {5}] [Response Http Version: {6}] [Server: {7}] [Time taken: {8}]",
                    request.GetClientIpAddress(),
                    request.Method,
                    request.RequestUri,
                    request.Version,
                    response.StatusCode,
                    response.ReasonPhrase,
                    response.Version,
                    _serverName,
                    elapsedMilliseconds);
        }
    }
}