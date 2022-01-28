using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CurrencyExchangeAPI.Services
{
    public class NBPRepository
    {
        private readonly HttpClient _httpClient;
        private readonly ExchangeRatesContext _exchangeRatesContext;
        public NBPRepository(HttpClient httpClient, ExchangeRatesContext exchangeRatesContext)
        {
            _httpClient = httpClient;
            _exchangeRatesContext = exchangeRatesContext;
        }

        public float GetExchangeRate(string code)
        {
            return _exchangeRatesContext.ExchangeRates
                .Where(val => val.Code == code)
                .FirstOrDefault().Rate;
        }
        public void UpdateExchangeRates()
        {
            throw new NotImplementedException();
        }

    }
}
