using SiteMonitor.Models.Repositories;
using System;
using System.Threading.Tasks;
using SiteMonitor.Infrastructure.HostPinger;

namespace SiteMonitor.Services.SiteAvailability
{
    //Можно unit тесты написать
    public class SiteAvailabilityJob
    {
        ISiteRepository _siteRepository;
        IServerOnlineChecker _serverChecker;

        public SiteAvailabilityJob(ISiteRepository siteRepository, IServerOnlineChecker serverChecker)
        {
            _siteRepository = siteRepository;
            _serverChecker = serverChecker;
        }

        public async Task Run(int siteId)
        {
            var site = await _siteRepository.Get(siteId);
            if (site == null)
                throw new InvalidOperationException($"Site with id {siteId} not found in storage");

            if (site.Uri == null)
                throw new InvalidOperationException($"Uri can not be null");

            var pingResult = await _serverChecker.CheckServerOnlineAsync(site.Uri.AbsoluteUri);
            site.LastCheck = DateTime.Now;
            site.SiteAvailability = pingResult;
            await _siteRepository.Update(site);
        }
    }
}
