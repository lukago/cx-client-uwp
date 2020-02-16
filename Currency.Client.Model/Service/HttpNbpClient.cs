using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Currency.Client.Model.Service
{
    public class HttpNbpClient : INbpApi, IDisposable
    {
        private readonly HttpClient _client;

        public HttpNbpClient(HttpClient httpClient)
        {
            _client = httpClient;
            _client.BaseAddress = new Uri("http://api.nbp.pl");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        
        public async Task<ExchangeRatesTable> FetchRatesTableForDateAsync(DateTime dateTime)
        {
            HttpResponseMessage response = await _client.GetAsync($"/api/exchangerates/tables/a/{dateTime:yyyy-MM-dd}");
            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ExchangeRatesTable[]>(json)[0];
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}