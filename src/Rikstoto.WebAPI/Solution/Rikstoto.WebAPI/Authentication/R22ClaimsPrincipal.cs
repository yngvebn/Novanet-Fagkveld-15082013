using System;
using System.Linq;
using Microsoft.IdentityModel.Claims;
using Rikstoto.Token;
using Thinktecture.IdentityModel.Tokens;

namespace Rikstoto.WebAPI.Authentication
{
    public abstract class R22ClaimsPrincipal : ClaimsPrincipal
    {
        protected R22ClaimsPrincipal(ClaimsIdentityCollection identityCollection, JsonWebToken token, string encryptedToken)
            : base(identityCollection)
        {
            Token = token;
            EncryptedToken = encryptedToken;
        }

        public string EncryptedToken { get; set; }
        public JsonWebToken Token { get; set; }

        public bool IdentityValid
        {
            get
            {
                var expires = Token.ExpirationTime.GetValueOrDefault().ToDateTime();
                return expires > DateTime.Now;
            }
        }

        protected Claim GetClaim(string claimType)
        {
            if (Token != null)
            {
                if (Token.Claims != null)
                {
                    return Token.Claims.FirstOrDefault(c => c.ClaimType == claimType);
                }
            }

            return null;
        }
    }
}