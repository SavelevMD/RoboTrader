using Models.Business;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Models.Repositories.Candles
{
    public interface ICandleRepository
    {
        Task AddCandleAsync(CandleModel candle);

        Task AddRangeCandlesAsync(IEnumerable<CandleModel> candles);
    }
}
