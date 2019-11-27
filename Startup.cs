using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SkyPay.Data;
using SkyPay.Models;
//
using Microsoft.AspNet.OData.Routing.Conventions;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.OData.Edm;
using Joonasw.AspNetCore.SecurityHeaders;
using Microsoft.OData.UriParser;
namespace SkyPay
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                //.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var cs = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //services.AddIdentity<ApplicationUser, IdentityRole>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>()
            //    .AddDefaultTokenProviders();

            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //   .AddJwtBearer(options =>
            //   {
            //       options.TokenValidationParameters = new TokenValidationParameters
            //       {
            //           ValidateIssuer = true,
            //           ValidateAudience = true,
            //           ValidateLifetime = true,
            //           ValidateIssuerSigningKey = true,
            //           ValidIssuer = Configuration["Jwt:Issuer"],
            //           ValidAudience = Configuration["Jwt:Issuer"],
            //           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
            //       };
            //   });
            services.AddCsp(nonceByteAmount: 32);
            services.AddOData();
            services.AddODataQueryFilter();
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var provider = app.ApplicationServices.GetService<IServiceProvider>();
            var model = GetEdmModel(app.ApplicationServices);
            //
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Xss-Protection", "1");
                context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                await next();
            });
            //app.UseCsp(csp =>
            //{
            //    csp.AllowScripts
            //            .FromSelf().From("localhost:6990/*").AllowUnsafeInline()
            //            ;

            //    //.AddNonce();
            //    csp.AllowStyles
            //            .FromSelf().From("localhost:6990/*").AllowUnsafeInline();
            //            //.AddNonce();
            //});
            app.UseStaticFiles();
            //app.UseAuthentication();

            app.UseMvc(routes =>
            {
            routes.MapODataServiceRoute("odataroute", "odata", model);
                routes.MapRoute(
                    name: "spa",
                    template: "Manage/{link?}", defaults: new { controller = "Manage", action = "Index" });
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
                routes.EnableDependencyInjection();
                routes.Count().Expand().Filter().OrderBy().Select().MaxTop(null);

            });
        }
        private static IEdmModel GetEdmModel(IServiceProvider service)
        {
            var builder = new ODataConventionModelBuilder(service, true);
            builder.AddEnumType(typeof(CompanyType));
            //builder.EntitySet<User>("Users");
            builder.EntitySet<Company>("Companies");
            builder.EntitySet<Category>("Categories");
            builder.EntitySet<Unit>("Units");
            builder.EntitySet<Product>("Products");
            builder.EntitySet<ProductProps>("ProductProps");
            builder.EntitySet<CategoryProps>("CategoryProps");
            builder.EntitySet<Document>("Documents");
            builder.EntitySet<DocumentItem>("DocumentItems");
            return builder.GetEdmModel();
        }
    }
}
