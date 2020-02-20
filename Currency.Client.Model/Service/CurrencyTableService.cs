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

        public async Task<List<Rate>> FetchRatesForCodeAsync(Rate rate, DateTime startTime, 
            DateTime endTime,
            Action<double> progressAction)
        {
            DateTime st;
            var rates = new List<Rate>();
            var diff = endTime - startTime;
            double progress = 1.0 / (diff.Days / 365.0);
            double progressBar = 0;
            for (st = startTime; endTime - st > TimeSpan.FromDays(365); st = st.AddDays(365))
            {
                List<Rate> newRates = await _nbpApi.FetchRatesTableForCodeBetweenDatesAsync(rate.Code, st, st.AddDays(365));
                await Task.Run(() =>
                {
                    List<Rate> ratesReduced = new List<Rate>();
                    for (int i = 0; i < newRates.Count; i+=5)
                    {
                        ratesReduced.Add(newRates[i]);
                    }
                    rates.AddRange(ratesReduced);
                });
                progressBar += progress;
                progressAction.Invoke(progressBar);
            }
            
            List<Rate> lastRates = await _nbpApi.FetchRatesTableForCodeBetweenDatesAsync(rate.Code, st, endTime);
            await Task.Run(() =>
            {
                if (diff > TimeSpan.FromDays(100))
                {
                    List<Rate> ratesReducedLast = new List<Rate>();
                    for (int i = 0; i < lastRates.Count; i += 5)
                    {
                        ratesReducedLast.Add(lastRates[i]);
                    }

                    rates.AddRange(ratesReducedLast);
                }
                else
                {
                    xrates.AddRange(lastRates);
                }
            });
            progressBar += progress;
            progressAction.Invoke(progressBar);

            return rates;
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
            return new Rate(rate.Code, rate.Currency, rate.Mid, flagUri, rate.EffectiveDate);
        }
    }
}