using System.Collections.Generic;

using BitfinexAdapter.Logic.Converters;

using Newtonsoft.Json;

namespace BitfinexAdapter.Logic.Models.Candles
{
    [JsonConverter(typeof(CandleJsonConverter))]
    public class CandlesModel
    {
        public string CurrencyName { get; set; }

        public int TimeFrame { get; set; }

        public int ChannelId { get; set; }

        public IList<CandleModel> CandleCollection { get; set; }
    }
}
