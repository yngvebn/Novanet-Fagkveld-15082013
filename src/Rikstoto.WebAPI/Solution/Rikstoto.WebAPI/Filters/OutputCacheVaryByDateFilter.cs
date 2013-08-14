using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using Rikstoto.WebAPI.Settings;

namespace Rikstoto.WebAPI.Filters
{
    public class OutputCacheVaryByDateFilter : OutputCacheActionFilter
    {
        private static WebAPISettings _apiSettings;
        private static TimeSpan _cacheDurationFuture;
        private static TimeSpan _cacheDurationPast;
        private static TimeSpan _cacheDurationToday;

        static OutputCacheVaryByDateFilter()
        {
            SetCacheDurationToDefaultValues();
        }

        /// <summary>
        /// This functionality is kept in a separate method for test purposes
        /// </summary>
        public static void SetCacheDurationToDefaultValues()
        {
            _cacheDurationFuture = new TimeSpan(1, 0, 0);
            _cacheDurationPast = new TimeSpan(1, 0, 0, 0);
            _cacheDurationToday = new TimeSpan(0, 0, 5);
            _apiSettings = null;
        }

        public override void OnActionExecuted(System.Web.Http.Filters.HttpActionExecutedContext actionExecutedContext)
        {
            bool isCacheable = IsCacheable(actionExecutedContext.ActionContext);
            
            SetCaching(actionExecutedContext, isCacheable);
        }

        protected override DateTime GetExpiration(HttpControllerContext ctx)
        {
            return DateTime.Now.Add(GetTimespan(ctx));
        }

        protected override bool IsCacheable(HttpActionContext ac)
        {
            if(!ac.ControllerContext.RouteData.Values.ContainsKey("date"))
                throw new InvalidOperationException("No {date} parameter in url");    

            return ac.Request.Method == HttpMethod.Get;
        }
        
        protected override TimeSpan GetTimespan(HttpControllerContext ctx)
        {
            SetUpCacheDurationFromConfigIfNotSet(ctx);

            var date = (string)ctx.RouteData.Values["date"];
            if (!string.IsNullOrEmpty(date))
            {
                DateTime parsedDate;
                if (DateTime.TryParse(date, out parsedDate))
                {
                    var compare = DateTime.Compare(DateTime.Now.Date, parsedDate.Date);
                    if (compare < 0)
                        return _cacheDurationFuture;

                    if (compare > 0)
                        return _cacheDurationPast;
                }
            }

            return _cacheDurationToday;
        }

        private static void SetUpCacheDurationFromConfigIfNotSet(HttpControllerContext ctx)
        {
            if (_apiSettings == null)
            {
                _apiSettings = (WebAPISettings)ctx.Configuration.DependencyResolver.GetService(typeof(WebAPISettings));
                if (_apiSettings != null)
                {
                    _cacheDurationFuture = _apiSettings.CacheDurationFuture;
                    _cacheDurationPast = _apiSettings.CacheDurationPast;
                    _cacheDurationToday = _apiSettings.CacheDurationToday;
                }
            }
        }
    }
}