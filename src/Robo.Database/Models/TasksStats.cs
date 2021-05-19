using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Robo.Database.Models
{
    [Table("tasks_stats")]
    public partial class TasksStats
    {
        public string TaskName { get; set; }
        public string CurrencyPair { get; set; }
        [Column(TypeName = "timestamp with time zone")]
        public DateTime? CreateDate { get; set; }
        public float? AmountIn { get; set; }
        public float? BuyBalance { get; set; }
        public float? SellBalance { get; set; }
        public double? LastOperProfit { get; set; }
        public double? DaysAction { get; set; }
        public string LastOper { get; set; }
        public string OperType { get; set; }
        public float? DirtyProfit { get; set; }
        public double? DirtyMonthProfitCash { get; set; }
        public double? DirtyMonthProfitPercentage { get; set; }
        public double? ClearProfit { get; set; }
        public double? ClearMonthProfitCash { get; set; }
        public double? ClearMonthProfitPercentage { get; set; }
    }
}
