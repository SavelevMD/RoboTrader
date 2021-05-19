using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Models.Business.Indicators;
using Models.SystemModels;

namespace IndicatorsBuilderService.Logic.Services.Builders
{
    internal interface IIndicatorBuilder
    {
        Task<IEnumerable<Indicator>> GetIndicatorsByCandleAsync(IndicatorServicePack data, CancellationToken cancellationToken = default);
    }
}