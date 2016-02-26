﻿using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Facebook;

namespace Points.Api.Auth.Providers
{
    public class FacebookAuthProvider : IFacebookAuthenticationProvider
    {
        public void ApplyRedirect(FacebookApplyRedirectContext context)
        {
            context.Response.Redirect(context.RedirectUri);
        }

        public Task Authenticated(FacebookAuthenticatedContext context)
        {
            context.Identity.AddClaim(new Claim("ExternalAccessToken", context.AccessToken));
            return Task.FromResult<object>(null);
        }

        public Task ReturnEndpoint(FacebookReturnEndpointContext context)
        {
            return Task.FromResult<object>(null);
        }
    }
}