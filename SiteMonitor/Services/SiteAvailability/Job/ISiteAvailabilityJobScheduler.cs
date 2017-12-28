using SiteMonitor.Models;
using System.Threading.Tasks;

namespace SiteMonitor.Services.SiteAvailability
{
    public interface ISiteAvailabilityJobScheduler
    {
        void CreateOrUpdateTask(Site site);
        void DeleteTask(Site site);
    }
}