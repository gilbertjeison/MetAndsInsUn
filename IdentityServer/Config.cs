// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServer
{
    public static class Config
    {        
        static string redirect_dev = "http://localhost:5000/callback";
        static string redirect_loc = "http://localhost/metins/callback";
        static string redirect_prod = "http://metinsman.digitoolsunilever.com/callback";

        static string post_logout_dev = "http://localhost:5000/main";
        static string post_logout_loc = "http://localhost/metins/main";
        static string post_logout_prod = "http://metinsman.digitoolsunilever.com/main";

        static string silent_redirect_dev = "http://localhost:5000/static/silent-renew.html";
        static string silent_redirect_loc = "http://localhost/metins/static/silent-renew.html";
        static string silent_redirect_prod = "http://metinsman.digitoolsunilever.com/static/silent-renew.html";

        static string allowed_cors_dev = "http://localhost:5000";
        static string allowed_cors_loc = "http://localhost/metins";
        static string allowed_cors_prod = "http://metinsman.digitoolsunilever.com";

        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "metro",
                    Password = "metro",

                    Claims = new []
                    {
                        new Claim("name", "Metro"),
                        new Claim("website", "https://alice.com")
                    }
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "mtto",
                    Password = "mtto",

                    Claims = new []
                    {
                        new Claim("name", "Mtto"),
                        new Claim("website", "https://bob.com")
                    }
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
            {
                new ApiResource("api1", "My API")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = { "api1" }
                },
                // resource owner password grant client
                new Client
                {
                    ClientId = "ro.client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "api1" }
                },
                // OpenID Connect hybrid flow client (MVC)
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",
                    AllowedGrantTypes = GrantTypes.Hybrid,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    RedirectUris           = { "http://localhost:5002/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1"
                    },

                    AllowOfflineAccess = true
                },
                // JavaScript Client
                new Client
                {
                    ClientId = "js",
                    ClientName = "JavaScript Client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    RequirePkce = true,
                    //RequireClientSecret = false,
                    AllowAccessTokensViaBrowser= true,
                    AllowOfflineAccess= true,
                    AccessTokenLifetime = 900000,
                    AbsoluteRefreshTokenLifetime = 0,
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    UpdateAccessTokenClaimsOnRefresh = true,
                    RequireConsent = false,

                    RedirectUris =           { redirect_dev, silent_redirect_dev },
                    PostLogoutRedirectUris = { post_logout_dev },
                    AllowedCorsOrigins =     { allowed_cors_dev },

                    //RedirectUris =           { redirect_loc, silent_redirect_loc },
                    //PostLogoutRedirectUris = { post_logout_loc },
                    //AllowedCorsOrigins =     { allowed_cors_loc },

                    //RedirectUris =           { redirect_prod, silent_redirect_prod },
                    //PostLogoutRedirectUris = { post_logout_prod },
                    //AllowedCorsOrigins =     { allowed_cors_prod },


                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1"
                    }
                }
            };
        }
    }
}