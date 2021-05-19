using System.Threading;
using System.Threading.Tasks;

using IndicatorsBuilderService.Logic.Services.Processing;

using MessageBroker.Subscriber;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Models.Business;
using Models.Business.BusMessages;
using Models.SystemModels;
using Models.Tasks;

namespace IndicatorsBuilderService.Logic.HostedServices
{
    internal class ChannelListnerHostedService : IHostedService
    {
        private readonly ISubscriber _subscriber;
        private readonly ICandleProcessing _candleProcessing;
        private readonly ITaskProcessing _taskProcessing;
        private readonly ILogger<ChannelListnerHostedService> _logger;

        public ChannelListnerHostedService(ISubscriber subscriber,
            ICandleProcessing candleProcessing,
            ITaskProcessing taskProcessing,
            ILogger<ChannelListnerHostedService> logger)
        {
            _subscriber = subscriber;
            _candleProcessing = candleProcessing;
            _taskProcessing = taskProcessing;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _subscriber.SubscribeAsync<BaseMessage<TaskWrapper>>("task_indicators_add", _taskProcessing.AddTasksProcessingAsync);
            await _subscriber.SubscribeAsync<BaseMessage<string>>("task_indicators_kill", _taskProcessing.KillTasksProcessingAsync);
            await _subscriber.SubscribeAsync<BaseMessage<CandleListnerParameters>>("candle*ping_pong", _candleProcessing.PingPongProcessingAsync);
            await _subscriber.SubscribeAsync<BaseMessage<CandlesModel>>("candle", _candleProcessing.CandlesProcessingAsync);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
