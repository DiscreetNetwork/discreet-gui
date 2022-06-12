using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Caches
{
    /// <summary>
    /// A cache to store all logs / output from the daemon
    /// </summary>
    public class DaemonLogCache
    {
        public event Action LogsModified;
        private List<string> _logs = new List<string>();
        public List<string> Logs { get => _logs; }

        public void Add(string log)
        {
            _logs.Add(log);
            LogsModified?.Invoke();
        } 
    }
}
