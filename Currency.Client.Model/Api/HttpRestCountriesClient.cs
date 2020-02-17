using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Currency.Client.Model.Api
{
    public class HttpRestCountriesClient : ICountriesApi
    {
        private const string BaseAddress = "http://restcountries.eu";

        private readonly HttpClient _client;

        public HttpRestCountriesClient(HttpClient httpClient)
        {
            _client = httpClient;
            _client.BaseAddress = new Uri(BaseAddress);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> FetchCountryCodeByCurrencyCode(string currencyCode)
        {
            if ("EUR".Equals(currencyCode) || "XDR".Equals(currencyCode)) return "EU";

            var response = await _client.GetAsync($"/rest/v2/currency/{currencyCode}");
            await Task.Run(() => Thread.Sleep(200));
            string json = await response.Content.ReadAsStringAsync();
            var token = JToken.Parse(json);
            return token is JArray
                ? JsonConvert.DeserializeObject<JArray>(json)[0]["alpha2Code"].Value<string>()
                : JsonConvert.DeserializeObject<JObject>(json)["alpha2Code"].Value<string>();
        }
    }
}