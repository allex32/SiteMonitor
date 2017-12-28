using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SiteMonitor.Data;
using SiteMonitor.Models;
using SiteMonitor.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Hangfire;
using SiteMonitor.Models.Repositories;
using SiteMonitor.Services.SiteAvailability;
using SiteMonitor.Infrastructure.HostPinger;
using SiteMonitor.Data.DbContexts;
using SiteMonitor.Infrastructure.Hangfire;

namespace SiteMonitor
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
            services.AddDbContext<AppDataDbContext>(options =>
                 options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();


            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = -1;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 0;
            });

            //https://code.msdn.microsoft.com/windowsdesktop/Integrate-background-jobs-be713dc4
            services.AddHangfire(options =>
            {
                options.UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection"));
            });


            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddScoped<ISiteRepository, EFSiteRepository>();
            services.AddScoped<ISiteAvailabilityJobScheduler, SiteAvailabilityJobScheduler>();
            services.AddScoped<SiteAvailabilityJob, SiteAvailabilityJob>();
            services.AddScoped<ISiteAvailabilityService, SiteAvailabilityService>();
            services.AddScoped<IServerOnlineChecker, ServerOnlineChecker>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Site/Error");
            }

            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseHangfireDashboard(options: new DashboardOptions
            {
                Authorization = new[] { new AllowAdminDashboardAuthorizationFilter() }
            });
            app.UseHangfireServer();
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Site}/{action=Index}/{id?}");
            });
        }
    }
}
