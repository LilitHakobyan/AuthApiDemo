using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace AuthApiDemo
{
    public class ClientStore
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("api")
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId()
            };
        }
        public static List<ApiScope> ApiScopes
        {
            get
            {
                List<ApiScope> apiScopes =
                    new List<ApiScope>();
                apiScopes.Add(new ApiScope
                    ("api", "Web API"));
               
                return apiScopes;
            }
        }
        public static IEnumerable<Client> GetClients()
        {
            var list = new List<Client> { new Client
            {
                ClientName = "Client Credential Flow",
                ClientId = "client_credential_flow",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets =
                {
                    new Secret("client_credential_flow_secret".Sha256())
                },
                AllowedScopes ={"api"},
                AllowOfflineAccess = false,
                AccessTokenLifetime = 60
            },new Client
            {
                ClientName = "Resource Owner Flow",
                ClientId = "resource_owner_flow",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                ClientSecrets =
                {
                    new Secret("resource_owner_flow_secret".Sha256())
                },
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.OfflineAccess
                },
                AllowOfflineAccess = true,
                RefreshTokenUsage = TokenUsage.ReUse,
                AccessTokenLifetime = 60,
                RefreshTokenExpiration = TokenExpiration.Absolute,
                AbsoluteRefreshTokenLifetime = 300
            }};
            return list;
        }
    }
}
