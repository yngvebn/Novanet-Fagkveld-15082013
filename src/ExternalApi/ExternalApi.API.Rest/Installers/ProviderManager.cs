using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using ExternalApi.API.Rest.Infrastructure;

namespace ExternalApi.API.Rest.Installers
{
    public class ProviderManager : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(AllTypes.FromThisAssembly().BasedOn(typeof (IManageDataProvidersFor<>)).WithServiceAllInterfaces());
            container.Register(AllTypes.FromThisAssembly().BasedOn(typeof(IManageDataProviders)).WithServiceAllInterfaces());
        }
    }
}