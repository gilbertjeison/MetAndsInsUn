using MetAndsInsUn.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using VueCliMiddleware;

namespace MetAndsInsUn
{
    public class Startup
    {
        private string url_dev = "http://localhost:5093/";
        private string url_loc = "http://localhost/identity/";
        private string url_prod = "http://appmtto7-001-site4.itempurl.com";
        string origin_1 = "http://metinsman.digitoolsunilever.com";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore); ;

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    //options.Authority = url_dev;
                    options.Authority = url_loc;
                    //options.Authority = url_prod;
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "api1";

                });

            services.AddDbContext<MetAndInsContext>(options
               => options.UseLazyLoadingProxies()
                    .UseSqlServer(Configuration.GetConnectionString("MetAndInstDatabaseLoc")));

           services.AddCors();

            //// To list physical files from a path provided by configuration:
            //var physicalProvider = new PhysicalFileProvider(Configuration.GetValue<string>("StoredFilesPath"));

            //// To list physical files in the temporary files folder, use:
            ////var physicalProvider = new PhysicalFileProvider(Path.GetTempPath());
            //services.AddSingleton<IFileProvider>(physicalProvider);
        }

        

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    //app.UseExceptionHandler("/Error");

            //    //app.UseHsts();
            //    app.UseHttpsRedirection();
            //}
            app.UseCors(opt => opt.AllowAnyOrigin());
            app.UseDeveloperExceptionPage();

            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseAuthentication();
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");

                //routes.MapSpaFallbackRoute(
                //    name: "spa-fallback",
                //    defaults: new { controller = "CatchAll", action = "Index" }
                //    );
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    // run npm process with client app
                    spa.UseVueCli(npmScript: "serve", port: 8080);
                    // if you just prefer to proxy requests from client app, use proxy to SPA dev server instead:
                    // app should be already running before starting a .NET client
                    // spa.UseProxyToSpaDevelopmentServer("http://localhost:8080"); // your Vue app port
                }
            });
        }


    }
}
