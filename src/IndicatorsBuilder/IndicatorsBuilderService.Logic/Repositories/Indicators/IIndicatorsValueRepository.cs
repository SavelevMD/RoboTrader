using System;
using System.Threading.Tasks;

using Models.Business.Indicators;
using Models.Results;

namespace IndicatorsBuilderService.Logic.Repositories.Indicators
{
    internal interface IIndicatorsValueRepository
    {
        Task AddIndicatorsAsync(string pair, int frame, Indicator indicator);
        Task<OperationResult<double>> GetIndicatorAsync(string pair, string indicatorName, int frame, DateTime dateTime);
    }
}