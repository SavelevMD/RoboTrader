using Models.Business;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Models.SystemModels
{
    /// <summary>
    /// Class for transfer data for indicator rest sevice service
    /// </summary>
    public class IndicatorServicePack
    {
        [JsonProperty("indicators")]
        public IEnumerable<string> Indicators { get; set; }
        [JsonProperty("candles")]
        public IEnumerable<CandleModel> Candles { get; set; }
    }
}
