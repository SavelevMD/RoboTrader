using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Models.Results;

using Robo.Database.Context;
using Robo.Database.Models;

namespace TaskManager.Storage.Repositories
{
    public class TaskListnerRepository
    {
        private readonly TickerContext _context;
        private readonly ILogger<TaskListnerRepository> _logger;

        public TaskListnerRepository(TickerContext context, ILogger<TaskListnerRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<OperationResult> AddTaskListnerStateAsync(string taskName, DateTime receiptCandleTime, string taskBody, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(taskName) || string.IsNullOrWhiteSpace(taskBody))
            {
                return OperationResult.Failure($"Не корректные входные параметры {nameof(taskName)} или {taskBody}");
            }

            var tsk = await _context.Tasks.SingleOrDefaultAsync(r => r.TaskName == taskName);
            if (tsk != null)
            {
                try
                {
                    await _context.TasksListnerStates.AddAsync(new TasksListnerStates
                    {
                        TaskId = tsk.Id,
                        ReceiptTime = receiptCandleTime,
                        State = taskBody
                    }, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);
                    return OperationResult.Success();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"При добавлении записи в таблицу TaskListnerState, произошла ошибка");
                    return OperationResult.Failure($"При добавлении записи в таблицу TaskListnerState, произошла ошибка", ex);
                }
            }
            else
            {
                return OperationResult.Failure($"При добавлении записи в таблицу TaskListnerState, произошла ошибка, таск по имени { taskName} не найден");
            }
        }
    }
}
