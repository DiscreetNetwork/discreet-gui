using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Daemon.Common
{
    public class DaemonErrorResponse
    {
        public int ErrID { get; set; }
        public string ErrMsg { get; set; }
        public object Result { get; set; }
    }
}
