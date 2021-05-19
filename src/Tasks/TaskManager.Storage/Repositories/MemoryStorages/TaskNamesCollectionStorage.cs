using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Caching.Distributed;

using Models.Business.Tasks;
using Models.Results;

using Newtonsoft.Json;

namespace TaskManager.Storage.Repositories.MemoryStorages
{
    internal class TaskNamesCollectionStorage : ITaskNamesCollectionStorage
    {
        private const string _prefixTasks = "tasksCollection";

        private readonly IDistributedCache _distributedCache;

        public TaskNamesCollectionStorage(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<OperationResult> AddOrUpdateTaskAsync(TaskShellAbstraction task)
        {
            if (string.IsNullOrWhiteSpace(task.TaskName))
            {
                return OperationResult.Failure($"Входной параметр {task.TaskName} не может быть null или пустым");
            }

            await _distributedCache.SetStringAsync($"{_prefixTasks}:{task.TaskName}", JsonConvert.SerializeObject(task));
            return OperationResult.Success();
        }

        public async Task<OperationResult<TaskShellAbstraction>> GetTaskAsync(string taskName)
        {
            if (string.IsNullOrWhiteSpace(taskName))
            {
                return OperationResult<TaskShellAbstraction>.Failure($"Входной параметр {taskName} не может быть null или пустым");
            }

            try
            {
                var serializedTask = await _distributedCache.GetStringAsync($"{_prefixTasks}:{taskName}");
                var task = JsonConvert.DeserializeObject<TaskShellAbstraction>(serializedTask);

                return OperationResult<TaskShellAbstraction>.Success(task);
            }
            catch (Exception ex)
            {
                return OperationResult<TaskShellAbstraction>.Failure($"В процессе получения таска по имени {taskName}, произошла ошибка", ex);
            }
        }

        public async Task<OperationResult<TaskShellAbstraction>> RemoveTaskAsync(string taskName)
        {
            if (string.IsNullOrWhiteSpace(taskName))
            {
                return OperationResult<TaskShellAbstraction>.Failure($"Входной параметр {taskName} не может быть null или пустым");
            }

            try
            {
                var serializedTask = await _distributedCache.GetStringAsync($"{_prefixTasks}:{taskName}");
                var task = JsonConvert.DeserializeObject<TaskShellAbstraction>(serializedTask);

                await _distributedCache.RemoveAsync($"{_prefixTasks}:{taskName}");

                return OperationResult<TaskShellAbstraction>.Success(task);
            }
            catch (Exception ex)
            {
                return OperationResult<TaskShellAbstraction>.Failure($"В процессе удаления таска по имени {taskName}, произошла ошибка", ex);
            }
        }
    }
}
