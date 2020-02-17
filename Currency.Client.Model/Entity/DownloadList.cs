using System.Collections.Generic;
using System.Threading.Tasks;

namespace Currency.Client.Model.Entity
{
    public class DownloadList<T>
    {
        private readonly List<Task<T>> _downloadList;

        public DownloadList(List<Task<T>> downloadList)
        {
            _downloadList = downloadList;
        }

        public async Task<T> GetNextFinishedAsync()
        {
            Task<T> finishedTask = await Task.WhenAny(_downloadList);
            _downloadList.Remove(finishedTask);
            return await finishedTask;
        }

        public bool IsFinished()
        {
            return _downloadList.Count == 0;
        }
    }
}