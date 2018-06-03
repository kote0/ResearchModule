using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ResearchModule.Data;
using ResearchModule.Service;
using ResearchModule.Models;
using Microsoft.AspNetCore.Identity;
using ResearchModule.Repository;
using ResearchModule.Repository.Interfaces;
using ResearchModule.Managers;
using ResearchModule.Managers.Interfaces;
using Microsoft.Extensions.Logging;
using ResearchModule.Components.Page;

namespace ResearchModule
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public static IServiceCollection Services { get; private set; }
        public static ServiceProvider ServiceProvider { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region default

            services.AddMvc();

            services.AddSingleton(new PublicationElements());

            services.AddDbContext<DBContext>();
            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 1;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<DBContext>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("admin", policy => policy.RequireRole("admin"));
            });
            services.AddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, Microsoft.AspNetCore.Http.HttpContextAccessor>();
            services.AddScoped<UserManager>();
            
            #endregion

            services.AddScoped<IBaseRepository, BaseRepository>();
            services.AddTransient<AuthorService>();
            services.AddTransient<PublicationService>();
            services.AddTransient<SelectListService>();
            services.AddScoped<IFileManager, FileManager>();
            //services.AddScoped<ILogger>();
            services.AddTransient<PAuthorRepository>();
            services.AddScoped<PublicationManager>();
            services.AddScoped<ChartService>();

            Services = services;
            ServiceProvider = services.BuildServiceProvider();
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
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Publication}/{action=CreatePublicationNew}/{id?}");
            });
        }
    }
}
