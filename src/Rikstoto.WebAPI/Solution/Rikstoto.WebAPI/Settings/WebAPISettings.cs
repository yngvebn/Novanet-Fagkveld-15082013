using System;
using Rikstoto.WebAPI.Logging;

namespace Rikstoto.WebAPI.Settings
{
    public class WebAPISettings
    {
        public WebAPISettings()
        {
            CacheDurationPast =  new TimeSpan(3,0,0);
            CacheDurationFuture = new TimeSpan(0,30,0);
            CacheDurationToday = new TimeSpan(0,5,0);
        }

        public TimeSpan CacheDurationPast { get; set; }
        public TimeSpan CacheDurationFuture { get; set; }
        public TimeSpan CacheDurationToday { get; set; }

        public string LinkBaseUri { get; set; }
        public string HostUri { get; set; }
        public LogLevel LogLevel { get; set; }
        public string AllowedCORSOrigins { get; set; }

        public int MaxConcurrentRequests { get; set; }

        public bool EnableTrace { get; set; }

        public bool LogTraffic { get; set; }
    }
}