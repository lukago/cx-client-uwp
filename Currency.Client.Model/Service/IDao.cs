using System;
using System.Threading.Tasks;

namespace Currency.Client.Model.Service
{
    public interface IDao<T>
    {
        Task WriteAsync(T dict);

        Task<T> ReadAsync();
    }
}