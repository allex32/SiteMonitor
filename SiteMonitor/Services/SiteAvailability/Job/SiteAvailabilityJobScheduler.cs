using SiteMonitor.Models;
using Hangfire;
using Hangfire.Common;

namespace SiteMonitor.Services.SiteAvailability
{
    public class SiteAvailabilityJobScheduler : ISiteAvailabilityJobScheduler
    {     
        IRecurringJobManager _jobManager;
        SiteAvailabilityJob _siteJob;

        public SiteAvailabilityJobScheduler(IRecurringJobManager jobManager, SiteAvailabilityJob siteJob)
        {         
            _jobManager = jobManager;
            _siteJob = siteJob;
        }

        public void CreateOrUpdateTask(Site site)
        {                                 
            _jobManager.AddOrUpdate(site.SiteId.ToString(),
                Job.FromExpression(() => _siteJob.Run(site.SiteId)), 
                Cron.MinuteInterval((int)site.CheckOnlineTimespan.TotalMinutes));
        }        

        public void DeleteTask(Site site)
        {
            _jobManager.RemoveIfExists(site.SiteId.ToString());
        }
    }

}
