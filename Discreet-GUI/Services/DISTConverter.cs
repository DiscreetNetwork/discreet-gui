using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public static class DISTConverter
    {
        public static decimal? Divide(ulong amount)
        {
            try
            {
                decimal r = Convert.ToDecimal(amount);
                r = r / 10000000000;
                return r;
            }
            catch (Exception)
            {
                return null;
            }
        } 
        public static ulong? Multiply(decimal amount)
        {
            try
            {
                var r = (ulong)(amount * 10000000000);
                return r;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static string ToStringFormat(decimal amount)
        {
            return amount.ToString("0.0000000000");
        }
    }
}
