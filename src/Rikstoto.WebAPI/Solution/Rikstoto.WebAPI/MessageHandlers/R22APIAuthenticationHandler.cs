using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.IdentityModel.Claims;
using Rikstoto.Token;
using Rikstoto.Token.Settings;
using Rikstoto.WebAPI.Authentication;
using Thinktecture.IdentityModel;

namespace Rikstoto.WebAPI.MessageHandlers
{
    public class R22APIAuthenticationHandler : DelegatingHandler
    {
        private readonly JWTCustomerAuthenticator _customerAuthenticator;
        private readonly JWTAgentAuthenticator _agentAuthenticator;

        public R22APIAuthenticationHandler(R22AuthenticationType authenticationType, TokenSettings settings)
        {
            if(authenticationType == R22AuthenticationType.Customer || authenticationType == R22AuthenticationType.CustomerAndAgent)
                _customerAuthenticator = new JWTCustomerAuthenticator(settings.CustomerTokenSigningKey);
            
            if(authenticationType == R22AuthenticationType.Agent || authenticationType == R22AuthenticationType.CustomerAndAgent)
                _agentAuthenticator = new JWTAgentAuthenticator(settings.AgentTokenSigningKey);
        }
        
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var authHeader = request.Headers.Authorization;
            bool hasBearerAutorization = authHeader != null && authHeader.Scheme == JWTConstants.AuthScheme;
            
            if (hasBearerAutorization == false)
                return SetThreadPrincipalAndContinue(request, cancellationToken, Principal.Anonymous);

            var customerPrincipal = ValidateCustomerToken(authHeader.Parameter);
            if(customerPrincipal != null)
                return SetThreadPrincipalAndContinue(request, cancellationToken, customerPrincipal);
            
            var agentPrincipal = ValidateAgentToken(authHeader.Parameter);
            if(agentPrincipal != null)
                return SetThreadPrincipalAndContinue(request, cancellationToken, agentPrincipal);
            
            return SendUnauthorizedResponse();
        }

        private Task<HttpResponseMessage> SetThreadPrincipalAndContinue(HttpRequestMessage request, CancellationToken cancellationToken,
                                             ClaimsPrincipal customerPrincipal)
        {
            Thread.CurrentPrincipal = customerPrincipal;

            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = customerPrincipal;
            }

            return base.SendAsync(request, cancellationToken);
        }

        private ClaimsPrincipal ValidateCustomerToken(string tokenString)
        {
            try
            {
                if (_customerAuthenticator == null)
                    return null;

                var tokenPair = _customerAuthenticator.Authenticate(tokenString);
                return new CustomerClaimsPrincipal(tokenPair.Claims, tokenPair.Token, tokenString);
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        private ClaimsPrincipal ValidateAgentToken(string tokenString)
        {
            try
            {
                if(_agentAuthenticator == null)
                    return null;

                var tokenPair = _agentAuthenticator.Authenticate(tokenString);
                return new AgentClaimsPrincipal(tokenPair.Claims, tokenPair.Token, tokenString);
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        private static Task<HttpResponseMessage> SendUnauthorizedResponse()
        {
            return Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                response.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue(JWTConstants.AuthScheme));
                return response;
            });
        }
    }
}