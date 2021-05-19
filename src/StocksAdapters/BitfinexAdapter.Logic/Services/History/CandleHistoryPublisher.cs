using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using BitfinexAdapter.Logic.REST;

using MessageBroker.Publisher;

using Microsoft.Extensions.Logging;

using Models.Business;
using Models.Enums;

namespace BitfinexAdapter.Logic.Services.History
{
    internal class CandleHistoryPublisher : ICandleHistoryPublisher
    {
        private readonly IPublisher _publisher;
        private readonly ICandlesHistoryRest _candlesHistory;
        private readonly IMapper _mapper;
        private readonly ILogger<CandleHistoryPublisher> _logger;

        public CandleHistoryPublisher(IPublisher publisher,
            ICandlesHistoryRest candlesHistory,
            IMapper mapper,
            ILogger<CandleHistoryPublisher> logger)
        {
            _publisher = publisher;
            _candlesHistory = candlesHistory;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task ReceiveAndPublishCandleAsync(string pairName, int frame)
        {
            try
            {
                var bitfinexCandles = (await _candlesHistory.GetCandleHistoryAsync(pairName, frame)).ToList();
                var candlesModel = new CandlesModel
                {
                    CurrencyName = pairName,
                    TimeFrame = frame,
                    CandleCollection = _mapper.Map<IList<CandleModel>>(bitfinexCandles)
                };

                await _publisher.PublishAsync($"candle", candlesModel, BusMessageType.Candles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "В процессе получения и публикации свечи произошла ошибка");
            }
        }
    }
}
