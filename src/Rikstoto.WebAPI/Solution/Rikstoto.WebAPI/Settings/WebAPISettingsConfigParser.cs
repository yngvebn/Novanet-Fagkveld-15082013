using System;
using System.Configuration;
using Rikstoto.WebAPI.Logging;

namespace Rikstoto.WebAPI.Settings
{
    public static class WebAPISettingsConfigParser
    {
        public static WebAPISettings ParseFromConfig(string prefix = "WebAPI")
        {
            var logLevel = ConfigurationManager.AppSettings[prefix + ".LogLevel"];
            var webAPISettings = new WebAPISettings
                                     {
                                         HostUri = ConfigurationManager.AppSettings[prefix + ".HostUri"],
                                         LinkBaseUri = ConfigurationManager.AppSettings[prefix + ".LinkBaseUri"],
                                         LogLevel = logLevel != null ? ParseEnum<LogLevel>(logLevel) : LogLevel.None,
                                         AllowedCORSOrigins = ConfigurationManager.AppSettings[prefix + ".AllowedCORSOrigins"],
                                         EnableTrace = bool.Parse(ConfigurationManager.AppSettings[prefix + ".EnableTrace"] ?? "false"),
                                         LogTraffic = bool.Parse(ConfigurationManager.AppSettings[prefix + ".LogWebAPITraffic"] ?? "false")
                                     };

            TimeSpan cacheDurationPast;
            if(TimeSpan.TryParse(ConfigurationManager.AppSettings[prefix + ".CacheDurationPast"], out cacheDurationPast))
            {
                webAPISettings.CacheDurationPast = cacheDurationPast;
            }

            TimeSpan cacheDurationFuture;
            if(TimeSpan.TryParse(ConfigurationManager.AppSettings[prefix + ".CacheDurationFuture"], out cacheDurationFuture))
            {
                webAPISettings.CacheDurationFuture = cacheDurationFuture;
            }

            TimeSpan cacheDurationToday;
            if(TimeSpan.TryParse(ConfigurationManager.AppSettings[prefix + ".CacheDurationToday"], out cacheDurationToday))
            {
                webAPISettings.CacheDurationToday = cacheDurationToday;
            }

            int maxConcurrentRequests;
            webAPISettings.MaxConcurrentRequests = int.TryParse(ConfigurationManager.AppSettings[prefix + ".MaxConcurrentRequests"], out maxConcurrentRequests) ? maxConcurrentRequests : 100;

            return webAPISettings;
        }
        
        private static T ParseEnum<T>(string value) where T : struct, IConvertible
        {
            return (T) Enum.Parse(typeof (T), value);
        }
    }
}