using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using IndicatorsBuilderService.Logic.Repositories.Indicators;

using Microsoft.Extensions.Logging;

using Models.Business.BusMessages;
using Models.Tasks;

namespace IndicatorsBuilderService.Logic.Services.Processing
{
    internal class TaskProcessing : ITaskProcessing
    {
        private readonly IIndicatorsRepositiry _indicatorsRepositiry;
        private readonly ILogger<TaskProcessing> _logger;

        public TaskProcessing(IIndicatorsRepositiry indicatorsRepositiry, ILogger<TaskProcessing> logger) =>
            (_indicatorsRepositiry, _logger) = (indicatorsRepositiry, logger);

        public Task KillTasksProcessingAsync(string channelName, BaseMessage<string> taskName)
        {
            throw new NotImplementedException();
        }

        public async void AddTasksProcessingAsync(string channelName, BaseMessage<TaskWrapper> task)
        {
            try
            {
                await _indicatorsRepositiry.AddIndicatorsAsync(task.Content.CurPair,
                    task.Content.TimeFrame,
                    new HashSet<string>(task.Content.Indicators));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"При добавлении индикаторов нового таска произошла ошибка");
            }
        }
    }
}
