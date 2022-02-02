using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyExchangeAPI.Services
{
    public class UpdateData
    {
        private readonly INBPRepository _repository;
        private readonly INBPWebService _webService;

        public UpdateData(INBPWebService webService, INBPRepository repository)
        {
            _webService = webService;
            _repository = repository;
        }
        [Hangfire.AutomaticRetry(Attempts = 100)]
        public async Task Update()
        {
            var newRates = await _webService.CurrentExchangeRates();
            await _repository.AddExchangeRatesAsync(newRates);
        }


    }
}
