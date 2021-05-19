using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Models.Business;
using Models.Results;

namespace IndicatorsBuilderService.Logic.Repositories.Candles
{
    internal interface ICandleRepository
    {
        Task AddCandleAsync(CandlesModel candles, CancellationToken cancellationToken = default);

        Task<OperationResult<CandleModel>> GetCandleAsync(string pairName, int frame, DateTimeOffset dateTime);

        Task<OperationResult<IEnumerable<CandleModel>>> GetCandlesAsync(string pairName, int frame, DateTimeOffset startDateTime);
    }
}