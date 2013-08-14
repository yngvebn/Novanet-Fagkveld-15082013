using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Caching;
using System.Text;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;
using Castle.DynamicProxy.Contributors;
using NUnit.Framework;
using Rikstoto.WebAPI.Filters;

namespace Rikstoto.WebAPI.UnitTests.Filters
{
    public class OutputCacheVaryByDateFilterTests : VaryByDateFilterTestBase
    {
        [SetUp]
        public void SetUp()
        {
            ClearCache();
            Filter = new OutputCacheVaryByDateFilter();
            Filter.OnActionExecuted(executedContext);
        }

        private void ClearCache()
        {
            List<string> cacheKeys = MemoryCache.Default.Select(kvp => kvp.Key).ToList();
            foreach (string cacheKey in cacheKeys)
            {
                MemoryCache.Default.Remove(cacheKey);
            }
        }

        [Test]
        public void OutputCacheShouldBePopulated()
        {
            Filter.OnActionExecuting(actionContext);
            Assert.IsNotNull(actionContext.Response);
        }

        [Test]
        public void ShouldBePublicCacheControl()
        {
            Filter.OnActionExecuting(actionContext);
            Assert.IsNotNull(actionContext.Response); //It was cached!
            Assert.IsTrue(actionContext.Response.Headers.CacheControl.Public);
        }

        [Test]
        public void ShouldHavePastExpireTime()
        {
            var expected = new TimeSpan(42, 0, 0, 0); //from mocked WebApiSettings object
            var context = GetActionContext(DateTime.Now.AddYears(-1));
            Filter.OnActionExecuting(context);
            Assert.IsNull(context.Response);

            var exContext = GetActionExcecutedContextWithMockWebApiSettings(DateTime.Now.AddYears(-1));
            Filter.OnActionExecuted(exContext);
            
            Assert.AreEqual(expected, exContext.Response.Headers.CacheControl.MaxAge);
        }

        [Test]
        public void ShouldHaveFutureExpireTime()
        {
            var expected = new TimeSpan(1, 0, 0);
            var context = GetActionContext(new DateTime(2025,5,25));
            Filter.OnActionExecuting(context);
            Assert.IsNotNull(context.Response, "Response should be cached."); //was cached, because it's the default url

            var reExecuted = GetActionExecutedContext(new DateTime(2025, 5, 25));
            Filter.OnActionExecuted(reExecuted);
            Assert.AreEqual(expected, reExecuted.Response.Headers.CacheControl.MaxAge);
            
        }

        [Test]
        public void ShouldFailOnNoDateInRoute()
        {
            Assert.Throws<InvalidOperationException>(
                    () => Filter.OnActionExecuting(GetActionContext(HttpMethod.Get, "http://any.where/"))
                );
        }

        [Test]
        public void ShouldHaveTodayExpireTime()
        {
            // This test depends on a Filter with default cache duration data, but filter could contain mock value from earlier tests
            OutputCacheVaryByDateFilter.SetCacheDurationToDefaultValues();

            var expected = new TimeSpan(0, 0, 5);
            var uri = string.Format("http://any.where/test/{0}", DateTime.Now.ToString("yyyy-MM-dd"));
            var routes = new HttpRouteCollection("/");
            var route = routes.MapHttpRoute("DefaultApi", "{controller}/{date}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary(new { date = DateTime.Now.ToString("yyyy-MM-dd") }));

            var newContext = GetActionContext(HttpMethod.Get, uri , routeData);

            Filter.OnActionExecuting(newContext);
            Assert.IsNull(newContext.Response); //not cached!

            var reExecuted = GetActionExecutedContext(uri);
            Filter.OnActionExecuted( reExecuted );
            Assert.AreEqual(expected, reExecuted.Response.Headers.CacheControl.MaxAge);
        }

        private HttpActionContext GetActionContext(DateTime date)
        {
            
            var routes = new HttpRouteCollection("/");
            var route = routes.MapHttpRoute("DefaultApi", "{controller}/{date}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary(new { date = date.ToString("yyyy-MM-dd") }));

            return GetActionContext(HttpMethod.Get, string.Format("http://any.where/test/{0}", date.ToString("yyyy-MM-dd") ), routeData);

        }
    }
}
