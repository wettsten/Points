﻿using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security.MicrosoftAccount;

namespace Points.Api.Auth.Providers
{
    public class MicrosoftAuthProvider : IMicrosoftAccountAuthenticationProvider
    {
        public void ApplyRedirect(MicrosoftAccountApplyRedirectContext context)
        {
            context.Response.Redirect(context.RedirectUri);
        }

        public Task Authenticated(MicrosoftAccountAuthenticatedContext context)
        {
            context.Identity.AddClaim(new Claim("ExternalAccessToken", context.AccessToken));
            return Task.FromResult<object>(null);
        }

        public Task ReturnEndpoint(MicrosoftAccountReturnEndpointContext context)
        {
            return Task.FromResult<object>(null);
        }
    }
}
