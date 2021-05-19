using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using BitfinexAdapter.Logic.Services;
using BitfinexAdapter.Logic.Services.History;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using TaskManager.Storage.Repositories;

namespace BitfinexAdapter.Logic.BackgroundWorker
{
    internal class WarmupSubscriptionsService : BackgroundService
    {
        private readonly TaskRepository _taskRepository;
        private readonly BitfinexMessageGenerator _messageGenerator;
        private readonly ISocketCommandService _bitfinexSocketService;
        private readonly ICandleHistoryPublisher _candleHistoryPublisher;
        private readonly ILogger<WarmupSubscriptionsService> _logger;

        public WarmupSubscriptionsService(TaskRepository taskRepository,
            BitfinexMessageGenerator messageGenerator,
            ISocketCommandService bitfinexSocketService,
            ICandleHistoryPublisher candleHistoryPublisher,
            ILogger<WarmupSubscriptionsService> logger)
        {
            _taskRepository = taskRepository;
            _messageGenerator = messageGenerator;
            _bitfinexSocketService = bitfinexSocketService;
            _candleHistoryPublisher = candleHistoryPublisher;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                //TODO: завести команду в нормальном виде а не вот это вот все
                _bitfinexSocketService.SendCommand("{\"event\":\"conf\",\"flags\":32768}");

                var subscriptions = await _taskRepository.GetPairNamesAndFramesForActiveTasksAsync();
                if (subscriptions.IsSuccess)
                {
                    var publisherTasks = subscriptions.Result
                        .Select(r => _candleHistoryPublisher.ReceiveAndPublishCandleAsync(r.pairName, r.frame));
                    await Task.WhenAll(publisherTasks);

                    subscriptions.Result.Select(r => _messageGenerator.Subscribe(r.pairName, r.frame))
                        .Where(r => r.IsSuccess)
                        .ToList()
                        .ForEach(r => _bitfinexSocketService.SendCommand(r.Result));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "В процессе разогрева подписок на свечи произошла ошибка");
                throw;
            }
        }
    }
}
