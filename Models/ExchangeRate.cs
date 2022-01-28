using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyExchangeAPI.Models
{
    public class ExchangeRate
    {
        public string CurrencyName { get; set; }
        public string Code { get; set; }
        public float Rate { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
