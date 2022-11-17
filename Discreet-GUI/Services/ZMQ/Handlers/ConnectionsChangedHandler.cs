using Services.Caches;
using Services.Daemon.Status;
using Services.ZMQ.Handlers.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ZMQ.Handlers
{
    public class ConnectionsChangedHandler : MessageHandler
    {
        private readonly DaemonStatusService _daemonStatusService;
        private readonly WalletCache _walletCache;

        public ConnectionsChangedHandler(DaemonStatusService daemonStatusService, WalletCache walletCache)
        {
            _daemonStatusService = daemonStatusService;
            _walletCache = walletCache;
        }

        public override Task Handle(byte[] bytes)
        {
            var numConnections = BitConverter.ToInt32(bytes);
            _walletCache.NumberOfConnections = numConnections;
            return Task.CompletedTask;
        }
    }
}
