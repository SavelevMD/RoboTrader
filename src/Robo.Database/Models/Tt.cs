using System.ComponentModel.DataAnnotations.Schema;

namespace Robo.Database.Models
{
    [Table("tt")]
    public partial class Tt
    {
        public string TaskName { get; set; }
        public string CurrencyPair { get; set; }
        public string DealTime { get; set; }
        public float? DealPrice { get; set; }
        public int? CurrentState { get; set; }
        [Column(TypeName = "jsonb")]
        public string Indicators { get; set; }
        public float? AmountIn { get; set; }
        public float? BuyBalance { get; set; }
        public float? SellBalance { get; set; }
        public float? LimitTradeAmount { get; set; }
        public float? SavedValue { get; set; }
        public long? OperRowNum { get; set; }
        public long? RowNum { get; set; }
        [Column(TypeName = "jsonb")]
        public string StateJson { get; set; }
    }
}
