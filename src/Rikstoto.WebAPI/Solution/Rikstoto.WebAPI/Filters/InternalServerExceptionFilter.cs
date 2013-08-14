using System.Web.Http.Filters;
using Rikstoto.WebAPI.Exceptions;
using Rikstoto.WebAPI.Extentions;
using Rikstoto.WebAPI.Helpers;
using Rikstoto.WebAPI.Models;
using log4net;

namespace Rikstoto.WebAPI.Filters
{
    public class InternalServerExceptionFilter : ExceptionFilterAttribute
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(InternalServerExceptionFilter));

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception.GetType() != typeof(BadFormatException))
            {
                var errorTag = ErrorTagGenerator.NewErrorTag();

                actionExecutedContext.Response = actionExecutedContext.Request.CreateInternalServerErrorResponseMessage(
                        "An unhandeled exception of type " + actionExecutedContext.Exception.GetType() +
                        " occured.", errorTag);

                Logger.Error(
                    string.Format("Internal server error: {0} [ITAG:{1}]", actionExecutedContext.Exception.GetType(),
                                  errorTag),
                    actionExecutedContext.Exception);

                base.OnException(actionExecutedContext);
            }
        }
    }
}