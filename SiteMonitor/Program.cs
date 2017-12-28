using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using SiteMonitor.Data;
using SiteMonitor.Infrastructure.Hangfire;

namespace SiteMonitor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildMainWebHost(args);
                   
            SeedDatabases(host.Services);
            host.Run();
        }

        private static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<MigrationStartup>()
                .Build();

        public static IWebHost BuildMainWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()               
                //https://blogs.msdn.microsoft.com/dotnet/2017/05/12/announcing-ef-core-2-0-preview-1/
                .UseDefaultServiceProvider(options =>
                    options.ValidateScopes = false)            
                .Build();

        private static void SeedDatabases(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                try
                {
                    DbInitializer.Seed(scope.ServiceProvider);
                }
                catch(Exception ex)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                    throw;
                }
            }
        }
    }
}
