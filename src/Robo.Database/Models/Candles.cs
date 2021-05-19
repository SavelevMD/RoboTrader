using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Robo.Database.Models
{
    [Table("candles")]
    public partial class Candles
    {
        [Column("id")]
        public long Id { get; set; }
        [Required]
        [Column("time_frame")]
        [StringLength(4)]
        public string TimeFrame { get; set; }
        [Column("open")]
        public double Open { get; set; }
        [Column("close")]
        public double Close { get; set; }
        [Column("high")]
        public double High { get; set; }
        [Column("low")]
        public double Low { get; set; }
        [Column("volume")]
        public double Volume { get; set; }
        [Column("cur_pair_id")]
        public long? CurPairId { get; set; }
        [Column("receipt_time", TypeName = "timestamp with time zone")]
        public DateTime ReceiptTime { get; set; }
        [Column("mtc")]
        public long? Mtc { get; set; }
        [Column("slice_period")]
        public long? SlicePeriod { get; set; }
    }
}
