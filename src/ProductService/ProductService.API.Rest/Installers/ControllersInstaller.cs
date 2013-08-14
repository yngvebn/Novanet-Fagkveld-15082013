using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using ProductService.API.Rest.Controllers;

namespace ProductService.API.Rest.Installers
{
    public class ControllersInstaller: IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(AllTypes.FromThisAssembly().InSameNamespaceAs<ProductsController>()
                                       .LifestyleTransient());
        }
    }
}