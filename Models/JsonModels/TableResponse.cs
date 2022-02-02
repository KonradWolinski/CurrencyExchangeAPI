using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyExchangeAPI.Models.JsonModels
{
    public class TableResponse
    {
        [DataType(DataType.Date)]
        public DateTime EffectiveDate { get; set; }
        public List<ExchangeRate> Rates { get; set; }
    }
}
