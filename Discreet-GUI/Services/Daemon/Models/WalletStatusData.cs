using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Daemon.Models
{
    public enum WalletStatus : int
    {
        UNLOCKED = 0,
        LOCKED = 1,
        UNLOADED = 2,
    }

    public class WalletStatusData
    {
        public WalletStatus Status { get; set; }
        public string Label { get; set; }
    }

    public class WalletStatusRV
    {
        public int Status { get; set; }
        public string Label { get; set; }
    }
}
