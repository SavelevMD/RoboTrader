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
using Models.Tasks;

using Newtonsoft.Json;

using Robo.Database.Context;
using Robo.Database.Models;

namespace TaskManager.Storage.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TickerContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<TaskRepository> _logger;

        public TaskRepository(TickerContext context, IMapper mapper, ILogger<TaskRepository> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OperationResult> AddTaskAsync(TaskWrapper task, CancellationToken cancellationToken = default)
        {
            if (task == default)
            {
                return OperationResult.Failure($"Входной параметр {nameof(task)} не может быть null");
            }

            try
            {
                await _context.Tasks.AddAsync(_mapper.Map<Tasks>(task), cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return OperationResult.Success();
            }
            catch (Exception ex)
            {
                return OperationResult.Failure("При добавлении нового таска в БД, произошла ошибка", ex);
            }
        }

        public async Task<OperationResult<IEnumerable<TaskModel>>> GetActiveTasksAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _context.Tasks
                    .AsNoTracking()
                    .Where(r => r.IsActive)
                    .ToListAsync(cancellationToken);
                return OperationResult<IEnumerable<TaskModel>>.Success(_mapper.Map<IEnumerable<Tasks>, IEnumerable<TaskModel>>(result));
            }
            catch (Exception ex)
            {
                return OperationResult<IEnumerable<TaskModel>>.Failure($"В процессе выполнения запроса произошла ошибка", ex);
            }
        }

        public async Task<OperationResult<IEnumerable<(string pairName, int frame)>>> GetPairNamesAndFramesForActiveTasksAsync(CancellationToken cancellationToken = default)
        {
            var result = await GetActiveTasksAsync(cancellationToken);
            if (result.IsSuccess)
            {
                var pairs = result.Result.Select(r => JsonConvert.DeserializeObject<TaskWrapper>(r.Body))
                          .Select(r => (r.CurPair, r.TimeFrame))
                          .ToHashSet<(string, int)>();
                return OperationResult<IEnumerable<(string pairName, int frame)>>.Success(pairs);
            }
            return OperationResult<IEnumerable<(string pairName, int frame)>>.Failure(result.ErrorMessage, result.Exception);
        }

        public async Task<OperationResult> DeactivateTaskByNameAsync(string tskName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(tskName))
            {
                OperationResult.Failure($"Входной параметр {nameof(tskName)} не может быть равным null или пустым");
            }

            if (await _context.Tasks.AnyAsync(r => r.TaskName == tskName, cancellationToken))
            {
                var task = await _context.Tasks.SingleAsync(r => r.TaskName == tskName, cancellationToken);
                task.IsActive = false;
                await _context.SaveChangesAsync(cancellationToken);
                return OperationResult.Success();
            }
            return OperationResult.Failure($"Таск с таким именем '{tskName}' не найден");
        }

        public Task<OperationResult> UpdateTaskAsync(TaskWrapper task, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
