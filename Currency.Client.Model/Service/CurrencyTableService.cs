using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Currency.Client.Model.Service
{
    public class CurrencyTableService : ICurrencyTableService
    {
        private readonly INbpApi _nbpApi;
        private readonly ICountriesApi _countriesApi;

        public CurrencyTableService(INbpApi nbpApi, ICountriesApi countriesApi)
        {
            _nbpApi = nbpApi;
            _countriesApi = countriesApi;
        }

        public async Task<List<Rate>> FetchRatesForDateWithFlagsAsync(DateTime dateTime)
        {
            ExchangeRatesTable table = await _nbpApi.FetchRatesTableForDateAsync(dateTime);
            var downloadTasks = table.Rates.Select(FetchFlagUriForRateAsync).ToList();
            var rates = new List<Rate>();
            
            while (downloadTasks.Count > 0)
            {
                var finishedTask = await Task.WhenAny(downloadTasks);
                downloadTasks.Remove(finishedTask);
                rates.Add(await finishedTask);
            }

            return rates;
        }

        private async Task<Rate> FetchFlagUriForRateAsync(Rate rate)
        {
            string countryCode = await _countriesApi.FetchCountryCodeByCurrencyCode(rate.Code);
            Uri flagUri =  new Uri($"https://www.countryflags.io/{countryCode}/flat/64.png");
            return new Rate(rate.Code, rate.Currency, rate.Mid, flagUri);
        }
    }
}