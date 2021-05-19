using System.Threading.Tasks;

using Models.Business.Tasks;
using Models.Results;

namespace TaskManager.Storage.Repositories.MemoryStorages
{
    public interface ITaskNamesCollectionStorage
    {
        Task<OperationResult> AddOrUpdateTaskAsync(TaskShellAbstraction task);
        Task<OperationResult<TaskShellAbstraction>> GetTaskAsync(string taskName);
        Task<OperationResult<TaskShellAbstraction>> RemoveTaskAsync(string taskName);
    }
}