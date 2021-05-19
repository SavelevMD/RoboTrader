using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Robo.Database.Models
{
    [Table("true_diapasons")]
    public partial class TrueDiapasons
    {
        [Column("currency_id")]
        public long CurrencyId { get; set; }
        [Column("date_shot", TypeName = "timestamp with time zone")]
        public DateTime DateShot { get; set; }
        [Column("n_period")]
        public int NPeriod { get; set; }
        [Column("prev_close_value")]
        public double? PrevCloseValue { get; set; }
        [Column("prev_close_time", TypeName = "timestamp with time zone")]
        public DateTime? PrevCloseTime { get; set; }
        [Column("cur_max_value")]
        public double? CurMaxValue { get; set; }
        [Column("cur_max_time", TypeName = "timestamp with time zone")]
        public DateTime? CurMaxTime { get; set; }
        [Column("cur_min_value")]
        public double? CurMinValue { get; set; }
        [Column("cur_min_time", TypeName = "timestamp with time zone")]
        public DateTime? CurMinTime { get; set; }
        [Column("true_max")]
        public double? TrueMax { get; set; }
        [Column("true_min")]
        public double? TrueMin { get; set; }
        [Column("true_diapason")]
        public double? TrueDiapason { get; set; }
    }
}
