using System.Globalization;
using System.Linq;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Filters;
using Castle.Windsor;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Rikstoto.WebAPI.Container;
using Rikstoto.WebAPI.Filters;
using Rikstoto.WebAPI.MessageHandlers;
using Rikstoto.WebAPI.Settings;
using Thinktecture.IdentityModel.Http.Cors.WebApi;
using log4net;

namespace Rikstoto.WebAPI
{
    public class WebApiConfigDefaults
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(WebApiConfigDefaults));

        private readonly string[] _allowedRequestHeaders = new[] {"Content-Type", "Authorization", "betdate" };

        public void Configure(IWindsorContainer container, HttpConfiguration configuration, WebAPISettings settings)
        {
            LogErrorAndHideUnobservedException();

            RegisterContainer(container, configuration);
            RegisterComponents(container);
            RegisterCors(configuration, settings.AllowedCORSOrigins);
            RegisterMessageHandlers(configuration, settings);
            RegisterCustomFormatters(configuration);
            RegisterGlobalFilters(configuration.Filters);
            RegisterRoutes(configuration.Routes);
            RegisterCustomErrorLevel(configuration);
        }

        private void LogErrorAndHideUnobservedException()
        {
            TaskScheduler.UnobservedTaskException += (sender, args) =>
            {
                Logger.Error("TASK FAILED", args.Exception);
                args.SetObserved();
            };
        }

        //http://brockallen.com/2012/06/28/cors-support-in-webapi-mvc-and-iis-with-thinktecture-identitymodel/
        protected virtual void RegisterCors(HttpConfiguration configuration, string allowedOrigins)
        {
            configuration.MessageHandlers.Add(new R22CorsWebkitHackHandler());
            var corsConfig = new WebApiCorsConfiguration();
            
            if(allowedOrigins == null)
                corsConfig
                    .ForAllOrigins()
                    .AllowAllMethods()
                    .AllowCookies()
                    .AllowRequestHeaders(_allowedRequestHeaders);
            else
                corsConfig
                    .ForOrigins(allowedOrigins.Split(','))
                    .AllowAllMethods()
                    .AllowCookies()
                    .AllowRequestHeaders(_allowedRequestHeaders);

            corsConfig.RegisterGlobal(configuration);
        }

        protected virtual void RegisterComponents(IWindsorContainer container)
        {
        }

        protected virtual void RegisterGlobalFilters(HttpFilterCollection filters)
        {
            filters.Add(new InternalServerExceptionFilter());
            filters.Add(new ModelValidationActionFilter());
            filters.Add(new ValidationExceptionFilter());
        }

        protected virtual void RegisterCustomErrorLevel(HttpConfiguration configuration)
        {
            configuration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
        }

        protected virtual void RegisterMessageHandlers(HttpConfiguration configuration, WebAPISettings settings)
        {
            configuration.MessageHandlers.Add(new LogTrafficHandler(settings.LogTraffic, configuration));
            configuration.MessageHandlers.Add(new LogMessageHandler(settings.LogLevel));
            configuration.MessageHandlers.Add(new NotAcceptableMessageHandler());
            configuration.MessageHandlers.Add(new UriFormatExtensionMessageHandler());
        }

        protected virtual void RegisterCustomFormatters(HttpConfiguration configuration)
        {
            var formatter = configuration.Formatters.OfType<JsonMediaTypeFormatter>().First();

            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };

            var converter = new IsoDateTimeConverter
                                {
                                    DateTimeStyles = DateTimeStyles.RoundtripKind,
                                    DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'sszzz"
                                };

            settings.Converters.Add(converter);

            formatter.SerializerSettings = settings;
        }

        protected virtual void RegisterContainer(IWindsorContainer container, HttpConfiguration configuration)
        {
            configuration.DependencyResolver = new WindsorDependencyResolver(container);
        }

        protected virtual void RegisterRoutes(HttpRouteCollection routes)
        {}
    }
}