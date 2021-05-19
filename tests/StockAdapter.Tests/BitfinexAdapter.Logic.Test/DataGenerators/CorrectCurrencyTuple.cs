using Brocker.Model;

using System.Collections.Generic;
using System.Reflection;

using Xunit.Sdk;

namespace BitfinexAdapter.Logic.UnitTests.DataGenerators
{
    public class CorrectCurrencyTuple : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[] { new CurrencyTuple("ETHBTC", 1) };
            yield return new object[] { new CurrencyTuple("ETHBTC", 1, 11) };
            yield return new object[] { new CurrencyTuple("ETHBTC", 1, 13) };
            yield return new object[] { new CurrencyTuple("BTCUSD", 1, 12) };
        }
    }
}
