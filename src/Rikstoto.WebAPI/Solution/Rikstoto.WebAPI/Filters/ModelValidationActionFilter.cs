using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Rikstoto.WebAPI.Extentions;
using Rikstoto.WebAPI.Helpers;

namespace Rikstoto.WebAPI.Filters
{
    public class ModelValidationActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var stateErrors = GetModelStateErrors(actionContext);
            if (stateErrors != null)
            {
                var stateErrorMessage = string.Join(",", stateErrors);
                var errorMessage = "The request was badly formatted and had the following errors: " + stateErrorMessage +
                                   " Use the error tag when contacting Norsk Rikstoto for further information.";

                actionContext.Response = actionContext.Request.CreateBadRequestErrorResponseMessage(errorMessage, ErrorTagGenerator.NewErrorTag(),((int)HttpStatusCode.BadRequest).ToString());
            }
        }

        private static IEnumerable<string> GetModelStateErrors(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                return actionContext.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .Select((e, i) => string.Format("[{0}] {1}", i + 1, e.Value.Errors.First().GetErrorMessage()));
            }
            return null;
        }

    }
}