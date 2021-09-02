// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;

namespace MicroService.IdentityServer
{
    public static class Config
    {

        public static IEnumerable<ApiResource> ApiResources => new ApiResource[] {
                 new ApiResource("resource_catalog"){Scopes = {"catalog_fullpermission"}},
                 new ApiResource("photostock_catalog"){Scopes = {"photostock_fullpermission"}},
                 new ApiResource(IdentityServerConstants.LocalApi.ScopeName)

            };

        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                       new IdentityResources.Email(),
                       new IdentityResources.OpenId(),
                       new IdentityResources.Profile(),
                       new IdentityResource(){ Name = "roles", DisplayName="Roller", Description = "Kullanıcı rolleri",UserClaims = new []{"role"} },
                   };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("catalog_fullpermission","Catalog Api için Full erişim"),
                new ApiScope("photostock_fullpermission","PhotoStock Api için Full erişim"),
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client{
                    ClientName = "Asp.Net Core Mvc",
                    ClientId = "WebMvcClient",
                    ClientSecrets={ new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "catalog_fullpermission", "photostock_fullpermission", IdentityServerConstants.LocalApi.ScopeName }
                },
                new Client{
                    ClientName = "Asp.Net Core Mvc",
                    ClientId = "WebMvcClientForUser",
                    AllowOfflineAccess = true,
                    ClientSecrets={ new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes = { IdentityServerConstants.StandardScopes.Email, IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.Profile, IdentityServerConstants.StandardScopes.OfflineAccess, IdentityServerConstants.LocalApi.ScopeName, "roles" },
                    AccessTokenLifetime = 1*60*60, // Access token geçerlilik süresi
                    //refresh token kullanıcının haberi olmadan access token süresi dolunca yeni bir access token alabilmeyi sağlıyor.
                    RefreshTokenExpiration= TokenExpiration.Absolute, // refresh token istedikce ömrü artsınmı
                    AbsoluteRefreshTokenLifetime = (int)(DateTime.Now.AddDays(60) - DateTime.Now  ).TotalSeconds, //refresh token süresi
                    RefreshTokenUsage= TokenUsage.ReUse // Birdefamı kullanılsın birden fazlamı kullanılsun
                }
            };
    }
}