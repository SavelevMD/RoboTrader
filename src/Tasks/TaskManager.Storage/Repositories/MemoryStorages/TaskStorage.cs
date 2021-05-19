using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

using Models.Business.Tasks;
using Models.Results;

using Newtonsoft.Json;

namespace TaskManager.Storage.Repositories.MemoryStorages
{
    public class TaskStorage : ITaskStorage
    {
        private const string _prefixTasks = "tasks";
        private readonly ITaskNamesCollectionStorage _taskNamesCollectionStorage;
        private readonly IDistributedCache _distributedCache;
        private readonly ILogger<TaskStorage> _logger;

        public TaskStorage(ITaskNamesCollectionStorage taskNamesCollectionStorage, IDistributedCache distributedCache, ILogger<TaskStorage> logger)
        {
            _taskNamesCollectionStorage = taskNamesCollectionStorage;
            _distributedCache = distributedCache;
            _logger = logger;
        }

        public async Task<OperationResult<TaskShellAbstraction>> AddOrUpdateAsync(TaskShellAbstraction item)
        {
            //TODO: завести валидацию
            if (item == default)
            {
                return OperationResult<TaskShellAbstraction>.Failure($"Входное значение {item} не может быть null");
            }

            try
            {
                var result = await _taskNamesCollectionStorage.AddOrUpdateTaskAsync(item);
                if (result.IsSuccess)
                {
                    
                }
                var serializedTasks = await _distributedCache.GetStringAsync($"{_prefixTasks}:{item.CurrencyPairName}");

                if (string.IsNullOrWhiteSpace(serializedTasks))
                {
                    await _distributedCache.SetStringAsync($"{_prefixTasks}:{item.CurrencyPairName}", JsonConvert.SerializeObject(new Collection<TaskShellAbstraction> { item }));
                }
                else
                {
                    var tasks = JsonConvert.DeserializeObject<ICollection<TaskShellAbstraction>>(serializedTasks);
                    if (tasks.Any(r => r.TaskName == item.TaskName))
                    {
                        var task = tasks.Single(r => r.TaskName == item.TaskName);
                        tasks.Remove(task);
                    }
                    tasks.Add(item);

                    await _distributedCache.SetStringAsync($"{_prefixTasks}:{item.CurrencyPairName}", JsonConvert.SerializeObject(tasks));
                }
            }
            catch (Exception ex)
            {
                OperationResult<TaskShellAbstraction>.Failure($"Не удалось добавить или обновить {item} в коллекцию tasks", ex);
            }
            return OperationResult<TaskShellAbstraction>.Success(item);
        }

        public async Task<OperationResult<TaskShellAbstraction>> GetTaskAsync(string pairName, string taskName)
        {
            if (string.IsNullOrWhiteSpace(pairName))
            {
                return OperationResult<TaskShellAbstraction>.Failure($"Входное значение {pairName} не может быть null или пустым");
            }

            if (string.IsNullOrWhiteSpace(taskName))
            {
                return OperationResult<TaskShellAbstraction>.Failure($"Входное значение {taskName} не может быть null или пустым");
            }

            var serializedTasks = await _distributedCache.GetStringAsync($"{_prefixTasks}:{pairName}");

            if (string.IsNullOrWhiteSpace(serializedTasks))
            {
                return OperationResult<TaskShellAbstraction>.Failure($"Task с именем {taskName} не найден");
            }

            var tasks = JsonConvert.DeserializeObject<ICollection<TaskShellAbstraction>>(serializedTasks);
            if (tasks.Any(r => r.TaskName == taskName))
            {
                return OperationResult<TaskShellAbstraction>.Success(tasks.Single(r => r.TaskName == taskName));
            }
            else
            {
                return OperationResult<TaskShellAbstraction>.Failure($"Task с именем {taskName} не найден");
            }
        }

        public async Task<OperationResult<IEnumerable<TaskShellAbstraction>>> GetTasksByPairNameAsync(string pairName)
        {
            if (string.IsNullOrWhiteSpace(pairName))
            {
                return OperationResult<IEnumerable<TaskShellAbstraction>>.Failure($"Входное значение {pairName} не может быть null или пустым");
            }

            var serializedTasks = await _distributedCache.GetStringAsync($"{_prefixTasks}:{pairName}");

            return string.IsNullOrWhiteSpace(serializedTasks)
                ? OperationResult<IEnumerable<TaskShellAbstraction>>.Failure($"Коллекция пуста")
                : (OperationResult<IEnumerable<TaskShellAbstraction>>)JsonConvert.DeserializeObject<ICollection<TaskShellAbstraction>>(serializedTasks);
        }

        public async Task<OperationResult> RemoveAsync(TaskShellAbstraction task)
        {
            if (task == default)
            {
                return OperationResult.Failure($"Входное значение {task} не может быть null");
            }

            try
            {
                var serializedTasks = await _distributedCache.GetStringAsync($"{_prefixTasks}:{task.CurrencyPairName}");

                if (!string.IsNullOrWhiteSpace(serializedTasks))
                {
                    var tasks = JsonConvert.DeserializeObject<ICollection<TaskShellAbstraction>>(serializedTasks);
                    if (tasks.Any(r => r.TaskName == task.TaskName))
                    {
                        tasks.Remove(tasks.Single(r => r.TaskName == task.TaskName));
                        await _distributedCache.SetStringAsync($"{_prefixTasks}:{task.CurrencyPairName}", JsonConvert.SerializeObject(tasks));
                    }
                }
            }
            catch (Exception ex)
            {
                OperationResult.Failure($"Не удалось удалить {task} в коллекцию tasks", ex);
            }
            return OperationResult.Success();
        }
    }
}
