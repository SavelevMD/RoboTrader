using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Robo.Database.Models
{
    [Table("tasks_ledger")]
    public partial class TasksLedger
    {
        public int? TaskId { get; set; }
        [Column(TypeName = "timestamp with time zone")]
        public DateTime? CreateDate { get; set; }
        public string TaskName { get; set; }
        public string CurrencyPair { get; set; }
        public string DealTime { get; set; }
        public float? DealPrice { get; set; }
        public int? CurrentState { get; set; }
        public float? AmountIn { get; set; }
        public float? BuyBalance { get; set; }
        public float? SellBalance { get; set; }
        public float? LimitTradeAmount { get; set; }
        public float? SavedValue { get; set; }
        public long? OperRowNum { get; set; }
        public string OperType { get; set; }
    }
}
