using System.Collections.Generic;
using System.Threading.Tasks;

using Models.Results;

namespace IndicatorsBuilderService.Logic.Repositories.Indicators
{
    internal interface IIndicatorsRepositiry
    {
        Task AddIndicatorAsync(string pair, int frame, string indicatorName);
        Task AddIndicatorsAsync(string pair, int frame, HashSet<string> indicators);
        Task<OperationResult<HashSet<string>>> GetIndicatorsAsync(string pair, int frame);
    }
}