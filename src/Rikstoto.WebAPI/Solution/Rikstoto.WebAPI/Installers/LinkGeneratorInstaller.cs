using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Rikstoto.WebAPI.Linking;

namespace Rikstoto.WebAPI.Installers
{
    public class LinkGeneratorInstaller: IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<ILinkPopulator>().ImplementedBy<LinkPopulator>());
        }
    }
}