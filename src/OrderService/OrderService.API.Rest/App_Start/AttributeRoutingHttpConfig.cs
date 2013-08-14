using System.Web.Http;
using AttributeRouting.Web.Http.WebHost;
using OrderService.API.Rest.App_Start;

[assembly: WebActivator.PreApplicationStartMethod(typeof(AttributeRoutingHttpConfig), "Start")]

namespace OrderService.API.Rest.App_Start 
{
    public static class AttributeRoutingHttpConfig
	{
		public static void RegisterRoutes(HttpRouteCollection routes) 
		{    
			// See http://github.com/mccalltd/AttributeRouting/wiki for more options.
			// To debug routes locally using the built in ASP.NET development server, go to /routes.axd

            routes.MapHttpAttributeRoutes();
		}

        public static void Start() 
		{
            RegisterRoutes(GlobalConfiguration.Configuration.Routes);
        }
    }
}