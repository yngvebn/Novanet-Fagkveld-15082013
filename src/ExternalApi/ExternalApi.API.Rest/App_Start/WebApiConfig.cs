using System.Web.Http;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Rikstoto.WebAPI.Container;

namespace ExternalApi.API.Rest.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            IWindsorContainer container = new WindsorContainer();
            container.Kernel.Resolver.AddSubResolver(new ListResolver(container.Kernel));
            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));

            config.DependencyResolver = new WindsorDependencyResolver(container);
            container.Install(FromAssembly.This());
        }
    }
}
