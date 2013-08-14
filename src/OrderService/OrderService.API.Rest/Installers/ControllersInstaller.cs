using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using OrderService.API.Rest.Controllers;

namespace OrderService.API.Rest.Installers
{
    public class ControllersInstaller: IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(AllTypes.FromThisAssembly().InSameNamespaceAs<OrdersController>()
                                       .LifestyleTransient());
        }
    }
}