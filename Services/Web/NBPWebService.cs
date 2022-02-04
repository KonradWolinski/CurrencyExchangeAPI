using CurrencyExchangeAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CurrencyExchangeAPI.Services
{
    public class NBPWebService : INBPWebService
    {
        private readonly HttpClient _httpClient;
        private const string url = "http://api.nbp.pl/api/exchangerates/";
        public NBPWebService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ICollection<ExchangeRate>> CurrentExchangeRates()
        {
            string[] tables = { "A", "B", "C" };
            List<ExchangeRate> exchangeRates = new();
            foreach (var table in tables)
            {
                HttpRequestMessage request = new(HttpMethod.Get,
                    $"{url}/tables/{table}?format=json");
                var response = await _httpClient.SendAsync(request);
                var responseString = await response.Content.ReadAsStringAsync();
                responseString = responseString.Substring(1, responseString.Length - 2);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var deserializedTableResponse = JsonSerializer.Deserialize<Models.JsonModels.TableResponse>(responseString, options);
                foreach (var rate in deserializedTableResponse.Rates)
                    rate.LastUpdate = deserializedTableResponse.EffectiveDate;
                exchangeRates.AddRange(deserializedTableResponse.Rates);

            }
            return exchangeRates;
            
        }

    }
}
