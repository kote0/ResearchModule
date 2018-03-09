using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using ResearchModule.Data;
using ResearchModule.Service;
using ResearchModule.Models;
using Microsoft.AspNetCore.Identity;
using ResearchModule.Managers;

namespace ResearchModule
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<PublicationService>();
            services.AddTransient<SelectListService>();
            services.AddTransient<FileManager>();
            services.AddTransient<PAManager>();
            
            services.AddSingleton(new PublicationElements());
            
            services.AddDbContext<DBContext>();
            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 1;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<DBContext>();
            
            services.AddTransient<BaseManager>();
            services.AddMvc();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdministratorOnly", policy => policy.RequireRole("Administrator"));
                options.AddPolicy("DeletePublication", policy => policy.RequireClaim("Delete Publication", "Delete Publication"));
                options.AddPolicy("AddPublication", policy => policy.RequireClaim("Add Publication", "Add Publication"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Base}/{action=Index}/{id?}");
            });
        }
    }
}
