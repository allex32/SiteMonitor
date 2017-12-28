using Microsoft.EntityFrameworkCore;
using SiteMonitor.Data.DbContexts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiteMonitor.Models.Repositories
{
    public class EFSiteRepository : ISiteRepository
    {
        private AppDataDbContext _dbContext;

        public EFSiteRepository(AppDataDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<Site>> Sites => _dbContext.Sites.ToListAsync();
      
        public async Task Create(Site site)
        {
            _dbContext.Add(site);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Site> Get(int? id)
        {
            return await _dbContext.Sites.SingleOrDefaultAsync(s => s.SiteId == id);
        }

        public async Task Update(Site site)
        {
            _dbContext.Entry(site).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(Site site)
        {
            _dbContext.Remove(site);
            await _dbContext.SaveChangesAsync();
        }

    }
}
