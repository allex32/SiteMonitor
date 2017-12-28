
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ObjectDumper;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace SiteMonitor.Infrastructure.HostPinger
{
    public class ServerOnlineChecker : IServerOnlineChecker
    {
        IConfiguration _configuration;
        ILogger _logger;

        private static readonly int _defaultTimeout = 5000;
        private readonly int _settingTimeout;
        public ServerOnlineChecker(IConfiguration configuration, ILogger<ServerOnlineChecker> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _settingTimeout =  _configuration.GetSection("ServerOnlineChecker").GetValue("Timeout", _defaultTimeout);
        }

        
        public async Task<bool> CheckServerOnlineAsync(string nameOrAdress)
        {
            HttpWebRequest request = WebRequest.Create(nameOrAdress) as HttpWebRequest;
           
            request.Timeout = _settingTimeout;
            request.Method = WebRequestMethods.Http.Head;
            try
            {
                using (var response = await request.GetResponseAsync())
                {
                    return true;
                }
            }
            catch (WebException ex)
            {
                _logger.LogError(ex, nameOrAdress);
            }

            return false;
        }
    }
}
