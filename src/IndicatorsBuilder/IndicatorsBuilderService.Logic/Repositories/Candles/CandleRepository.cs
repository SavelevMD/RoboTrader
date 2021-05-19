using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

using Models.Business;
using Models.Results;

using Newtonsoft.Json;

namespace IndicatorsBuilderService.Logic.Repositories.Candles
{
    internal class CandleRepository : ICandleRepository
    {
        private readonly IDistributedCache _distributedCache;
        private readonly ILogger<CandleRepository> _logger;

        private static readonly string _PREFIX = "candles";

        public CandleRepository(IDistributedCache distributedCache, ILogger<CandleRepository> logger)
        {
            _distributedCache = distributedCache;
            _logger = logger;
        }

        public async Task AddCandleAsync(CandlesModel candles, CancellationToken cancellationToken = default)
        {
            try
            {
                var tasks = candles.CandleCollection.Select(r => _distributedCache.SetStringAsync($"{_PREFIX}:{candles.CurrencyName}:{candles.TimeFrame}:{r.ReceiptTime}", JsonConvert.SerializeObject(r)));
                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"При добавление свечей в cache возникла ошибка");
            }
        }

        public async Task<OperationResult<CandleModel>> GetCandleAsync(string pairName, int frame, DateTimeOffset dateTime)
        {
            //TODO: проверка входных параметров
            var serializedCandle = await _distributedCache.GetStringAsync($"{_PREFIX}:{pairName}:{frame}:{dateTime}");

            return string.IsNullOrWhiteSpace(serializedCandle)
                ? OperationResult<CandleModel>.Success(JsonConvert.DeserializeObject<CandleModel>(serializedCandle))
                : OperationResult<CandleModel>.Failure($"По данному ключу {pairName}:{frame}:{dateTime} значение не найдено");
        }

        //TODO: подумать над вот этим решением, что ты тут вообще хотел, и еще раз пересмотри интерфейс репозитория
        public async Task<OperationResult<IEnumerable<CandleModel>>> GetCandlesAsync(string pairName, int frame, DateTimeOffset startDateTime)
        {
            var collection = DateEnumration(startDateTime, frame)
                .Select(r => _distributedCache.GetStringAsync($"{_PREFIX}:{pairName}:{frame}:{r.DateTime}"));
            var result = await Task.WhenAll(collection);

            var resultCollection = result.Where(r => !string.IsNullOrWhiteSpace(r))
                .Select(r => JsonConvert.DeserializeObject<CandleModel>(r))
                .ToList();

            return resultCollection.Any()
                ? OperationResult<IEnumerable<CandleModel>>.Success(resultCollection)
                : OperationResult<IEnumerable<CandleModel>>.Failure($"По данным параметрам {pairName}:{frame}:{startDateTime} свечи не найдены");
        }

        private IEnumerable<DateTimeOffset> DateEnumration(DateTimeOffset start, int period)
        {
            //TODO: 2000 magic number => Options
            return Enumerable.Range(0, 2000).Select(r => start.AddMinutes(r * period));
        }
    }
}
