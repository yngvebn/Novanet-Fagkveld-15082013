using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Caching;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Rikstoto.WebAPI.Filters
{
    //Based on https://github.com/filipw/aspnetwebapi-outputcache

    public class OutputCacheActionFilter : ActionFilterAttribute
    {
        // Use state here at your own risk. It seems the same instance can be reused amount different method calls.
        // Presumably the Web API is clever enough not to reuse the same instance set up with different ctor param values.
        // -Jørund

        // cache length in seconds
        private readonly int _timespan;
        // client cache length in seconds
        protected int _clientTimeSpan;

        // cache repository
        protected static readonly ObjectCache WebApiCache = MemoryCache.Default;

        //should proxies be allowed to cache this
        protected virtual bool UsePublicCache { get { return true; } }
        
        /// <summary>
        /// Caches  the output of the request
        /// </summary>
        /// <param name="timespan">Seconds to keep in cache</param>
        /// <param name="clientTimeSpan">Seconds for client to keep in cache</param>
        public OutputCacheActionFilter(int timespan = 10, int clientTimeSpan = 10)
        {
            _timespan = timespan;
            _clientTimeSpan = clientTimeSpan;
        }
        
        public override void OnActionExecuting(HttpActionContext ac)
        {
            if (ac == null)
            {
                throw new ArgumentNullException("ac");
            }
            
            if (IsCacheable(ac))
            {
                string cachekey = GetCacheKey(ac.Request);

                if (WebApiCache.Contains(cachekey))
                {
                    var val = (string) WebApiCache.Get(cachekey);
                    var contenttype = (MediaTypeHeaderValue)WebApiCache.Get(cachekey + ":response-ct") ??
                                      new MediaTypeHeaderValue(cachekey.Split(':')[1]);

                    if (val != null )
                    {
                        ac.Response = ac.Request.CreateResponse();
                        ac.Response.Content = new StringContent(val);
                        ac.Response.Content.Headers.ContentType = contenttype;

                        SetHeaders(ac.Response, GetTimespan(ac.ControllerContext));
                    }
                }
            }
        }
        
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            //Because of testing.
            bool cacheable = IsCacheable(actionExecutedContext.ActionContext);
            
            SetCaching(actionExecutedContext, cacheable);
        }
        
        protected virtual string GetCacheKey(HttpRequestMessage request)
        {
            return String.Join(":",
                               new []
                                   {
                                       request.RequestUri.PathAndQuery,
                                       request.Headers.Accept.FirstOrDefault().ToString()
                                   });
        }

        protected virtual bool IsCacheable(HttpActionContext ac)
        {
            if (_timespan > 0 && _clientTimeSpan > 0)
            {
                if (ac.Request.Method == HttpMethod.Get) return true;
            }
            else
            {
                throw new InvalidOperationException("Wrong Arguments");
            }

            return false;
        }

        protected virtual DateTime GetExpiration(HttpControllerContext ctx)
        {
            return DateTime.Now.AddSeconds(_timespan);
        }

        protected virtual TimeSpan GetTimespan(HttpControllerContext ctx)
        {
            return TimeSpan.FromSeconds(_clientTimeSpan);
        }
        
        protected void SetCaching(HttpActionExecutedContext actionExecutedContext, bool cacheable)
        {
            if(actionExecutedContext.Response == null)
                return;

            string cachekey = GetCacheKey(actionExecutedContext.Request);

            var expiration = GetExpiration(actionExecutedContext.ActionContext.ControllerContext);

            if ( !(WebApiCache.Contains(cachekey)) && cacheable)
            {
                var body = actionExecutedContext.Response.Content.ReadAsStringAsync().Result;
                var contentType = actionExecutedContext.Response.Content.Headers.ContentType;

                WebApiCache.Add(cachekey, body, expiration);
                WebApiCache.Add(cachekey + ":response-ct", contentType, expiration);
            }

            if (cacheable)
            {
                SetHeaders(actionExecutedContext.ActionContext.Response, GetTimespan(actionExecutedContext.ActionContext.ControllerContext));
            }
        }

        private void SetHeaders(HttpResponseMessage response, TimeSpan timeSpan)
        {
            response.Headers.Vary.Add("Accept");
            response.Headers.Vary.Add("Accept-Encoding");
            response.Headers.CacheControl = SetClientCache(timeSpan);
        }

        private CacheControlHeaderValue SetClientCache(TimeSpan timeSpan)
        {
            return new CacheControlHeaderValue
            {
                MaxAge = timeSpan,
                MustRevalidate = true,
                Public = UsePublicCache
            };
        }
    }
}