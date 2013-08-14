using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using CustomerService.API.Rest.Controllers;

namespace CustomerService.API.Rest.Installers
{
    public class ControllersInstaller: IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(AllTypes.FromThisAssembly().InSameNamespaceAs<CustomersController>()
                                       .LifestyleTransient());
        }
    }
}