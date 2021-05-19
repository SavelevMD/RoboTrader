using System.ComponentModel.DataAnnotations.Schema;

namespace Robo.Database.Models
{
    [Table("task_strategy_check_deep")]
    public partial class TaskStrategyCheckDeep
    {
        public string ExchCandleTime { get; set; }
        public float? DealPrice { get; set; }
        public float? LastPrice { get; set; }
        public int? CurrentState { get; set; }
        public string PeriodCounter { get; set; }
        [Column("RSI", TypeName = "numeric(16,12)")]
        public decimal? Rsi { get; set; }
        [Column("ema_9_13", TypeName = "numeric")]
        public decimal? Ema913 { get; set; }
        [Column("ema_13_39", TypeName = "numeric")]
        public decimal? Ema1339 { get; set; }
        [Column("ema_101_39", TypeName = "numeric")]
        public decimal? Ema10139 { get; set; }
        [Column("ema_101_9", TypeName = "numeric")]
        public decimal? Ema1019 { get; set; }
        [Column("ema9", TypeName = "numeric(16,12)")]
        public decimal? Ema9 { get; set; }
        [Column("ema13", TypeName = "numeric(16,12)")]
        public decimal? Ema13 { get; set; }
        [Column("ema39", TypeName = "numeric(16,12)")]
        public decimal? Ema39 { get; set; }
        [Column("ema101", TypeName = "numeric(16,12)")]
        public decimal? Ema101 { get; set; }
        [Column("condition_check")]
        public string ConditionCheck { get; set; }
    }
}
