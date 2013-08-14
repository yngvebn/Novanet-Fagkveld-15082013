using System.Threading;
using System.Web.Http;

namespace Rikstoto.WebAPI.Authentication.Attributes
{
    public class KnownUserOrAgentAttribute : AuthorizeAttribute
    {
        protected override bool IsAuthorized(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if(base.IsAuthorized(actionContext))
            {
                var agentPrincipal = Thread.CurrentPrincipal as AgentClaimsPrincipal;
                if(agentPrincipal != null && agentPrincipal.IdentityValid)
                    return true;

                var customerPrincipal = Thread.CurrentPrincipal as CustomerClaimsPrincipal;
                return (customerPrincipal != null && customerPrincipal.IdentityValid);
            }
            return false;
        }
    }
}