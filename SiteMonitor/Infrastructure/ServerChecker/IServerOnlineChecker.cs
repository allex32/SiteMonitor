using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteMonitor.Infrastructure.HostPinger
{

    public interface IServerOnlineChecker
    {
        Task<bool> CheckServerOnlineAsync(string nameOrAdress);
    }
}
