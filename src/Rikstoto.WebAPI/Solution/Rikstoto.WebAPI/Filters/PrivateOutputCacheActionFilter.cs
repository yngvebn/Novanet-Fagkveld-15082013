using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Castle.Core.Internal;

namespace Rikstoto.WebAPI.Filters
{
    public class PrivateOutputCacheActionFilter : OutputCacheActionFilter
    {
        protected override bool UsePublicCache { get { return false; } }

        /// <summary>
        /// Caches  the output of the request
        /// </summary>
        /// <param name="timespan">Seconds to keep in cache</param>
        /// <param name="clientTimeSpan">Seconds for client to keep in cache</param>
        public PrivateOutputCacheActionFilter(int timespan = 30, int clientTimeSpan = 30) : base(timespan,clientTimeSpan)
        {
        }

        protected override string GetCacheKey(HttpRequestMessage request)
        {
            string authorization = string.Empty, accept = string.Empty, requestUri = string.Empty;
            var queryStringParameters = new StringBuilder();

            requestUri = request.RequestUri.AbsolutePath;

            var headers = request.Headers;

            if (headers.Authorization != null)
                authorization = headers.Authorization.Parameter;

            if (headers.Accept.Any())
                accept = headers.Accept.FirstOrDefault().ToString();

            var queryparams = request.GetQueryNameValuePairs();
            var keyValuePairs = queryparams as List<KeyValuePair<string, string>> ?? queryparams.ToList();
            if (keyValuePairs.Any())
            {
                keyValuePairs.OrderBy(c => c.Key).ForEach((c) => queryStringParameters.AppendFormat("{0}:{1}", c.Key, Convert.ToString(c.Value)));
            }

            return String.Join(":",
                               new []
                                   {
                                       requestUri, 
                                       authorization,
                                       accept,
                                       queryStringParameters.ToString()
                                   });
        }
        
    }
}