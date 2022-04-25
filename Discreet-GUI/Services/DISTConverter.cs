using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public static class DISTConverter
    {
        public static decimal Divide(ulong amount)
        {
            decimal r = Convert.ToDecimal(amount);
            r = r / 10000000000;
            return r;
        } 
        public static ulong Multiply(decimal amount)
        {
            var r = (ulong)(amount * 10000000000);
            return r;
        }
        public static string ToStringFormat(decimal amount)
        {
            return amount.ToString("0.0000000000");
        }
    }
}
