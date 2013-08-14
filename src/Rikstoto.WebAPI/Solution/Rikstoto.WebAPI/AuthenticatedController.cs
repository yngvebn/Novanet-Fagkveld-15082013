using System.Web.Http;
using Rikstoto.WebAPI.Authentication;
using Rikstoto.WebAPI.Settings;

namespace Rikstoto.WebAPI
{
    public class AuthenticatedController : ApiController
    {
        public WebAPISettings Settings { get; set; }

        protected int CustomerId
        {
            get
            {
                var principal = User as CustomerClaimsPrincipal;

                if (principal != null)
                {
                    return principal.CustomerId;
                }

                return -1;
            }
        }

        protected int BettingCardSequenceNumber
        {
            get
            {
                var principal = User as CustomerClaimsPrincipal;

                if (principal != null)
                {
                    return principal.BettingCardSequenceNumber;
                }

                return -1;
            }
        }
        
        protected string OwnerTrack
        {
            get
            {
                var principal = User as CustomerClaimsPrincipal;

                if (principal != null)
                {
                    return principal.OwnerTrack;
                }

                return string.Empty;
            }
        }

        protected string PaymentAccountNumber
        {
            get
            {
                var principal = User as CustomerClaimsPrincipal;

                if (principal != null)
                {
                    return principal.PaymentAccountNumber;
                }

                return string.Empty;
            }
        }
    }
}