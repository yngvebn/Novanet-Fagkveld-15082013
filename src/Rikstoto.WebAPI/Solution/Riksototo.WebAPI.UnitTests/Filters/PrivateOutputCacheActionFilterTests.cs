using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Caching;
using System.Text;
using NUnit.Framework;
using Rikstoto.WebAPI.Filters;

namespace Rikstoto.WebAPI.UnitTests.Filters
{
    class PrivateOutputCacheActionFilterTests : FilterTestBase
    {
        [SetUp]
        public void SetUp()
        {
            ClearCache();
            Filter = new PrivateOutputCacheActionFilter();
            executedContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJleHAiOjEzNDk2MDc4NzYsIm5iZiI6MTM0NzAxMjI3NiwiaWF0IjoxMzQ3MDE1ODc2LCJpc3MiOiJ1cm46cmlrc3RvdG86YXBpIiwiYXVkIjoiaHR0cHM6Ly9hcGkucmlrc3RvdG8ubm8vIiwiY3VzTmFtZSI6IkFybmViamFybmUiLCJjdXNJZCI6IjEyMyIsImFnZW50IjoiMDA4MDEiLCJjbGllbnQiOiJNb2JpbGVUZWNoIiwicHVyZXhwIjoiMTM0NzAyNjY3NiIsImF1dGh0eXBlIjoiVXNlcm5hbWVQYXNzd29yZCIsImNpZCI6IiIsInNlcSI6IiIsIm93bnRyIjoiIn0.VONGZGKi1_IlB9qR0KdUOmSVxK5CamXimmPDGHkf298");
            Filter.OnActionExecuted(executedContext); //populates cache
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
        public void PrivateOutputCacheShouldVaryByFilter()
        {
            Filter.OnActionExecuting(actionContext);
            Assert.IsNotNull(actionContext.Response); //It was cached!

            //Call with different authorization
            var reExecutingContext = GetActionContext(HttpMethod.Get, "http://anyurl/");
            reExecutingContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJleHAiOjEzNDk2MDc4ODIsIm5iZiI6MTM0NzAxMjI4MiwiaWF0IjoxMzQ3MDE1ODgyLCJpc3MiOiJ1cm46cmlrc3RvdG86YXBpIiwiYXVkIjoiaHR0cHM6Ly9hcGkucmlrc3RvdG8ubm8vIiwiY3VzTmFtZSI6IkFybmViamFybmUiLCJjdXNJZCI6IjEyMyIsImFnZW50IjoiMDA4MDEiLCJjbGllbnQiOiJNb2JpbGVUZWNoIiwicHVyZXhwIjoiMTM0NzAyNjY4MiIsImF1dGh0eXBlIjoiVXNlcm5hbWVQYXNzd29yZCIsImNpZCI6IiIsInNlcSI6IiIsIm93bnRyIjoiIn0.BJd2ClLH30wTCy8wYpVlc72dGGeYzUBph7S040jJ0Ic");
            Filter.OnActionExecuting(reExecutingContext);

            Filter.OnActionExecuting(actionContext);
            Assert.IsNotNull(actionContext.Response); //It was cached!

            Assert.IsNull(reExecutingContext.Response);
        }

        [Test]
        public  void PrivateOutputCacheShouldNotBePublicCacheControl()
        {
            Filter.OnActionExecuting(actionContext);
            Assert.IsNotNull(actionContext.Response); //It was cached!
            Assert.IsFalse(actionContext.Response.Headers.CacheControl.Public );
        }

    }
}
