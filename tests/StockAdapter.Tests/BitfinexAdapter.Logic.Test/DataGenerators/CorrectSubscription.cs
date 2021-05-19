using System.Collections.Generic;
using System.Reflection;

using BitfinexAdapter.Logic.Extensions.Bitfinex.Net.Extensions.Enums;

using Brocker.Model;

using Xunit.Sdk;

namespace BitfinexAdapter.Logic.UnitTests.DataGenerators
{
    public class CorrectSubscription : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[] { new SubscribeMessage { ChannelId = 111, ChannelName = "BTCUSD", Event = MessageType.Subscribed, Key = "trade:1m:tBTCUSD" } };
            yield return new object[] { new SubscribeMessage { ChannelId = 112, ChannelName = "ETHBTC", Event = MessageType.Subscribed, Key = "trade:1m:tETHBTC" } };
            yield return new object[] { new SubscribeMessage { ChannelId = 1110, ChannelName = "BTCUSD", Event = MessageType.Subscribed, Key = "trade:1m:tBTCUSD" } };
        }
    }
}
