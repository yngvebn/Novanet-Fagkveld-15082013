using System.Web.Http;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Rikstoto.WebAPI.Container;

namespace ProductService.API.Rest.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            IWindsorContainer container = new WindsorContainer();
            config.DependencyResolver = new WindsorDependencyResolver(container);
            container.Install(FromAssembly.This());
        }
    }
}
