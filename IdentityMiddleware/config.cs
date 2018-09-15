using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Builder;
namespace IdentityMiddleware
{
    public class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("api1", "My API",new List<string>(){ClaimTypes.Name,JwtClaimTypes.Role,"Area"}),
                new ApiResource("UserManger", "UserManger Scope",new List<string>(){ClaimTypes.Name,JwtClaimTypes.Role})
            };
            
        }
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client1",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret1".Sha256())
                    },
                    // scopes that client has access to
                    AllowedScopes = { "api1",IdentityServerConstants.StandardScopes.OpenId,"ClaimsInfo" },
                    AllowedCorsOrigins={"http://localhost:5001"},
                    
                },
                new Client{
                    ClientId = "UserManger",
                    AllowedGrantTypes =GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("UserMangerSecret".Sha256())
                    },
                    AllowedScopes = { 
                        "UserManger",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "ClaimsInfo" }
                },
                new Client
                {
                    ClientId = "test",
                    ClientName = "Test Client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = { "http://localhost:5003/callback.html" },
                    PostLogoutRedirectUris = { "http://localhost:5003/index.html" },
                    AllowedCorsOrigins = { "http://localhost:5003" },
                    AllowedScopes =
                    {
                        "api1",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "ClaimsInfo"
                    },
                }
            };
        }
        

        public static List<TestUser> GetUsers()
    {
        return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "alice",
                    Password = "password"
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "bob",
                    Password = "password"
                }
            };
    }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                //new IdentityResources.Email(),
                new IdentityResource
                {
                    Name = "ClaimsInfo",
                    DisplayName="ClaimsInfo",
                    Description="Show user Claims Info.",
                    UserClaims = new[]{JwtClaimTypes.Role},
                    ShowInDiscoveryDocument = true,
                    Required=true,
                    Emphasize = true
                }
            };
        }
    }
}
