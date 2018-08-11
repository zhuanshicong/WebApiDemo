using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
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
                new ApiResource("Scope1", "My Scope")
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
                    AllowedScopes = { "api1","openid","ClaimsInfo" }
                    
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
                new IdentityResources.Email(),
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
