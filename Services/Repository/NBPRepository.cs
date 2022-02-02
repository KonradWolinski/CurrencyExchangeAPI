using CurrencyExchangeAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CurrencyExchangeAPI.Services
{
    public class NBPRepository : INBPRepository
    {
        private readonly ExchangeRateContext _exchangeRatesContext;
        public NBPRepository(ExchangeRateContext exchangeRatesContext)
        {
            _exchangeRatesContext = exchangeRatesContext;
        }

        public async Task<Models.ExchangeRate> GetExchangeRateAsync(string code)
        {
            var rate = await _exchangeRatesContext.ExchangeRates
                .Where(val => val.Code == code)
                .FirstOrDefaultAsync();
            return rate;
        }

        public async Task AddExchangeRatesAsync(ICollection<Models.ExchangeRate> exchangeRates)
        {
            await _exchangeRatesContext.AddRangeAsync(exchangeRates);
            await _exchangeRatesContext.SaveChangesAsync();
        }
        public async Task<Models.ExchangeRate> GetExchangeRateOnDateAsync(DateTime date)
        {
            if (date.CompareTo(new DateTime(2002, 01, 2)) < 0)
                return null;

            var rate = await _exchangeRatesContext.ExchangeRates
                .Where(val => val.LastUpdate.Date == date.Date)
                .FirstOrDefaultAsync();
            return rate;
        }
        public async Task<ICollection<Models.ExchangeRate>> GetExchangeRateTimeframe(DateTime dateFrom, DateTime dateTo, string code)
        {
            var rates = await _exchangeRatesContext.ExchangeRates
                .Where(val => val.Code == code)
                .Where(val => val.LastUpdate.Date.CompareTo(dateFrom.Date) >= 0 && val.LastUpdate.Date.CompareTo(dateTo.Date) <= 0)
                .ToListAsync();
            return rates;
        }
    }
}
