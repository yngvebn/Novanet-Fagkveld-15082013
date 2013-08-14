using System.Web.Http;
using System.Web.Mvc;
using ExternalApi.API.Rest.App_Start;

namespace ExternalApi.API.Rest
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            AttributeRoutingHttpConfig.Start();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            MappingConfig.Configure();
        }
    }
}