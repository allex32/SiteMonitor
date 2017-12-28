using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using SiteMonitor.Data.DbContexts;
using SiteMonitor.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SiteMonitor.Data
{
    public static class DbInitializer
    {
        private static object _lock = new object();
        private static bool _initialized = false;
        
        internal static void Seed(IServiceProvider serviceProvider)
        {
            if (!_initialized)
            {
                lock (_lock)
                {
                    if (_initialized)
                        return;

                    SeedIdentityData(serviceProvider);
                    _initialized = true;
                }
            }

        }
        private static void SeedIdentityData(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();            
            if (userManager == null)
                throw new InvalidOperationException($"Service {nameof(UserManager<ApplicationUser>)} is not configured");

            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();
            if (roleManager == null)
                throw new InvalidOperationException($"Service {nameof(RoleManager<IdentityRole>)} is not configured");
            
            
            EnsureSeededAdminData(userManager, roleManager).Wait();
        }

        private static async Task EnsureSeededAdminData(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {

            if (!await roleManager.RoleExistsAsync(PolicyNames.AdminRole))
                await roleManager.CreateAsync(new IdentityRole { Name = PolicyNames.AdminRole });

            var user = await userManager.FindByNameAsync(PolicyNames.AdminName);
            if (user == null)
            {
                user = new ApplicationUser { UserName = PolicyNames.AdminName };
                var creationResult = await userManager.CreateAsync(user, PolicyNames.AdminPassword);
                if (creationResult.Errors.Any())
                {
                    Debug.Assert(false, "Change exception message");
                    throw new InvalidOperationException("Invalid password");
                }
            }

            if (!await userManager.IsInRoleAsync(user, PolicyNames.AdminRole))
                await userManager.AddToRoleAsync(user, PolicyNames.AdminRole);

        }
    }
}
