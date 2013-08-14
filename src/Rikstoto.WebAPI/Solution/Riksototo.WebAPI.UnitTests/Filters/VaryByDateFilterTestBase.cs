using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dependencies;
using System.Web.Http.Filters;
using System.Web.Http.Routing;
using Moq;
using Rikstoto.WebAPI.Settings;

namespace Rikstoto.WebAPI.UnitTests.Filters
{
    public class VaryByDateFilterTestBase : FilterTestBase
    {
        protected new readonly string DefaultUri = "http://anyurl/test/2012-12-12";

        protected override HttpActionContext GetActionContext(HttpMethod httpMethod, string uri)
        {

            var request = new HttpRequestMessage(httpMethod, uri);

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

            var controllerContext = new HttpControllerContext(new HttpConfiguration(),
                                                              new HttpRouteData(new HttpRoute()),
                                                              request);



            return new HttpActionContext(controllerContext, new ReflectedHttpActionDescriptor());
        }

        protected override HttpActionExecutedContext GetActionExecutedContext(string uri)
        {
            string date = string.Empty;
            if( uri.Length > 10)
                date = uri.Substring(uri.Length - 10, 10);


            var routes = new HttpRouteCollection("/");
            var route = routes.MapHttpRoute("DefaultApi", "{controller}/{date}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary(new { date = date}));

            var executingContext = GetActionContext(HttpMethod.Get, "http://any.where/test/2025-05-25", routeData);
            var response = GetResponseMessage("#just some content stuff", executingContext);


            return GetActionExecutedContext(response, executingContext);
        }

        protected HttpActionExecutedContext GetActionExecutedContext(DateTime date)
        {
            var routes = new HttpRouteCollection("/");
            var route = routes.MapHttpRoute("DefaultApi", "{controller}/{date}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary(new { date = date.ToString("yyyy-MM-dd") }));

            var executingContext = GetActionContext(HttpMethod.Get, string.Format("http://any.where/test/{0}", date.ToString("yyyy-MM-dd")), routeData);
            var response = GetResponseMessage("#just some content stuff", executingContext);
            
            return GetActionExecutedContext(response, executingContext);
        }

        protected HttpActionExecutedContext GetActionExcecutedContextWithMockWebApiSettings( DateTime date)
        {
            var routes = new HttpRouteCollection("/");
            var route = routes.MapHttpRoute("DefaultApi", "{controller}/{date}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary(new { date = date.ToString("yyyy-MM-dd") }));

            var executingContext = GetActionContext(HttpMethod.Get, string.Format("http://any.where/test/{0}", date.ToString("yyyy-MM-dd")), routeData);
          

            var mock = new Mock<IDependencyResolver>();
            mock.Setup(m => m.GetService(typeof(WebAPISettings))).Returns(new WebAPISettings
            {
                CacheDurationFuture = new TimeSpan(5, 0, 0),
                CacheDurationToday = new TimeSpan(0, 0, 30),
                CacheDurationPast = new TimeSpan(42, 0, 0, 0)
            });
            executingContext.ControllerContext.Configuration.DependencyResolver = mock.Object;

            var response = GetResponseMessage("#just some content stuff", executingContext);

            return GetActionExecutedContext(response, executingContext);
        }


    }
}
