using System;

namespace Models.Business
{
    public partial class CandleModel : ICloneable
    {
        public long Id { get; set; }
        public string TimeFrame { get; set; }
        public decimal Open { get; set; }
        public decimal Close { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Volume { get; set; }
        public long? CurPairId { get; set; }
        public DateTime ReceiptTime { get; set; }
        public long? Mtc { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
