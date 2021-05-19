using System.Collections.Generic;
using System.Threading.Tasks;

using BitfinexAdapter.Logic.Models.Candles;

namespace BitfinexAdapter.Logic.REST
{
    internal interface ICandlesHistoryRest
    {
        Task<IEnumerable<CandleModel>> GetCandleHistoryAsync(string pairName, int period);
    }
}
