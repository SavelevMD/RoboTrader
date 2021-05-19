using System.Collections.Generic;

using BitfinexAdapter.Logic.Extensions.Bitfinex.Net.Extensions.Enums;

using Brocker.BackgroundWorker.Model;
using Brocker.Model;

using Models.Results;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace BitfinexAdapter.Logic.Services
{
    internal class BitfinexMessageGenerator
    {
        private static readonly JsonSerializerSettings _bitfinexJsonSettings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.None,
            Converters = new List<JsonConverter>() { new StringEnumConverter(typeof(CamelCaseNamingStrategy)) },
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public OperationResult<string> Subscribe(string pairName, int period)
        {
            if (string.IsNullOrWhiteSpace(pairName) || period <= 0)
            {
                return OperationResult<string>.Failure($"Параметры отписки на прослушку пары {nameof(pairName)}, {nameof(period)} не могут быть пустыми или null");
            }

            var message = new SubscribeMessage
            {
                Event = MessageType.Subscribe,
                ChannelName = "candles",
                Key = $"trade:{period}m:t{pairName.ToUpper()}"
            };
            return OperationResult<string>.Success(JsonConvert.SerializeObject(message, _bitfinexJsonSettings));
        }

        public OperationResult<string> Unsubscribe(string pairName, int period, int channelId)
        {
            if (string.IsNullOrWhiteSpace(pairName) || period <= 0)
            {
                return OperationResult<string>.Failure($"Параметры отписки на прослушку пары {nameof(pairName)}, {nameof(period)} не могут быть пустыми или null");
            }

            var message = new UnsubscribeMessage
            {
                Event = MessageType.Unsubscribe,
                ChannelName = "candles",
                Key = $"trade:{period}m:t{pairName.ToUpper()}",
                ChannelId = channelId
            };
            return OperationResult<string>.Success(JsonConvert.SerializeObject(message, _bitfinexJsonSettings));
        }

        public string GeneratePingMessage(int cid)
        {
            var message = new PingRequest()
            {
                Event = "ping",
                CID = cid
            };
            return JsonConvert.SerializeObject(message, _bitfinexJsonSettings);
        }
    }
}
