using System;
using Microsoft.IdentityModel.Claims;
using Rikstoto.Token;
using Thinktecture.IdentityModel.Tokens;

namespace Rikstoto.WebAPI.Authentication
{
    public class AgentClaimsPrincipal : R22ClaimsPrincipal
    {
        public AgentClaimsPrincipal(ClaimsIdentityCollection identityCollection, JsonWebToken token, string encryptedToken)
            : base(identityCollection, token, encryptedToken)
        {
        }

        public string AgentKey
        {
            get
            {
                Claim claim = GetClaim(RikstotoClaimTypes.AgentId);

                if (claim == null)
                    throw new NullReferenceException("AgentId claim is null");

                return claim.Value;
            }
        }

        public string TrackAffiliation
        {
            get { return GetClaim(RikstotoClaimTypes.TrackAffiliation).Value; }
        }

        public string TypeOfTerminal
        {
            get { return GetClaim(RikstotoClaimTypes.BetSource).Value; }
        }

        public int WindowNumber
        {
            get
            {
                Claim claim = GetClaim(RikstotoClaimTypes.WindowNumber);

                if (claim == null)
                    throw new NullReferenceException("WindowNumber is null");

                return Int32.Parse(claim.Value);
            }
        }
    }
}