using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Validation;

namespace AuthApiDemo
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            if (context.Request.Raw["password"] != "password" ||
                context.Request.Raw["username"] != "username")
            {
                return Task.CompletedTask;
            }

            context.Result.IsError = false;
            context.Result.Subject = GetClaimsPrincipal();

            return Task.CompletedTask;
        }

        private static ClaimsPrincipal GetClaimsPrincipal()
        {
            var issued = DateTimeOffset.Now.ToUnixTimeSeconds();

            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject, Guid.NewGuid().ToString()),
                new Claim(JwtClaimTypes.AuthenticationTime, issued.ToString()),
                new Claim(JwtClaimTypes.IdentityProvider, "localhost")
            };

            return new ClaimsPrincipal(new ClaimsIdentity(claims));
        }
    }
}
