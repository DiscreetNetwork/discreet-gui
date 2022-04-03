using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Daemon.Models
{
    public class TransactionHistoryOutput
    {
        public string Address { get; set; }
        public ulong Amount { get; set; }
    }
}
