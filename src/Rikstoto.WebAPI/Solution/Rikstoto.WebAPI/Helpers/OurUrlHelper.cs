using System;
using System.Configuration;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;

namespace Rikstoto.WebAPI.Helpers
{
    public static class OurUrlHelper
    {

        public static Uri Link(string routeName, object routeValues, HttpControllerContext context)
        {
            var defaultUri = new Uri(new UrlHelper(context.Request).Link(routeName, routeValues));
            var baseUrl = ConfigurationManager.AppSettings["WebAPI.LinkBaseUri"];

            if (string.IsNullOrWhiteSpace(baseUrl))
                return defaultUri;

            var relativePart = defaultUri.GetComponents(UriComponents.PathAndQuery, UriFormat.SafeUnescaped);
            return new Uri(new Uri(baseUrl), relativePart);
        }

        public static Uri DefaultLink<T>(object id, HttpControllerContext context) where T : ApiController
        {
            string controllerName = typeof(T).Name;

            int lastIndexOf = controllerName.LastIndexOf("Controller", StringComparison.Ordinal);

            if (lastIndexOf >= 0)
                controllerName = controllerName.Substring(0, lastIndexOf);

            return Link(Routes.RouteNamesDefaults.DefaultApi, new { controller = controllerName, id }, context);
        }
    }
}