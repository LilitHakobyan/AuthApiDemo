using System.Collections.Generic;
using System.Collections.Specialized;
using IdentityServer4.ResponseHandling;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using IdentityModel;
using IdpTokenResponse = IdentityServer4.ResponseHandling.TokenResponse;

namespace AuthApiDemo
{
    public class TokenProvider : ITokenProvider
    {
        private readonly ITokenRequestValidator RequestValidator;
        private readonly IClientSecretValidator ClientValidator;
        private readonly ITokenResponseGenerator ResponseGenerator;
        private readonly IHttpContextAccessor HttpContextAccessor;

        public TokenProvider(ITokenRequestValidator requestValidator, IClientSecretValidator clientValidator,
            ITokenResponseGenerator responseGenerator, IHttpContextAccessor httpContextAccessor)
        {
            this.RequestValidator = requestValidator;
            this.ClientValidator = clientValidator;
            this.ResponseGenerator = responseGenerator;
            this.HttpContextAccessor = httpContextAccessor;
        }
        public async Task<TokenResponse> GetToken(TokenRequest request)
        {
            var parameters = new NameValueCollection
            {
                { "username", request.Username },
                { "password", request.Password },
                { "grant_type", request.GrantType },
                { "scope", request.Scope },
                { "refresh_token", request.RefreshToken },
                { "response_type", OidcConstants.ResponseTypes.Token }
            };

            var response = await GetIdpToken(parameters);

            return GetTokenResponse(response);
        }

        private async Task<IdpTokenResponse> GetIdpToken(NameValueCollection parameters)
        {
            var clientResult = await ClientValidator.ValidateAsync(HttpContextAccessor.HttpContext);

            if (clientResult.IsError)
            {
                return new IdpTokenResponse
                {
                    Custom = new Dictionary<string, object>
                    {
                        { "Error", "invalid_client" },
                        { "ErrorDescription", "Invalid client/secret combination" }
                    }
                };
            }

            var validationResult = await RequestValidator.ValidateRequestAsync(parameters, clientResult);

            if (validationResult.IsError)
            {
                return new IdpTokenResponse
                {
                    Custom = new Dictionary<string, object>
                    {
                        { "Error", validationResult.Error },
                        { "ErrorDescription", validationResult.ErrorDescription }
                    }
                };
            }

            return await ResponseGenerator.ProcessAsync(validationResult);
        }

        private static TokenResponse GetTokenResponse(IdpTokenResponse response)
        {
            if (response.Custom != null && response.Custom.ContainsKey("Error"))
            {
                return new TokenResponse
                {
                    Error = response.Custom["Error"].ToString(),
                    ErrorDescription = response.Custom["ErrorDescription"]?.ToString()
                };
            }

            return new TokenResponse
            {
                AccessToken = response.AccessToken,
                RefreshToken = response.RefreshToken,
                ExpiresIn = response.AccessTokenLifetime,
                TokenType = "Bearer"
            };
        }
    }
}
