using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Models.Business;
using Models.Results;

using Robo.Database.Context;
using Robo.Database.Models;

namespace TaskManager.Storage.Repositories
{
    public class TaskHistoryRepository
    {
        private readonly TickerContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<TaskHistoryRepository> _logger;

        public TaskHistoryRepository(TickerContext tickerContext, IMapper mapper, ILogger<TaskHistoryRepository> logger)
        {
            _context = tickerContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OperationResult<IEnumerable<TasksHistoryModel>>> GetHistoryForTaskAsync(string taskName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(taskName))
            {
                return OperationResult<IEnumerable<TasksHistoryModel>>.Failure($"{nameof(taskName)}");
            }

            try
            {
                var history = await _context.TasksHistories
                    .Where(r => r.TaskName == taskName)
                    .ToListAsync(cancellationToken);
                return OperationResult<IEnumerable<TasksHistoryModel>>.Success(_mapper.Map<List<TasksHistories>, IEnumerable<TasksHistoryModel>>(history));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"В процессе выполнения запроса произошла ошибка {ex.Message}");
                return OperationResult<IEnumerable<TasksHistoryModel>>.Failure($"В процессе выполнения запроса произошла ошибка", ex);
            }
        }

        public async Task<OperationResult> AddTaskHistoryAsync(TasksHistoryModel record, CancellationToken cancellationToken = default)
        {
            try
            {
                if (await _context.Tasks.AnyAsync(r => r.TaskName == record.TaskName && r.IsActive, cancellationToken))
                {
                    record.TaskId = (await _context.Tasks.SingleAsync(r => r.TaskName == record.TaskName, cancellationToken)).Id;
                    await _context.TasksHistories.AddAsync(_mapper.Map<TasksHistories>(record), cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);
                    return OperationResult.Success();
                }
                else
                {
                    _logger.LogError($"При выполнении операции сохранения TaskHistory, произошла ошибка, не был найден Task по имени {record.TaskName}");
                    return OperationResult.Failure($"При выполнении операции сохранения TaskHistory, произошла ошибка, не был найден Task по имени {record.TaskName}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "При добавлении записи в TaskHistory, произошла ошибка");
                return OperationResult.Failure($"При добавлении записи в TaskHistory, произошла ошибка", ex);
            }
        }
    }
}
