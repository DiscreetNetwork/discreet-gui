using Services.Caches;
using Services.ZMQ.Handlers.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ZMQ.Handlers
{
    public class DaemonSyncHandler : MessageHandler
    {
        private readonly DaemonCache _daemonCache;

        public DaemonSyncHandler(DaemonCache daemonCache)
        {
            _daemonCache = daemonCache;
        }

        public override Task Handle(byte[] bytes)
        {
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes, 0, 8);
            }
            int to = BitConverter.ToInt32(bytes, 0);
            int from = BitConverter.ToInt32(bytes, 4);
            float percentage = BitConverter.ToSingle(bytes, 8);

            _daemonCache.SyncTo = to;
            _daemonCache.SyncFrom = from;
            _daemonCache.SyncPercentage = percentage;

            return Task.CompletedTask;
        }
    }
}
