using System.Collections.Generic;
using System.Threading.Tasks;

namespace CurrencyExchangeAPI.Services
{
    public interface INBPWebService
    {
        Task<ICollection<Models.ExchangeRate>> CurrentExchangeRates();
    }
}