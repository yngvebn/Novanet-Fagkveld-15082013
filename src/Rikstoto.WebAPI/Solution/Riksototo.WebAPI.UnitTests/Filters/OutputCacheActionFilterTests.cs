using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Caching;
using NUnit.Framework;
using Rikstoto.WebAPI.Filters;

namespace Rikstoto.WebAPI.UnitTests.Filters
{
    [TestFixture]
    public class OutputCacheActionFilterTests : FilterTestBase
    {
        [SetUp]
        public void SetUp()
        {
            ClearCache();
            Filter = new OutputCacheActionFilter(60, 60);
            Filter.OnActionExecuted(executedContext);
        }
        
        private void ClearCache()
        {
            foreach (var element in MemoryCache.Default)
            {
                var removed = MemoryCache.Default.Remove(element.Key);
            }      

            Assert.That( !MemoryCache.Default.Any());
        }

        [Test]
        public void OutputCacheShouldBePopulated()
        {
            Filter.OnActionExecuting(actionContext);

            Assert.IsNotNull(actionContext.Response);
        }

        [Test]
        public void OutputCacheShouldNotVaryByToken()
        {
            
            Filter.OnActionExecuting(actionContext);
            Assert.IsNotNull(actionContext.Response);
            
            //Call with different authorization
            var reExecutingContext = GetActionContext(HttpMethod.Get, DefaultUri);
            reExecutingContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "1234123412341234");
            Filter.OnActionExecuting(reExecutingContext);
            
            Assert.IsNotNull( reExecutingContext.Response.Headers);
            
        }

        [Test]
        public void ShouldVaryByAcceptHeader()
        {
            Filter.OnActionExecuting(actionContext);
            Assert.IsNotNull(actionContext.Response);

            //Call with different accept header
            var reExecutingContext = GetActionContext(HttpMethod.Get, DefaultUri);
            reExecutingContext.Request.Headers.Clear();
            reExecutingContext.Request.Headers.Accept.Add( new MediaTypeWithQualityHeaderValue("application/json"));
            Filter.OnActionExecuting(reExecutingContext);

            Assert.IsNull(reExecutingContext.Response);
        }
        
        [Test]
        public void ShouldHandleNullResponse()
        {
            ClearCache();
            
            var context = GetNullActionExecutedContext();
            Filter.OnActionExecuted( context );

            Assert.IsNull( context.Response );
            
        }

        [Test]
        public void ShouldGiveCorrectContentType()
        {
            Filter = new OutputCacheActionFilter(60,60);
            ClearCache();
            

            Filter.OnActionExecuted(executedContext);
            Filter.OnActionExecuting(actionContext);
            
            var contentType = actionContext.Response.Content.Headers.ContentType;
            Assert.IsNotNull(actionContext.Response, "Response is null");
            Assert.IsNotNull(contentType, "Content-type is null");

            var reExecutingContext = GetActionContext(HttpMethod.Get, DefaultUri);
            Filter.OnActionExecuting(reExecutingContext);

            Assert.IsNotNull(reExecutingContext.Response, "Re-executing context response is null");

            var cachedContentType = reExecutingContext.Response.Content.Headers.ContentType;
            Assert.IsNotNull(cachedContentType, "Cached content-type is null");

            Assert.AreEqual(contentType, cachedContentType);
            
        }

        [Test]
        public void OutputCacheShouldExpireAfterExpiryTime()
        {
            Filter = new OutputCacheActionFilter(1, 1);
            ClearCache();
            
            Filter.OnActionExecuting(actionContext);
            Assert.IsNotNull(actionContext.Response, "Should be cached");

            //now sleep to see cache expire
            System.Threading.Thread.Sleep(1005);
            var reExecutingContext = GetActionContext(HttpMethod.Get, DefaultUri);
            Filter.OnActionExecuting(reExecutingContext);
            Assert.IsNull(reExecutingContext.Response,"Should have expired");
            
        }

        [Test]
        public void OutputCacheShouldVaryByQueryString()
        {
            Filter = new OutputCacheActionFilter(1, 1);
            ClearCache();

            var context = GetActionExecutedContext("http://anyurl/?yayayaya&nononno&paela=yes");
            
            Filter.OnActionExecuted(context);
            Assert.IsNotNull(actionContext.Response, "Should be cached");
            
            var reExecutingContext = GetActionContext(HttpMethod.Get, "http://anyurl/?yayayaya&nononno&paela=no");

            Filter.OnActionExecuting(reExecutingContext);
            Assert.IsNull(reExecutingContext.Response, "Should not be cached");

            ClearCache();
        }

        [Test]
        public void OutputCacheShouldBePublicCacheControl()
        {
            Filter.OnActionExecuting(actionContext);
            Assert.IsNotNull(actionContext.Response); //It was cached!
            Assert.IsTrue(actionContext.Response.Headers.CacheControl.Public);
        }
    }
}
