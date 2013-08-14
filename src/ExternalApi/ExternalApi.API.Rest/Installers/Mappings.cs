using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using ExternalApi.API.Rest.Infrastructure.Mapping;

namespace ExternalApi.API.Rest.Installers
{
    public class Mappings : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(AllTypes.FromThisAssembly().BasedOn<IMapper>().WithService.AllInterfaces());
            container.Register(AllTypes.FromThisAssembly().BasedOn<IMappingConfiguration>().WithService.AllInterfaces());
        }
    }
}