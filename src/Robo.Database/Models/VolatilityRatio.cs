using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Robo.Database.Models
{
    [Table("volatility_ratio")]
    public partial class VolatilityRatio
    {
        [Column("currency_id")]
        public long CurrencyId { get; set; }
        [Column("date_shot", TypeName = "timestamp with time zone")]
        public DateTime DateShot { get; set; }
        [Column("period")]
        public int Period { get; set; }
        [Column("vr")]
        public double Vr { get; set; }
    }
}
