using System;
using System.Threading.Tasks;
using Currency.Client.Model.Entity;

namespace Currency.Client.Model.Api
{
    public interface INbpApi
    {
        Task<ExchangeRatesTable> FetchRatesTableForDateAsync(DateTime dateTime);
    }
}