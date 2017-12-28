using Microsoft.EntityFrameworkCore;
using SiteMonitor.Models;


namespace SiteMonitor.Data.DbContexts
{
    public class AppDataDbContext : DbContext
    {
        public AppDataDbContext(DbContextOptions<AppDataDbContext> options) : base(options)
        {
        }
        public DbSet<Site> Sites { get; set; }
    }
}
