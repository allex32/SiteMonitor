using Hangfire.Dashboard;
using Hangfire.Annotations;
using SiteMonitor.Data;

namespace SiteMonitor.Infrastructure.Hangfire
{
    public class AllowAdminDashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {       
            var httpContext = ((AspNetCoreDashboardContext)context).HttpContext;
        
            return httpContext.User.IsInRole(PolicyNames.AdminRole);
        }
    }
}
