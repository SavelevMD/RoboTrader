using Models.Business;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Models.SystemModels
{
    public class IndicatorsFabricResult
    {
        [JsonProperty("currency_pair")]
        public string CurrencyPair { get; set; }
        [JsonProperty("receipt_time")]
        public DateTime ReciptTime { get; set; }
        [JsonProperty("candles")]
        public CandleModel Candle { get; set; }
        [JsonProperty("indicators_value")]
        public Dictionary<string, decimal> IndicatorsValue { get; set; }
    }
}
