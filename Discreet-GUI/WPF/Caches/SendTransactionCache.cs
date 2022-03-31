using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.Caches
{
    public class SendTransactionCache
    {
        public string Receiver { get; set; }
        public double Amount { get; set; }
        public string Sender { get; set; }
    }
}
