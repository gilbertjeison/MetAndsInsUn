// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using IdentityServer.Quickstart.Extension;
using IdentityServer4;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer
{
    public class Startup
    {
        public IHostingEnvironment Environment { get; }
        private readonly ILogger<Startup> _logger;
        string conn_str = "Data Source=SQL7005.site4now.net;Initial Catalog=DB_A46034_identityserver;User Id=DB_A46034_identityserver_admin;Password=Appmtto70722;";
        string migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
        string origin_1 = "http://metinsman.digitoolsunilever.com";

        public Startup(ILogger<Startup> logger, IHostingEnvironment environment)
        {
            _logger = logger;
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_1);

            var builder = services.AddIdentityServer()
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApis())
                .AddInMemoryClients(Config.GetClients())
                .AddTestUsers(Config.GetUsers());

            //var builder = services.AddIdentityServer()
            //    .AddTestUsers(Config.GetUsers())
            //    // this adds the config data from DB (clients, resources)
            //    .AddConfigurationStore(options =>
            //    {
            //        options.ConfigureDbContext = b =>
            //            b.UseSqlServer(conn_str, sql =>
            //            sql.MigrationsAssembly(migrationsAssembly));
            //    })
            //    // this adds the operational data from DB (codes, tokens, consents)
            //    .AddOperationalStore(options =>
            //    {
            //        options.ConfigureDbContext = b =>
            //        b.UseSqlServer(conn_str,
            //        sql => sql.MigrationsAssembly(migrationsAssembly));

            //        // this enables automatic token cleanup. this is optional.
            //        options.EnableTokenCleanup = true;
            //    });

            if (Environment.IsDevelopment())
            {               
                builder.AddDeveloperSigningCredential();
            }
            else
            {                
                try
                {
                    var PathWithFolderName = Path.Combine(Environment.WebRootPath, "resources");
                    // load certificate into MachineKeySet which does NOT require a User Profile to be loaded for crypto: https://github.com/dotnet/corefx/issues/23780
                    builder.AddSigningCredential(new X509Certificate2(PathWithFolderName + "/identityserver-cert.pfx", "Appmtto70722", X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.EphemeralKeySet));

                    
                    //X509Certificate2 cert = new X509Certificate2(File.ReadAllBytes(PathWithFolderName + "/identityserver-cert.pfx"), "Appmtto70722");
                    //builder.AddSigningCredential(cert);
                    //throw new Exception("need to configure key material");
                }
                catch (Exception ex)
                {
                    _logger.LogInformation("Error personalizado: " + ex.ToString());
                }
            }
           
            services.AddAuthentication()
                .AddGoogle("Google", options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                    options.ClientId = "<insert here>";
                    options.ClientSecret = "<insert here>";
                })
                .AddOpenIdConnect("oidc", "OpenID Connect", options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    options.SignOutScheme = IdentityServerConstants.SignoutScheme;
                    options.SaveTokens = true;

                    options.Authority = "https://demo.identityserver.io/";
                    options.ClientId = "implicit";

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = "name",
                        RoleClaimType = "role"
                    };
                });

            //services.AddCors();
        }

        public void Configure(IApplicationBuilder app, ILogger<Startup> logger)
        {
            // this will do the initial DB population
            //InitializeDatabase(app);

            //if (Environment.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{

            //}

            //app.UseCors(opt => opt.AllowAnyOrigin());

            app.ConfigureExceptionHandler(logger);

            app.UseStaticFiles();

            app.UseIdentityServer();

            app.UseMvcWithDefaultRoute();
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
               
                context.Database.Migrate();
                if (!context.Clients.Any())
                {
                    foreach (var client in Config.GetClients())
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in Config.GetIdentityResources())
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiResources.Any())
                {
                    foreach (var resource in Config.GetApis())
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}