using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;

namespace WebApiJwt.TokenAuth
{
    public class TokenAuthFilter : Attribute, IAuthenticationFilter
    {
        public bool AllowMultiple => false;

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            try
            {
                HttpRequestMessage request = context.Request;
                AuthenticationHeaderValue authorization = request.Headers.Authorization;

                if (authorization == null)
                {
                    context.ErrorResult = new AuthenticationFailureResult("Missing autorization header", request);
                    return;
                }
                if (authorization.Scheme != "Bearer")
                {
                    context.ErrorResult = new AuthenticationFailureResult("Invalid autorization scheme", request);
                    return;
                }
                if (String.IsNullOrEmpty(authorization.Parameter))
                {
                    context.ErrorResult = new AuthenticationFailureResult("Missing Token", request);
                    return;
                }

                SecurityToken correctToken = TokenManager.ValidateToken(authorization.Parameter);

                if (correctToken == null)
                    context.ErrorResult = new AuthenticationFailureResult("Invalid Token", request);

                if (DateTime.Now > correctToken.ValidTo )
                    context.ErrorResult = new AuthenticationFailureResult("Token Expired", request);
            }
            catch (Exception ex)
            {
                context.ErrorResult = new AuthenticationFailureResult("Exception: \n" + ex.Message, context.Request);
            }
        }

        public async Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return;
        }
    }
}