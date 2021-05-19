using System;
using System.Linq;
using System.Threading.Tasks;

using IndicatorsBuilderService.Logic.Repositories.Candles;
using IndicatorsBuilderService.Logic.Repositories.Indicators;
using IndicatorsBuilderService.Logic.Services.Builders;

using MessageBroker.Publisher;

using Microsoft.Extensions.Logging;

using Models.Business;
using Models.Business.BusMessages;
using Models.SystemModels;

using Newtonsoft.Json;

namespace IndicatorsBuilderService.Logic.Services.Processing
{
    internal class CandleProcessing : ICandleProcessing
    {
        private readonly IIndicatorsRepositiry _indicatorsRepositiry;
        private readonly IIndicatorsValueRepository _indicatorsValueRepository;
        private readonly ICandleRepository _candleRepository;
        private readonly IIndicatorBuilder _indicatorBuilder;
        private readonly IPublisher _publisher;
        private readonly ILogger<CandleProcessing> _logger;

        public CandleProcessing(IIndicatorsRepositiry indicatorsRepositiry,
            IIndicatorsValueRepository indicatorsValueRepository,
            ICandleRepository candleRepository,
            IIndicatorBuilder indicatorBuilder,
            IPublisher publisher,
            ILogger<CandleProcessing> logger)
        {
            _indicatorsRepositiry = indicatorsRepositiry;
            _indicatorsValueRepository = indicatorsValueRepository;
            _candleRepository = candleRepository;
            _indicatorBuilder = indicatorBuilder;
            _publisher = publisher;
            _logger = logger;
        }

        public async Task CandlesProcessingAsync(string channelName, BaseMessage<CandlesModel> candles)
        {
            await _candleRepository.AddCandleAsync(candles.Content);
        }

        public async Task PingPongProcessingAsync(string channelName, BaseMessage<CandleListnerParameters> pingPongParameters)
        {
            try
            {
                var frame = pingPongParameters.Content.Period;
                var pairName = pingPongParameters.Content.ChannelName;

                var candleDT = DateTimeOffset.UtcNow.AddMinutes(-2000 * frame).DateTime;
                var candles = await _candleRepository.GetCandlesAsync(pairName,
                                                                      frame,
                                                                      candleDT.AddSeconds(-candleDT.Second).AddMilliseconds(-candleDT.Millisecond));
                var indicators = await _indicatorsRepositiry.GetIndicatorsAsync(pairName, frame);

                if (candles.IsSuccess &&
                    indicators.IsSuccess)
                {
                    var indicatorsPack = new IndicatorServicePack
                    {
                        Candles = candles.Result,
                        Indicators = indicators.Result
                    };

                    var indicatorsResult = await _indicatorBuilder.GetIndicatorsByCandleAsync(indicatorsPack);

                    var tasksForSave = indicatorsResult.Select(r => _indicatorsValueRepository.AddIndicatorsAsync(pairName, frame, r));
                    await Task.WhenAll(tasksForSave);

                    await _publisher.PublishAsync("indicators_value", pingPongParameters.Content, Models.Enums.BusMessageType.Indicators);
                }
                else
                {
                    _logger.LogError($"Операция построения индикаторов не удалась, по причине {candles.ErrorMessage}, {indicators.ErrorMessage} или список индикаторов был пуст");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"В процессе обработки операции ping pong {JsonConvert.SerializeObject(pingPongParameters.Content)} произошла обшика");
            }
        }
    }
}
