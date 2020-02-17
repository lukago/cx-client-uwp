using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Currency.Client.Model.Storage
{
    public class JsonDao<T> : IDao<T>
    {
        private readonly Func<Task<string>> _streamReader;
        private readonly Func<string, Task> _streamWriter;

        public JsonDao(Func<string, Task> streamWriter, Func<Task<string>> streamReader)
        {
            _streamWriter = streamWriter;
            _streamReader = streamReader;
        }

        public Task WriteAsync(T dict)
        {
            string json = JsonConvert.SerializeObject(dict);
            return _streamWriter.Invoke(json);
        }

        public async Task<T> ReadAsync()
        {
            string json = await _streamReader.Invoke();
            return string.IsNullOrEmpty(json) ? default : JsonConvert.DeserializeObject<T>(json);
        }
    }
}