using System.Net;
using System.Web.Http.Filters;
using Rikstoto.WebAPI.Exceptions;
using Rikstoto.WebAPI.Extentions;
using Rikstoto.WebAPI.Helpers;

namespace Rikstoto.WebAPI.Filters
{
    public class ValidationExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if(actionExecutedContext.Exception.GetType() == typeof(BadFormatException))
            {
                var errorMessage = actionExecutedContext.Exception.Message;

                actionExecutedContext.Response = actionExecutedContext.Request.CreateBadRequestErrorResponseMessage(errorMessage, ErrorTagGenerator.NewErrorTag(), ((int)HttpStatusCode.BadRequest).ToString());
            }

            base.OnException(actionExecutedContext);
        }
    }
}