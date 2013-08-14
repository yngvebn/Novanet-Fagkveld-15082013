using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using ExternalApi.API.Rest.Controllers;

namespace ExternalApi.API.Rest.Installers
{
    public class ControllersInstaller: IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(AllTypes.FromThisAssembly().InSameNamespaceAs<CustomerOrdersController>()
                                       .LifestyleTransient());
        }
    }
}