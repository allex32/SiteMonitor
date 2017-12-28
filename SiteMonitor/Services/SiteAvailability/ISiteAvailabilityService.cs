using SiteMonitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteMonitor.Services.SiteAvailability
{
    public interface ISiteAvailabilityService
    {
        Task<List<Site>> Sites { get; }
        Task<Site> GetAsync(int? id);
        Task CreateAndSetUpAsync(Site site);
        Task DeleteSiteAsync(int id);
        Task ChangeCheckTimeSpanAsync(Site site, TimeSpan newTimeSpan);

        Task<bool> SiteExistsAsync(int id);

    }
}
