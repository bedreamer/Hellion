using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Hellion.Core.Network
{
    public static class HostResolver
    {
        /// <summary>
        /// Resolves a hostname to an IP-Address.
        /// If an IP-Address is given the IP-Address will be returned.
        /// </summary>
        public static string ResolveToIp(string hostOrIp)
        {
            return Dns.GetHostAddressesAsync(hostOrIp).Result.First().ToString();
        }
    }
}
