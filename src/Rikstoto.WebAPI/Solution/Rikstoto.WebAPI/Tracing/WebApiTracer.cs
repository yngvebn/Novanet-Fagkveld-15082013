using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web.Http.Tracing;
using log4net;

namespace Rikstoto.WebAPI.Tracing
{
    public class WebApiTracer : ITraceWriter
    {

        private static Lazy<Dictionary<TraceLevel, Action<string>>> loggingMap = new Lazy<Dictionary<TraceLevel, Action<string>>>(() => new Dictionary<TraceLevel, Action<string>> { 
            {TraceLevel.Info, LogManager.GetLogger(typeof(WebApiTracer)).Info},
            {TraceLevel.Debug, LogManager.GetLogger(typeof(WebApiTracer)).Debug},
            {TraceLevel.Error, LogManager.GetLogger(typeof(WebApiTracer)).Error},
            {TraceLevel.Fatal, LogManager.GetLogger(typeof(WebApiTracer)).Fatal},
            {TraceLevel.Warn, LogManager.GetLogger(typeof(WebApiTracer)).Warn}
     });

        public void Trace(HttpRequestMessage request, string category, TraceLevel level, Action<TraceRecord> traceAction)
        {

            if (level != TraceLevel.Off)
            {
                var record = new TraceRecord(request, category, level);
                traceAction(record);
                Log(record);
            }
        }


        private void Log(TraceRecord record)
        {
            var message = new StringBuilder();

            if (record.Request != null)
            {
                if (record.Request.Method != null)
                    message.AppendFormat(" {0}", record.Request.Method);

                if (record.Request.RequestUri != null)
                    message.AppendFormat(" {0}", record.Request.RequestUri);
            }

            if (!string.IsNullOrWhiteSpace(record.Category))
                message.AppendFormat(" {0}", record.Category);

            if (!string.IsNullOrWhiteSpace(record.Operator))
                message.AppendFormat(" {0} {1}", record.Operator, record.Operation);

            if (!string.IsNullOrWhiteSpace(record.Message))
                message.AppendFormat(" {0}", record.Message);
            
            if (record.Exception != null)
            {
                if (record.Exception.GetBaseException().Message != null)
                    message.AppendFormat(" {0}", record.Exception.GetBaseException().Message);
            }

            _logger[record.Level](message.ToString());
        }


        private Dictionary<TraceLevel, Action<string>> _logger
        {
            get
            {
                return loggingMap.Value;
            }
        }
    }
}