using System.Diagnostics;
using Castle.DynamicProxy;
using log4net;

namespace Rikstoto.WebAPI.Interceptors
{
    public class TimerInterceptor : IInterceptor
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(TimerInterceptor));
        
        public void Intercept(IInvocation invocation)
        {
            var timer = Stopwatch.StartNew();
            
            invocation.Proceed();
            
            timer.Stop();

            if (invocation.Method.Name != "Dispose")
            {
                var elapsed = timer.ElapsedMilliseconds;
                _logger.Debug(string.Format("{0} took {1}ms to complete.", invocation.Method.Name, elapsed));
            }

        }
    }
}