using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Currency.Client.Model.Entity;

namespace Currency.Client.Model.Service
{
    public interface ICurrencyTableService
    {
        Task<List<Rate>> FetchRatesForDateAsync(DateTime dateTime);

        DownloadList<Rate> FetchFlagsForRates(IEnumerable<Rate> rates);
    }
}