using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Currency.Client.Model.Entity;

namespace Currency.Client.Model.Service
{
    public interface ICurrencyTableService
    {
        Task<List<Rate>> FetchRatesForDateAsync(DateTime dateTime);

        Task<List<Rate>> FetchRatesForCodeAsync(Rate rate, DateTime startTime, DateTime endTime, Action<double> progressAction);

        DownloadList<Rate> FetchFlagsForRates(IEnumerable<Rate> rates);
    }
}