using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Robo.Database.Models
{
    [Table("ticker")]
    public partial class Ticker
    {
        [Column("id")]
        public long Id { get; set; }
        [Column("receipt_time", TypeName = "timestamp(6) with time zone")]
        public DateTime ReceiptTime { get; set; }
        [Column("last_price")]
        public double LastPrice { get; set; }
        [Column("cur_pair_id")]
        public long? CurPairId { get; set; }
    }
}
