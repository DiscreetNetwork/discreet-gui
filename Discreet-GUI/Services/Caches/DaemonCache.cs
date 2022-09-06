using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Caches
{
    public class DaemonCache
    {
        public event Action DaemonStartedChanged;
        private bool _daemonStarted = false;
        public bool DaemonStarted { get => _daemonStarted; set { _daemonStarted = value; DaemonStartedChanged?.Invoke(); } }
    }
}
