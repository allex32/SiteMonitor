using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteMonitor.Models.Repositories
{
    public interface ISiteRepository
    {
        Task<List<Site>> Sites { get; }

        Task<Site> Get(int? id);

        Task Create(Site site);

        Task Update(Site site);

        Task Delete(Site site);
    }
}
