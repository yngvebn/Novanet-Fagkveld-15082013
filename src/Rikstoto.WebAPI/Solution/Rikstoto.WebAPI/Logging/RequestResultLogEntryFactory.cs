using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Rikstoto.WebAPI.Extentions;

namespace Rikstoto.WebAPI.Logging
{
    public static class RequestResultLogEntryFactory
    {
        public static string CreateLogEntry(HttpRequestResponsePair pair)
        {
            var correlation = string.Format("WebAPI: {0}{1}", DateTime.Now.Ticks, Thread.CurrentThread.ManagedThreadId);
            var clientIP = string.Format("Client: {0}", pair.Request.GetClientIpAddress());

            var requestInfo = string.Format("REQUEST - {0} {1}", pair.Request.Method, pair.Request.RequestUri);
            var requestVersion = string.Format("HTTP Version: {0}", pair.Request.Version);
            
            var requestBodyAsString = Encoding.UTF8.GetString(pair.RequestBody);
            requestBodyAsString = Obscure(requestBodyAsString,
                                          new []
                                              {
                                                  "socialSecurityNumber", "accountNumber", "password", "securityAnswer",
                                                  "mobileNumber"
                                              });

            var resultInfo = string.Format("RESPONSE - {0} {1}", (int) pair.Result.StatusCode, pair.Result.StatusCode.ToString());

            var resultBodyAsString = Encoding.UTF8.GetString(pair.ResultBody);
            
            var builder = new StringBuilder();
            builder.AppendLine(correlation);
            builder.AppendLine(clientIP);
            builder.AppendLine(requestVersion);

            builder.AppendLine(requestInfo);
            if (string.IsNullOrWhiteSpace(pair.RequestHeaders) == false)
                builder.AppendLine(pair.RequestHeaders);
            if(string.IsNullOrEmpty(requestBodyAsString) == false)
            {
                builder.AppendLine();
                builder.AppendLine(requestBodyAsString);
                builder.AppendLine();
            }
            else
                builder.AppendLine();

            builder.AppendLine(resultInfo);
            if(string.IsNullOrWhiteSpace(pair.ResponseHeaders) == false)
                builder.AppendLine(pair.ResponseHeaders);
            if(string.IsNullOrEmpty(resultBodyAsString) == false)
            {
                


                builder.AppendLine();
                builder.AppendLine(resultBodyAsString);
                builder.AppendLine();
            }
            builder.AppendLine("-----------------");
            return builder.ToString();
        }

        private static string Obscure(string source, IEnumerable<string> keys)
        {
            foreach (string key in keys)
            {
                if (source.Contains(key))
                {
                    var asList = source.Split(',').ToList();
                    
                    var val = asList.FirstOrDefault(s => s.Contains(key));
                    if (val != null)
                    {
                        val = val.Remove(val.IndexOf(":", StringComparison.Ordinal));
                        val = val + ":'<removed from log>',";
                    }

                    var sb = new StringBuilder();
                    foreach (string t in asList)
                    {
                        sb.Append(t.Contains(key) ? val : t.Trim() != string.Empty ? t + "," : t);
                    }


                    var s2 = sb.ToString();
                    source = s2.Remove(s2.LastIndexOf(','), 1) + "}";
                }
            }

            return source;
        }
    }
}