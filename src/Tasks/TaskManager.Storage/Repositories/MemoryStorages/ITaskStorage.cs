
using System.Collections.Generic;
using System.Threading.Tasks;

using Models.Business.Tasks;
using Models.Results;

namespace TaskManager.Storage.Repositories.MemoryStorages
{
    public interface ITaskStorage
    {
        Task<OperationResult> RemoveAsync(TaskShellAbstraction task);

        Task<OperationResult<IEnumerable<TaskShellAbstraction>>> GetTasksByPairNameAsync(string pairName);

        Task<OperationResult<TaskShellAbstraction>> GetTaskAsync(string pairName, string taskName);

        Task<OperationResult<TaskShellAbstraction>> AddOrUpdateAsync(TaskShellAbstraction item);

    }
}
