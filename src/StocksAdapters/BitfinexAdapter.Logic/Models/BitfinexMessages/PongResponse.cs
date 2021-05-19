using Newtonsoft.Json;

namespace Brocker.Model
{
    internal class PongResponse
    {
        [JsonProperty("ts")]
        public long TimeStamp { get; set; }

        public int CID { get; set; }
    }
}
