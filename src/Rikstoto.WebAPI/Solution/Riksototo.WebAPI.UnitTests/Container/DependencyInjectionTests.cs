using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NUnit.Framework;
using Rikstoto.WebAPI.Container;

namespace Rikstoto.WebAPI.UnitTests.Container
{
    [TestFixture]
    public class DependencyInjectionTests
    {
        [Test]
        public void WindsorResolverShouldResolveRegisteredDummyRepositoryTest()
        {
            using (var container = new WindsorContainer())
            {
                container.Register(
                    Component.For<IDummyRepository>().Instance(new InMemoryDummyRepository()));

                var resolver = new WindsorDependencyResolver(container);
                var instance = resolver.GetService(typeof (IDummyRepository));

                Assert.IsNotNull(instance);
            }
        }

        [Test]
        public void WindsorResolverShouldNotResolveNonRegisteredDummyRepositoryTest()
        {
            using (var container = new WindsorContainer())
            {
                var resolver = new WindsorDependencyResolver(container);
                var instance = resolver.GetService(typeof (IDummyRepository));

                Assert.IsNull(instance);
            }
        }

        [Test]
        public void WindsorResolverShouldResolveRegisteredDummyRepositoryThroughControllerTest()
        {
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute("default",
                                       "api/{controller}/{id}", new {id = RouteParameter.Optional});

            using (var container = new WindsorContainer())
            {
                container.Register(
                    Component.For<IDummyRepository>().Instance(new InMemoryDummyRepository()));

                config.DependencyResolver = new WindsorDependencyResolver(container);

                var server = new HttpServer(config);
                var client = new HttpClient(server);

                client.GetAsync("http://anything/api/contacts").ContinueWith(task =>
                                                                                 {
                                                                                     var response = task.Result;
                                                                                     Assert.IsNotNull(response.Content);
                                                                                 });
            }
        }

        [Test]
        public void WindsorResolverInHttpConfigShouldNotResolvePipelineTypeButFallbackToDefaultResolverTest()
        {
            using (var container = new WindsorContainer())
            {

                var config = new HttpConfiguration();
                config.DependencyResolver = new WindsorDependencyResolver(container);
                var instance = config.Services.GetService(typeof (IHttpActionSelector));

                Assert.IsNotNull(instance);
            }
        }
    }


    public class InMemoryDummyRepository : IDummyRepository
    {
    }

    public interface IDummyRepository
    {
    }
}