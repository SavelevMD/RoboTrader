using System;
using System.Threading;
using System.Threading.Tasks;

namespace Models.Repositories.Currency
{
    public class CurrencyRepository : ICurrencyRepository
    {
        public Task<long> GetCurrencyPairIdByNameAsync(string currencyPairName, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
