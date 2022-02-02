using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CurrencyExchangeAPI.Models
{
    public class ExchangeRate
    {
        //table a, b use Mid, table C uses bidding & asking price
        private float _rate;
        public float Mid 
        { 
            get { return _rate; }
            set { _rate = value; }
        }
        public float Bid
        {
            get { return _rate; }
            set { _rate = value; }
        }
        public int ID { get; set; }
        [JsonPropertyName("Currency")]
        public string CurrencyName { get; set; }
        public string Code { get; set; }
        [DataType(DataType.Date)]
        public DateTime LastUpdate { get; set; }
    }
}
