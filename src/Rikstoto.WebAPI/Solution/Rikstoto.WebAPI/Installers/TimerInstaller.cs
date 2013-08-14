using Castle.DynamicProxy;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Rikstoto.WebAPI.Interceptors;

namespace Rikstoto.WebAPI.Installers
{
    public class TimerInstaller: IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(AllTypes.FromAssemblyContaining<TimerInterceptor>().BasedOn<IInterceptor>().WithService.Self());
        }
    }
}