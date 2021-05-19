using System;
using System.Linq;
using System.Threading.Tasks;

using BitfinexAdapter.Logic.Extensions;
using BitfinexAdapter.Logic.Extensions.Bitfinex.Net.Extensions.Enums;
using BitfinexAdapter.Logic.Subscription;

using Brocker.Model;

using MessageBroker.Publisher;

using Microsoft.Extensions.Logging;

using Models.Business.BusMessages;
using Models.Enums;
using Models.SystemModels;

using Newtonsoft.Json;

namespace BitfinexAdapter.Logic.Services.Processing
{
    internal class EventMessageProcessing : IEventMessageProcessing
    {
        private readonly IPublisher _publisher;
        private readonly SubscriptionManager _subscriptionManager;
        private readonly BitfinexMessageGenerator _messageGenerator;
        private readonly ISocketCommandService _bitfinexSocketService;

        private readonly PingPongRequestSchedulerService _pingPongRequestSchedulerService;

        private readonly ILogger<EventMessageProcessing> _logger;

        public EventMessageProcessing(IPublisher publisher,
            SubscriptionManager subscriptionManager,
            BitfinexMessageGenerator messageGenerator,
            ISocketCommandService bitfinexSocketService,
            PingPongRequestSchedulerService pingPongRequestSchedulerService,
            ILogger<EventMessageProcessing> logger)
        {
            _publisher = publisher;
            _subscriptionManager = subscriptionManager;
            _messageGenerator = messageGenerator;
            _bitfinexSocketService = bitfinexSocketService;
            _pingPongRequestSchedulerService = pingPongRequestSchedulerService;
            _logger = logger;
        }

        public async Task StatusMessageHandleAsync(string message)
        {
            var eventType = JsonConvert.DeserializeObject<BaseMessage>(message);

            switch (eventType.Event)
            {
                case MessageType.Subscribed:
                    var (_, frame) = _subscriptionManager.SubscribedAction(message);
                    _pingPongRequestSchedulerService.NewSubscription(frame);
                    break;
                case MessageType.Unsubscribed:
                    var resultUnsub = _subscriptionManager.UnsubscribedAction(message, _messageGenerator, _bitfinexSocketService);
                    if (resultUnsub != default)
                    {
                        _pingPongRequestSchedulerService.RemoveScheduledRequest(resultUnsub.Frame);
                    }
                    break;
                case MessageType.Pong:
                    await PublishPingPongAsync(message);
                    break;
                case MessageType.Error:
                    _bitfinexSocketService.ErrorMessageProcessing(JsonConvert.DeserializeObject<ErrorMessage>(message), _logger);
                    _logger.LogError(message);
                    break;
                case MessageType.Info:
                    _bitfinexSocketService.InfoMessageProcessing(JsonConvert.DeserializeObject<InfoMessage>(message), _logger);
                    _logger.LogInformation(message);
                    break;
                default:
                    _logger.LogError($"Пришла не обработанная команда: {message}");
                    break;
            }
        }

        private async Task PublishPingPongAsync(string message)
        {
            try
            {
                var answer = JsonConvert.DeserializeObject<PongResponse>(message);

                var timeOfStock = DateTime.UnixEpoch.AddMilliseconds(answer.TimeStamp);

                _logger.LogInformation($"pong received, time of stock: {timeOfStock}");

                var frame = _pingPongRequestSchedulerService.TryGetFrameByCID(answer.CID);

                var pairsCollection = _subscriptionManager.Pairs.Where(r => r.Frame == frame);
                if (pairsCollection.Any())
                {
                    var delay = TimeSpan.FromMinutes(1).TotalSeconds - timeOfStock.Second;
                    await Task.Delay((int)TimeSpan.FromSeconds(delay).TotalMilliseconds);

                    //notification about new minute
                    await _publisher.PublishAsync("current_time", JsonConvert.SerializeObject(timeOfStock.AddSeconds(delay)), BusMessageType.Candles);

                    var messageTime = timeOfStock.AddSeconds(-timeOfStock.Second).AddMilliseconds(-timeOfStock.Millisecond);

                    var tasks = pairsCollection.Select(r => _publisher.PublishAsync($"candle_{r.CurrencyPair}_{r.Frame}_ping_pong",
                                                                                    BuildIndicatorMessage(r.CurrencyPair, r.Frame, messageTime)));
                    //что бы уж точно все свечи долетели
                    await Task.Delay(TimeSpan.FromSeconds(15));
                    await Task.WhenAll(tasks);
                    _logger.LogInformation($"pong delay: {delay} value of candles is sended $$$$$");
                }
                else
                {
                    _logger.LogError("По заданному CID не найдены пары");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Во время выполнения обработки PingPong произошла ошибка");
            }
        }

        private BaseMessage<CandleListnerParameters> BuildIndicatorMessage(string currencyPair, int period, DateTimeOffset actualTime)
        {
            return new BaseMessage<CandleListnerParameters>
            {
                Content = new CandleListnerParameters
                {
                    ChannelName = currencyPair,
                    Period = period
                },
                CreationPoint = actualTime,
                MessageType = BusMessageType.Candles
            };
        }
    }
}
