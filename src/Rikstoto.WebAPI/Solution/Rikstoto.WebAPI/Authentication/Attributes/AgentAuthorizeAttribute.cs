using System.Threading;
using System.Web.Http;

namespace Rikstoto.WebAPI.Authentication.Attributes
{
    public class AgentAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool IsAuthorized(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (!base.IsAuthorized(actionContext))
                return false;

            var principal = Thread.CurrentPrincipal as AgentClaimsPrincipal;

            return principal != null && principal.IdentityValid;
        }
    }
}