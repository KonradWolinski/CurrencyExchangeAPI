using CsvHelper;
using CurrencyExchangeAPI.Authentication;
using CurrencyExchangeAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyExchangeAPI.Controllers.v1
{
    //[BasicAuthorization]
    [Route("api/v1")]
    public class CurrencyExchangeController : Controller
    {
        private readonly INBPRepository _repository;
        private readonly INBPWebService _webService;

        public CurrencyExchangeController(INBPWebService webService, INBPRepository repository)
        {
            _webService = webService;
            _repository = repository;
        }
        [HttpGet("{code}")]
        public async Task<IActionResult> Index(string code)
        {
            var rate = await _repository.GetExchangeRateAsync(code.ToUpper());
            if (rate != null)
                return Ok(rate);
            return NotFound();
        }
        [HttpGet("date/{date}")]
        public async Task<IActionResult> Date(DateTime date)
        {
            var rate = (List<Models.ExchangeRate>)await _repository.GetExchangeRateOnDateAsync(date);
            if (rate != null && rate.Count > 0 && rate[0].LastUpdate == date)
                return Ok(rate);
            return NotFound();
        }
        [HttpGet("convert")]
        public async Task<IActionResult> Convert(string codeFrom, string codeTo, float value)
        {
            if (value < 0.0f)
                return NotFound();
            codeFrom = codeFrom.ToUpper();
            codeTo = codeTo.ToUpper();

            var rateFrom = 1.0f;
            var rateTo = 1.0f;
            if (codeFrom != "PLN")
            {
                var getRateFrom = await _repository.GetExchangeRateAsync(codeFrom);
                if (getRateFrom == null)
                    return NotFound();
                rateFrom = getRateFrom.Mid;
            }
            if (codeTo != "PLN")
            {
                var getRateTo = await _repository.GetExchangeRateAsync(codeTo);
                if (getRateTo == null)
                    return NotFound();
                rateTo = getRateTo.Mid;
            }
            var conversionRate = rateFrom / rateTo;
            return Ok(value * conversionRate);
        }
        [HttpGet("fromPLN")]
        public async Task<IActionResult> ConvertFromPLN(string codeTo, float value) => await Convert("PLN", codeTo, value);
        [HttpGet("toPLN")]
        public async Task<IActionResult> ConvertToPLN(string codeFrom, float value) => await Convert(codeFrom, "PLN", value);
        [HttpGet("historicalData")]
        public async Task<IActionResult> GetHistoricalDataCSV(DateTime dateFrom, DateTime dateTo, string code)
        {
            var records = await _repository.GetExchangeRateTimeframe(dateFrom, dateTo, code.ToUpper());
            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream))
                using (var csvWriter = new CsvWriter(streamWriter, System.Globalization.CultureInfo.InvariantCulture))
                {
                    csvWriter.WriteRecords(records);
                }

                return Ok(memoryStream.ToArray());
            }
        }

    }
}
