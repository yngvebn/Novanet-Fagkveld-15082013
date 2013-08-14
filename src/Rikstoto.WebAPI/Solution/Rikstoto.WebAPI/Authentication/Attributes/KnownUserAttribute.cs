using System.Threading;
using System.Web.Http;

namespace Rikstoto.WebAPI.Authentication.Attributes
{
    public class KnownUserAttribute : AuthorizeAttribute
    {
        protected override bool IsAuthorized(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if(base.IsAuthorized(actionContext))
            {
                var principal = Thread.CurrentPrincipal as CustomerClaimsPrincipal;
                return (principal != null && principal.IdentityValid);
            }
            return false;
        }
    }
}