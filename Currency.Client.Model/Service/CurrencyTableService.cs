using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Currency.Client.Model.Api;
using Currency.Client.Model.Entity;

namespace Currency.Client.Model.Service
{
    public class CurrencyTableService : ICurrencyTableService
    {
        private readonly ICountriesApi _countriesApi;
        private readonly INbpApi _nbpApi;

        public CurrencyTableService(INbpApi nbpApi, ICountriesApi countriesApi)
        {
            _nbpApi = nbpApi;
            _countriesApi = countriesApi;
        }

        public async Task<List<Rate>> FetchRatesForDateAsync(DateTime dateTime)
        {
            var table = await _nbpApi.FetchRatesTableForDateAsync(dateTime);
            return table.Rates;
        }

        public DownloadList<Rate> FetchFlagsForRates(IEnumerable<Rate> rates)
        {
            List<Task<Rate>> downloadList = rates.Select(FetchFlagUriForRateAsync).ToList();
            return new DownloadList<Rate>(downloadList);
        }

        private async Task<Rate> FetchFlagUriForRateAsync(Rate rate)
        {
            string countryCode = await _countriesApi.FetchCountryCodeByCurrencyCode(rate.Code);
            var flagUri = new Uri($"https://www.countryflags.io/{countryCode}/flat/64.png");
            return new Rate(rate.Code, rate.Currency, rate.Mid, flagUri);
        }
    }
}