using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

using Models.Results;

using Newtonsoft.Json;

namespace IndicatorsBuilderService.Logic.Repositories.Indicators
{
    internal class IndicatorsRepositiry : IIndicatorsRepositiry
    {
        private readonly IDistributedCache _distributedCache;
        private readonly ILogger<IndicatorsRepositiry> _logger;
        private const string PREFIX = "indicators";

        public IndicatorsRepositiry(IDistributedCache distributedCache, ILogger<IndicatorsRepositiry> logger) => (_distributedCache, _logger) = (distributedCache, logger);

        public async Task AddIndicatorsAsync(string pair, int frame, HashSet<string> indicators)
        {
            if (string.IsNullOrWhiteSpace(pair) ||
                indicators == default || !indicators.Any() ||
                frame <= 0)
            {
                throw new ArgumentException($"Входной параметры не корректны {nameof(pair)}:{pair} || {nameof(indicators)}:{indicators} || {nameof(frame)}:{frame}");
            }

            try
            {
                var collection = await GetIndicatorsAsync(pair, frame);
                if (collection.IsSuccess)
                {
                    collection.Result.UnionWith(indicators);
                    await _distributedCache.SetStringAsync($"{PREFIX}:{pair}:{frame}", JsonConvert.SerializeObject(collection.Result));
                }
                else
                {
                    await _distributedCache.SetStringAsync($"{PREFIX}:{pair}:{frame}", JsonConvert.SerializeObject(indicators));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"В процессе получения или сохранения индикаторов по параметрам {pair}:{frame} произошла ошибка {ex.Message}:{Environment.NewLine}:{ex.StackTrace}");
            }
        }

        public async Task AddIndicatorAsync(string pair, int frame, string indicatorName)
        {
            //TODO: validation
            if (!string.IsNullOrWhiteSpace(pair) ||
                !string.IsNullOrWhiteSpace(indicatorName) ||
                frame <= 0)
            {
                throw new ArgumentException($"Входной параметры не корректны {nameof(pair)}:{pair} || {nameof(indicatorName)}:{indicatorName} || {nameof(frame)}:{frame}");
            }

            try
            {
                var collection = await GetIndicatorsAsync(pair, frame);
                if (collection.IsSuccess)
                {
                    collection.Result.Add(indicatorName);
                    await _distributedCache.SetStringAsync($"{PREFIX}:{pair}:{frame}", JsonConvert.SerializeObject(collection.Result));
                }
                else
                {
                    _logger.LogError($"{collection.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"В процессе получения индикаторов по параметрам {pair}:{frame} произошла ошибка {ex.Message}:{Environment.NewLine}:{ex.StackTrace}");
            }

        }

        //TODO: convert on OperationResult
        public async Task<OperationResult<HashSet<string>>> GetIndicatorsAsync(string pair, int frame)
        {
            var result = await _distributedCache.GetStringAsync($"{PREFIX}:{pair}:{frame}");
            return result == default
                ? OperationResult<HashSet<string>>.Failure($"По данным параметрам {PREFIX}:{pair}:{frame} список индикаторов оказался пустой")
                : OperationResult<HashSet<string>>.Success(JsonConvert.DeserializeObject<HashSet<string>>(result));
        }
    }
}
