using Castle.Windsor;
using Castle.Windsor.Installer;
using Rikstoto.WebAPI.Installers;

namespace ProductService.API.Rest
{
    public static class CastleBootstrapperForRestApi
    {
        public static void Setup(IWindsorContainer container)
        {
            container.Install(FromAssembly.Containing<TimerInstaller>());
            container.Install(FromAssembly.This());
        }
    }
}