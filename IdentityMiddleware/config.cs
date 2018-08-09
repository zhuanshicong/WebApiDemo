﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                new ApiResource("api1", "My API"),
                new ApiResource("Scope1", "My Scope")
            };
            
        }
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                //new Client
                //{
                //    ClientId = "client",
                //    ClientName = "Client",
                //    // no interactive user, use the clientid/secret for authentication
                //    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                //    // secret for authentication
                //    ClientSecrets =
                //    {
                //        new Secret("secret".Sha256())
                //    },
                //    // scopes that client has access to
                //    AllowedScopes = { "Scope1" }
                //},
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
                    AllowedScopes = { "api1" }
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
}
}
