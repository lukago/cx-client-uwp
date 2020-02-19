using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Currency.Client.Model.Entity;
using Newtonsoft.Json;

namespace Currency.Client.Model.Api
{
    public class HttpNbpClient : INbpApi, IDisposable
    {
        private const string BaseAddress = "http://api.nbp.pl";

        private readonly HttpClient _client;

        public HttpNbpClient(HttpClient httpClient)
        {
            _client = httpClient;
            _client.BaseAddress = new Uri(BaseAddress);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        public async Task<ExchangeRatesTable> FetchRatesTableForDateAsync(DateTime dateTime)
        {
            HttpResponseMessage response = await _client.GetAsync($"/api/exchangerates/tables/a/{dateTime:yyyy-MM-dd}");
            if (!response.IsSuccessStatusCode) return new ExchangeRatesTable("", "", DateTime.Now, new List<Rate>());
            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ExchangeRatesTable[]>(json)[0];
        }
    }
}