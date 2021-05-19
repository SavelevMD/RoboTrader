using Newtonsoft.Json;

namespace Brocker.Model
{
    public class SubscribeResponse
    {
        [JsonProperty("event")]
        public string EventName { get; set; }
        public string Сhannel { get; set; }
        public int СhanId { get; set; }
        public string Key { get; set; }
    }
}
