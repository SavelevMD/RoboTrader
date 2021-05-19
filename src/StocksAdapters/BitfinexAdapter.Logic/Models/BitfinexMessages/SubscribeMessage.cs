
using BitfinexAdapter.Logic.Extensions.Bitfinex.Net.Extensions.Enums;

using Newtonsoft.Json;

namespace Brocker.Model
{
    public class SubscribeMessage : BaseMessage
    {
        [JsonProperty("chanId")]
        public int ChannelId { get; set; }

        [JsonProperty("channel")]
        public string ChannelName { get; set; }

        /// <summary>
        /// Composed "key: 'trade:1m:tBTCUSD'"
        /// </summary>
        [JsonProperty("key")]
        public string Key { get; set; }
    }

    public class BaseMessage
    {
        [JsonProperty("event")]
        public MessageType Event { get; set; }
    }

    public class ErrorMessage : BaseMessage
    {
        [JsonProperty("msg")]
        public string Message { get; set; }
        public ErrorCodes Code { get; set; }
    }

    public class InfoMessage : BaseMessage
    {
        public string Vesion { get; set; }
        public Platform Platform { get; set; }

        [JsonProperty("msg")]
        public string Message { get; set; }
        public ErrorCodes Code { get; set; }
    }

    public class Platform
    {
        public int Status { get; set; }
    }
}
