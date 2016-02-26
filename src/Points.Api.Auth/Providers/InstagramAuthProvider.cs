﻿using System.Security.Claims;
using System.Threading.Tasks;
using Aminjam.Owin.Security.Instagram;

namespace Points.Api.Auth.Providers
{
    public class InstagramAuthProvider : IInstagramAuthenticationProvider
    {
        public Task Authenticated(InstagramAuthenticatedContext context)
        {
            context.Identity.AddClaim(new Claim("ExternalAccessToken", context.AccessToken));
            return Task.FromResult<object>(null);
        }

        public Task ReturnEndpoint(InstagramReturnEndpointContext context)
        {
            return Task.FromResult<object>(null);
        }
    }
}
