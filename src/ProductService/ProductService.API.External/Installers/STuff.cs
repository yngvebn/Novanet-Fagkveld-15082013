using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Rikstoto.ExternalApi.Contracts;

namespace ProductService.API.External.Installers
{
    public class RepositoryInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(AllTypes.FromThisAssembly().BasedOn<IProvideDataFor>().WithServiceBase());

            container.Register(Component.For<IProductRepository>().ImplementedBy<ProductRepository>());
        }
    }
}
