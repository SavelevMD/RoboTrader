using Newtonsoft.Json;

namespace Brocker.Model
{
    public class UnsubscribeMessage : BaseMessage
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
}
