using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Rikstoto.ExternalApi.Contracts;

namespace OrderService.API.Rest.Installers
{
    public class Providers : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(AllTypes.FromThisAssembly().BasedOn<IProvideDataFor>().WithServiceBase());
        }
    }
}