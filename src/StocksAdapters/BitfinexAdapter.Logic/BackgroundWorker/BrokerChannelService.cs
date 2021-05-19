using System;
using System.Threading;
using System.Threading.Tasks;

using BitfinexAdapter.Logic.Extensions;
using BitfinexAdapter.Logic.Services;
using BitfinexAdapter.Logic.Services.History;
using BitfinexAdapter.Logic.Subscription;

using MessageBroker.Subscriber;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Models.Business.BusMessages;
using Models.Enums;
using Models.Results;
using Models.SystemModels;

namespace Brocker.BackgroundWorker
{
    internal class BrokerChannelService : BackgroundService
    {
        private readonly ILogger<BrokerChannelService> _logger;
        private readonly ISubscriber _subscriber;
        private readonly SubscriptionManager _subscriptionManager;
        private readonly BitfinexMessageGenerator _messageGenerator;
        private readonly ISocketCommandService _bitfinexSocketService;
        private readonly ICandleHistoryPublisher _candleHistoryPublisher;
        private const string BrockerChannelPrefix = "brocker";

        public BrokerChannelService(ISubscriber subscriber,
                                    SubscriptionManager subscriptionManager,
                                    BitfinexMessageGenerator messageGenerator,
                                    ISocketCommandService bitfinexSocketService,
                                    ICandleHistoryPublisher candleHistoryPublisher,
                                    ILogger<BrokerChannelService> logger)
        {
            _logger = logger;
            _subscriber = subscriber;
            _subscriptionManager = subscriptionManager;
            _messageGenerator = messageGenerator;
            _bitfinexSocketService = bitfinexSocketService;
            _candleHistoryPublisher = candleHistoryPublisher;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _subscriber.SubscribeAsync<BaseMessage<CandleListnerParameters>>($"{BrockerChannelPrefix}", BrockerChannelHandlerAsync);
        }

        #region private

        private async void BrockerChannelHandlerAsync(string channel, BaseMessage<CandleListnerParameters> value)
        {
            try
            {
                //TODO: add validation input params

                var command = _messageGenerator.GenerateMessage(value, _subscriptionManager);
                if (command.IsSuccess)
                {
                    _subscriptionManager.MarkOnUnsubscribe(value);

                    await RequestAndPublishHistoryCandlesAsync(value);

                    SendCommand(command);
                }
                else
                {
                    _logger.LogError(command.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "При обработке сообщения для broker произошла ошибка");
            }
        }

        private void SendCommand(OperationResult<string> command)
        {
            var result = _bitfinexSocketService.SendCommand(command.Result);
            if (!result.IsSuccess)
            {
                _logger.LogError(result.Exception, result.ErrorMessage);
            }
        }

        private async Task RequestAndPublishHistoryCandlesAsync(BaseMessage<CandleListnerParameters> subscription)
        {
            if (subscription == default)
            {
                throw new ArgumentNullException($"Входной параметр {nameof(subscription)} не может быть null");
            }

            //TODO: think about IsMaintenanceMode, what we can do?
            if (subscription.MessageType == BusMessageType.SubscribeChannel &&
                !_bitfinexSocketService.IsMaintenanceMode)
            {
                await _candleHistoryPublisher.ReceiveAndPublishCandleAsync(subscription.Content.ChannelName, subscription.Content.Period);
            }
        }
        #endregion
    }
}
