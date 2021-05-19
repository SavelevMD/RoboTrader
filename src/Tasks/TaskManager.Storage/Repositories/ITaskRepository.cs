using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Models.Business;
using Models.Results;
using Models.Tasks;

namespace TaskManager.Storage.Repositories
{
    public interface ITaskRepository
    {
        Task<OperationResult> AddTaskAsync(TaskWrapper task, CancellationToken cancellationToken = default);

        Task<OperationResult<IEnumerable<TaskModel>>> GetActiveTasksAsync(CancellationToken cancellationToken = default);

        Task<OperationResult> DeactivateTaskByNameAsync(string tskName, CancellationToken cancellationToken = default);

        Task<OperationResult<IEnumerable<(string pairName, int frame)>>> GetPairNamesAndFramesForActiveTasksAsync(CancellationToken cancellationToken = default);

        Task<OperationResult> UpdateTaskAsync(TaskWrapper task, CancellationToken cancellationToken = default);
    }
}
