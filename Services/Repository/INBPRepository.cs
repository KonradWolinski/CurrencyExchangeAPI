using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CurrencyExchangeAPI.Services
{
    public interface INBPRepository
    {
        Task<Models.ExchangeRate> GetExchangeRateAsync(string code);
        public Task AddExchangeRatesAsync(ICollection<Models.ExchangeRate> exchangeRates);
        public Task<Models.ExchangeRate> GetExchangeRateOnDateAsync(DateTime date);
        public Task<ICollection<Models.ExchangeRate>> GetExchangeRateTimeframe(DateTime dateFrom, DateTime dateTo, string code);



    }
}