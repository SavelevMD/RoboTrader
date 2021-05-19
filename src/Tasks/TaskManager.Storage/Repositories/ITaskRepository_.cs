using System.Threading;
using System.Threading.Tasks;

using Models.Business.Tasks;

namespace TaskManager.Storage.Repositories
{
    internal interface ITaskRepository_
    {
        Task AddTaskAsync<T>(T item, CancellationToken cancellationToken = default);

        Task UpdateTaskAsync<T>(T item, CancellationToken cancellationToken = default);

        Task DeactivateTaskAsync(string taskName, CancellationToken cancellationToken = default);

        Task SaveTaskToHistoryAsync(TaskShellAbstraction task, CancellationToken cancellationToken = default);
    }
}
