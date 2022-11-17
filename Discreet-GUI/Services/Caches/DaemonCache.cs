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


        public event Action SyncPercentageChanged;
        private float _syncPercentage = 1;
        public float SyncPercentage { get => _syncPercentage; set { _syncPercentage = value; SyncPercentageChanged?.Invoke(); } }


        public event Action SyncFromChanged;
        private int _syncFrom = 0;
        public int SyncFrom { get => _syncFrom; set { _syncFrom = value; SyncFromChanged?.Invoke(); } }


        public event Action SyncToChanged;
        private int _syncTo = 0;
        public int SyncTo { get => _syncTo; set { _syncTo = value; SyncToChanged?.Invoke(); } }
    }
}
