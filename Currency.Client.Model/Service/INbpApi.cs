using System;
using System.Threading.Tasks;

namespace Currency.Client.Model.Service
{
    public interface INbpApi
    {
        Task<ExchangeRatesTable> FetchRatesTableForDateAsync(DateTime dateTime);
    }
}