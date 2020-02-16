using System;
using System.Threading.Tasks;

namespace Currency.Client.Model.Service
{
    public interface ICountriesApi
    {
        Task<string> FetchCountryCodeByCurrencyCode(string currencyCode);
    }
}