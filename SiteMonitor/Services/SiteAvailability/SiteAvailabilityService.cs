using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SiteMonitor.Models;
using SiteMonitor.Models.Repositories;
using System.Transactions;

namespace SiteMonitor.Services.SiteAvailability
{
    public class SiteAvailabilityService : ISiteAvailabilityService
    {
        private readonly ISiteRepository _siteRepository;
        private readonly ISiteAvailabilityJobScheduler _jobScheduler;

        public SiteAvailabilityService(ISiteRepository siteRepository, ISiteAvailabilityJobScheduler jobScheduler)
        {
            _siteRepository = siteRepository;
            _jobScheduler = jobScheduler;
        }
        public Task<List<Site>> Sites => _siteRepository.Sites;


        //ToDo. Добавить транзакционность или пересмотреть архитектуру
        public async Task ChangeCheckTimeSpanAsync(Site site, TimeSpan newTimeSpan)
        {
            site.CheckOnlineTimespan = newTimeSpan;

            await _siteRepository.Update(site);
            _jobScheduler.CreateOrUpdateTask(site);

        }

        //ToDo. Добавить транзакционность или пересмотреть архитектуру
        public async Task CreateAndSetUpAsync(Site site)
        {
            await _siteRepository.Create(site);
            _jobScheduler.CreateOrUpdateTask(site);
        }

        //ToDo. Добавить транзакционность или пересмотреть архитектуру
        public async Task DeleteSiteAsync(int id)
        {
            
            var site = await this.GetAsync(id);
            await _siteRepository.Delete(site);
            _jobScheduler.DeleteTask(site);
            
        }

        public async Task<Site> GetAsync(int? id)
        {
            return await _siteRepository.Get(id);
        }

        public  async Task<bool> SiteExistsAsync(int id)
        {
            var site = await _siteRepository.Get(id);
            return site != null;
        }
    }
}
