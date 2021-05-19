using System.ComponentModel.DataAnnotations.Schema;

namespace Robo.Database.Models
{
    [Table("task_scan")]
    public partial class TaskScan
    {
        public string ReqCandleTime { get; set; }
        public string ExchCandleTime { get; set; }
        [Column("currentprice", TypeName = "numeric(16,12)")]
        public decimal? Currentprice { get; set; }
        [Column("lastprice", TypeName = "numeric(16,12)")]
        public decimal? Lastprice { get; set; }
        public int? CurrentState { get; set; }
        [Column("cond_98")]
        public bool? Cond98 { get; set; }
        [Column("cond_97")]
        public bool? Cond97 { get; set; }
        [Column("cond_96")]
        public bool? Cond96 { get; set; }
        [Column("cond_95")]
        public bool? Cond95 { get; set; }
        [Column("i_RSI_14", TypeName = "numeric(16,12)")]
        public decimal? IRsi14 { get; set; }
        [Column("rr", TypeName = "numeric")]
        public decimal? Rr { get; set; }
    }
}
