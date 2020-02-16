using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Currency.Client.Model.Service
{
    public interface ICurrencyTableService
    {
        Task<List<Rate>> FetchRatesForDateWithFlagsAsync(DateTime dateTime);
    }
}