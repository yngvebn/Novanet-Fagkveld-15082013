using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Rikstoto.WebAPI.Installers;

namespace ExternalApi.API.Rest
{
    public static class CastleBootstrapperForRestApi
    {
        private static void InitializeExternalApiImplementations(IWindsorContainer container)
        {
            var loadedAssemblies = new List<Assembly>();

            string implPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin\\impl");

            if (!Directory.Exists(implPath))
                return;

            foreach (var file in Directory.GetFiles(implPath))
            {
                if (file.Contains("API.External") && file.EndsWith(".dll"))
                {
                    loadedAssemblies.Add(Assembly.LoadFrom(file));
                }
            }

            foreach (var loadedAssembly in loadedAssemblies)
            {
                container.Install(FromAssembly.Instance(loadedAssembly));
            }
        }


        public static void Setup(IWindsorContainer container)
        {
            container.Install(FromAssembly.Containing<TimerInstaller>());
            container.Install(FromAssembly.This());
            InitializeExternalApiImplementations(container);

        }
    }
}