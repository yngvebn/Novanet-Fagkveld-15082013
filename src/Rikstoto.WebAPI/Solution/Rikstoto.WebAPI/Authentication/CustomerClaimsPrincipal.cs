using System;
using System.Linq;
using Microsoft.IdentityModel.Claims;
using Rikstoto.Token;
using Thinktecture.IdentityModel.Tokens;

namespace Rikstoto.WebAPI.Authentication
{
    public class CustomerClaimsPrincipal : R22ClaimsPrincipal
    {
        public CustomerClaimsPrincipal(ClaimsIdentityCollection identityCollection, JsonWebToken token, string encryptedToken)
            : base(identityCollection, token, encryptedToken)
        {
        }

        public bool RecentlyAuthenticated
        {
            get
            {
                if (IdentityValid == false)
                    return false;

                var purchaseExpiresClaim = Token.Claims.First(c => c.ClaimType == RikstotoClaimTypes.PurchaseExpires);
                var purchaseExpires = Convert.ToInt64(purchaseExpiresClaim.Value).ToDateTime();
                return purchaseExpires > DateTime.Now;
            }
        }

        public int CustomerId
        {
            get
            {
                var claim = GetClaim(RikstotoClaimTypes.CustomerId);

                if (claim != null)
                {
                    int customerId;

                    if (int.TryParse(claim.Value, out customerId))
                    {
                        return customerId;
                    }
                }

                return -1;
            }
        }

        public int BettingCardSequenceNumber
        {
            get
            {
                var claim = GetClaim(RikstotoClaimTypes.SequenceNumber);

                if (claim != null)
                {
                    int sequenceNumber;

                    if (int.TryParse(claim.Value, out sequenceNumber))
                    {
                        return sequenceNumber;
                    }
                }

                return -1;
            }
        }
        
        public string OwnerTrack
        {
            get
            {
                var claim = GetClaim(RikstotoClaimTypes.OwnerTrack);

                if (claim != null)
                {
                    return claim.Value;
                }

                return string.Empty;
            }
        }

        public string PaymentAccountNumber
        {
            get
            {
                var claim = GetClaim(RikstotoClaimTypes.PaymentAccountNumber);

                if (claim != null)
                {
                    return claim.Value;
                }

                return string.Empty;
            }
        }
    }
}