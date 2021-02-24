using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AccountWeb.Infrastructure;
using AccountWeb.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace AccountWeb
{
    public class Startup
    {

        private readonly IConfiguration _configuration;

        public Startup()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddDbContext<LiteIdentityDbContext>(options =>
            {
                options.UseSqlServer(_configuration.GetConnectionString("Test"));
            });

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<LiteIdentityDbContext>();

            services
                .AddAuthentication("Goosadf")
                .AddCookie("Goosadf", options =>
                {
                    options.Cookie.Name = "Goosadf";
                })
                .AddGoogle(options =>
                {
                    options.ClientId = "588393634591-3ufrhe4rp3jrtmlvibmlsng9p4f97hcb.apps.googleusercontent.com";
                    options.ClientSecret = "7NZxKLaPI1wvM7vKFYnsCErI";
                });
                //
                // services.AddAuthentication("CookieAuth")
                //     .AddCookie("CookieAuth", config =>
                //     {
                //         config.Cookie.Name = "Grandma.Cookie";
                //         config.LoginPath = "/Home/Authenticate";
                //     });
                
            services.AddScoped<IClaimsTransformation, ClaimsTransformationLite>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();

            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "default", pattern: "{Controller=Home}/{Action=Index}/{id?}");
            });
        }
    }
}