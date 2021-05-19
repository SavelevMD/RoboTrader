using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

using Models.Business.Indicators;
using Models.Results;

namespace IndicatorsBuilderService.Logic.Repositories.Indicators
{
    //TODO: добавить валидацию на входные параметры
    internal class IndicatorsValueRepository : IIndicatorsValueRepository
    {
        private readonly IDistributedCache _distributedCache;
        private readonly ILogger<IndicatorsValueRepository> _logger;

        private const string PREFIX = "indicatorValue";

        public IndicatorsValueRepository(IDistributedCache distributedCache, ILogger<IndicatorsValueRepository> logger)
        {
            _distributedCache = distributedCache;
            _logger = logger;
        }

        public async Task AddIndicatorsAsync(string pair, int frame, Indicator indicator)
        {
            try
            {
                var tasks = indicator.DateTimeValueSegment
                    .Select(r => _distributedCache.SetStringAsync($"{PREFIX}:{pair}:{indicator.IndicatorName}:{frame}:{r.ValueDatetTime}", r.Value.ToString()));
                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"При добавлении значения индикатора {indicator.IndicatorName}:{frame}:{indicator.DateTimeValueSegment.Max(r => r.ValueDatetTime)} произошла ошибка");
            }
        }

        public async Task<OperationResult<double>> GetIndicatorAsync(string pair, string indicatorName, int frame, DateTime dateTime)
        {
            try
            {
                var result = await _distributedCache.GetStringAsync($"{PREFIX}:{pair}:{indicatorName}:{frame}:{dateTime}");
                return result != default
                    ? OperationResult<double>.Success(double.Parse(result))
                    : OperationResult<double>.Failure($"Не удалось найти значение по данному ключу {pair}:{indicatorName}:{frame}:{dateTime}");
            }
            catch (Exception ex)
            {
                return OperationResult<double>.Failure($"В процессе поиска в кеш значения произошла ошибка {ex.Message}{Environment.NewLine}{ex.StackTrace}");
            }
        }
    }
}
