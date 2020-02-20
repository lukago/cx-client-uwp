using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Currency.Client.Model.Entity;

namespace Currency.Client.Model.Api
{
    public interface INbpApi
    {
        Task<ExchangeRatesTable> FetchRatesTableForDateAsync(DateTime dateTime);

        Task<List<Rate>> FetchRatesTableForCodeBetweenDatesAsync(
            string code,
            DateTime startTime,
            DateTime endTime);
    }
}