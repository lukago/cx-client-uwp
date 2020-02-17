using System.Threading.Tasks;

namespace Currency.Client.Model.Api
{
    public interface ICountriesApi
    {
        Task<string> FetchCountryCodeByCurrencyCode(string currencyCode);
    }
}