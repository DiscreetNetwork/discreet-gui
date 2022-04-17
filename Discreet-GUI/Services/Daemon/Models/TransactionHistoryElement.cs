using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Daemon.Models
{
    public class TransactionHistoryElement
    {
        public string TxID { get; set; }
        public long Timestamp { get; set; }

        public ulong SentAmount { get; set; }
        public ulong ReceivedAmount { get; set; }

        public List<TransactionHistoryOutput> Inputs { get; set; }
        public List<TransactionHistoryOutput> Outputs { get; set; }
    }
}
