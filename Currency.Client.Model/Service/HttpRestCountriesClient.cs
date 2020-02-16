using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Currency.Client.Model.Service
{
    public class HttpRestCountriesClient : ICountriesApi
    {
        private readonly HttpClient _client;

        public HttpRestCountriesClient(HttpClient httpClient)
        {
            _client = httpClient;
            _client.BaseAddress = new Uri("http://restcountries.eu");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        
        public async Task<string> FetchCountryCodeByCurrencyCode(string currencyCode)
        {
            if ("EUR".Equals(currencyCode)) return "EU";
            
            HttpResponseMessage response = await _client.GetAsync($"/rest/v2/currency/{currencyCode}");
            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<JArray>(json)[0]["alpha3Code"].Value<string>();
        }
    }
}