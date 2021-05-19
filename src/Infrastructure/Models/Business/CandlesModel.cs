using System;
using System.Collections.Generic;


namespace Models.Business
{
    public class CandlesModel
    {
        public string CurrencyName { get; set; }

        public int TimeFrame { get; set; }

        public int ChannelId { get; set; }

        public IList<CandleModel> CandleCollection { get; set; }

        public DateTimeOffset StockTime { get; set; }
    }
}
