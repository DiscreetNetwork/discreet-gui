using Services.Caches;
using Services.ZMQ.Handlers.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ZMQ.Handlers
{
    internal class DaemonStateChangedHandler : MessageHandler
    {
        private readonly DaemonCache _daemonCache;

        public DaemonStateChangedHandler(DaemonCache daemonCache)
        {
            _daemonCache = daemonCache;
        }

        public override Task Handle(byte[] bytes)
        {
            string message = Encoding.UTF8.GetString(bytes);
            if (!message.Equals("ready")) return Task.CompletedTask;
            _daemonCache.DaemonStarted = true;

            return Task.CompletedTask;
        }
    }
}
