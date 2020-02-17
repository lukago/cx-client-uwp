using System.Threading.Tasks;

namespace Currency.Client.Model.Storage
{
    public interface IDao<T>
    {
        Task WriteAsync(T dict);

        Task<T> ReadAsync();
    }
}