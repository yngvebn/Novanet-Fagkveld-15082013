using System;
using System.Net;
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
    public class FilterTestBase
    {
        protected ActionFilterAttribute Filter;
        protected HttpActionExecutedContext executedContext;
        protected readonly string DefaultUri = "http://anyurl/";
        
        public FilterTestBase()
        {
            executedContext = GetActionExecutedContext();
        }

        public HttpActionContext actionContext { get { return executedContext.ActionContext; } }

        protected virtual HttpActionContext GetActionContext(HttpMethod httpMethod, string uri)
        {
            
            var request = new HttpRequestMessage(httpMethod, uri);

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            
            var controllerContext = new HttpControllerContext(new HttpConfiguration(),
                                                              new HttpRouteData(new HttpRoute()), 
                                                              request);

           

            return new HttpActionContext(controllerContext, new ReflectedHttpActionDescriptor());
        }

        protected HttpActionContext GetActionContext(HttpMethod httpMethod, string uri, HttpRouteData routeData)
        {
            var request = new HttpRequestMessage(httpMethod, uri);

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

            var controllerContext = new HttpControllerContext(new HttpConfiguration(),
                                                              new HttpRouteData(
                                                                new HttpRoute("{controller}/{date}",
                                                                    new HttpRouteValueDictionary(new { date = "2013-01-01" })))

                                                                    , request);

            controllerContext.RouteData = routeData;
           
            return new HttpActionContext(controllerContext, new ReflectedHttpActionDescriptor());
        }

        protected HttpActionExecutedContext GetActionExecutedContext()
        {
            return GetActionExecutedContext(DefaultUri);
        }

        protected virtual HttpActionExecutedContext GetActionExecutedContext(string uri )
        {
            var executingContext = GetActionContext(HttpMethod.Get, uri);
            var response = GetResponseMessage("#just some content stuff", executingContext);
            return GetActionExecutedContext(response, executingContext);
        }

        protected HttpActionExecutedContext GetNullActionExecutedContext()
        {
            var executingContext = GetActionContext(HttpMethod.Get, DefaultUri);
            var response = (HttpResponseMessage)null;
            return GetActionExecutedContext(response, executingContext);
        }

        protected HttpActionExecutedContext GetActionExecutedContext(HttpResponseMessage response, HttpActionContext context)
        {
            return new HttpActionExecutedContext
                {
                    ActionContext = context,
                    Response = response
                };
        }

        protected HttpResponseMessage GetResponseMessage(string content, HttpActionContext executingContext)
        {
             var x = new HttpResponseMessage
                {StatusCode = HttpStatusCode.OK, Content = new StringContent(content) };
            return x;
        }
    }
}
