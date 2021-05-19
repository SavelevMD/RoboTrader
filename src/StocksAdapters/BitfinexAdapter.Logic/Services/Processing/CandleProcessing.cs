using System;
using System.Linq;
using System.Threading.Tasks;

using BitfinexAdapter.Logic.Subscription;

using MessageBroker.Publisher;

using Microsoft.Extensions.Logging;

using Models.Business;
using Models.Enums;
using Models.JsonConverters;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BitfinexAdapter.Logic.Services.Processing
{
    internal class CandleProcessing : ICandleProcessing
    {
        private readonly SubscriptionManager _subscriptionManager;
        private readonly IPublisher _publisher;
        private readonly JsonSerializerSettings _settings;
        private readonly ILogger<CandleProcessing> _logger;

        public CandleProcessing(SubscriptionManager subscriptionManager,
            IPublisher publisher,
            ILogger<CandleProcessing> logger)
        {
            _subscriptionManager = subscriptionManager;
            _publisher = publisher;
            _settings = new JsonSerializerSettings();
            _settings.Converters.Add(new BitfinexCandlesConverter());

            _logger = logger;
        }

        public async Task CandleMessageHandleAsync(string message)
        {
            try
            {
                //TODO: think about correct serialization 
                if (JsonConvert.DeserializeObject<JArray>(message)[1].ToString() == "hb")
                {
                    //heartbeat case
                    return;
                }

                var parsedCandles = BuildCandles(message);
                if (parsedCandles != default)
                {
                    await _publisher.PublishAsync($"candle", parsedCandles, BusMessageType.Candles);
                    var c = parsedCandles.CandleCollection.First();
                    _logger.LogInformation($"new candle_{parsedCandles.CurrencyName}_{parsedCandles.TimeFrame}_{parsedCandles.CandleCollection.Count}// Close:{c.Close}// ReceiptTime:{c.ReceiptTime}", false);
                }
                else
                {
                    _logger.LogInformation("Оч странная ситуация, пришли свечи с channelID которого нет в коллекции");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "При получении свечей произошла ошибка");
            }
        }

        private CandlesModel BuildCandles(string message)
        {
            var parsedCandles = JsonConvert.DeserializeObject<CandlesModel>(message, _settings);
            var channel = _subscriptionManager.GetPairByChannel(parsedCandles.ChannelId);
            if (channel != default)
            {
                parsedCandles.CurrencyName = channel.CurrencyPair;
                parsedCandles.TimeFrame = channel.Frame;
                return parsedCandles;
            }
            return default;
        }
    }
}
