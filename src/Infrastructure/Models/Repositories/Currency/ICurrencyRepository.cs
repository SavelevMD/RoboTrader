using System.Threading;
using System.Threading.Tasks;

namespace Models.Repositories.Currency
{
    public interface ICurrencyRepository
    {
        Task<long> GetCurrencyPairIdByNameAsync(string currencyPairName, CancellationToken cancellationToken = default);
    }
}
