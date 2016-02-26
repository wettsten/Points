﻿using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Twitter;

namespace Points.Api.Auth.Providers
{
    public class TwitterAuthProvider : ITwitterAuthenticationProvider
    {
        public void ApplyRedirect(TwitterApplyRedirectContext context)
        {
            context.Response.Redirect(context.RedirectUri);
        }

        public Task Authenticated(TwitterAuthenticatedContext context)
        {
            context.Identity.AddClaim(new Claim("ExternalAccessToken", context.AccessToken));
            return Task.FromResult<object>(null);
        }

        public Task ReturnEndpoint(TwitterReturnEndpointContext context)
        {
            return Task.FromResult<object>(null);
        }
    }
}
