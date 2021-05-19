using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Robo.Database.Models
{
    [Table("ledger")]
    public partial class Ledger
    {
        [Column("id")]
        public long Id { get; set; }
        [Column("oper_name", TypeName = "character varying")]
        public string OperName { get; set; }
        [Column("price")]
        public double? Price { get; set; }
        [Column("balance_in")]
        public double? BalanceIn { get; set; }
        [Column("currency_from_id")]
        public long? CurrencyFromId { get; set; }
        [Column("balance_out")]
        public double? BalanceOut { get; set; }
        [Column("currency_to_id")]
        public long? CurrencyToId { get; set; }
        [Column("time_operation", TypeName = "timestamp with time zone")]
        public DateTime? TimeOperation { get; set; }
        [Column("uid_thread")]
        public Guid UidThread { get; set; }
        [Column("condition", TypeName = "character varying")]
        public string Condition { get; set; }
        [Column("cond_for_op", TypeName = "character varying")]
        public string CondForOp { get; set; }
        [Column("cur_pair", TypeName = "character varying")]
        public string CurPair { get; set; }
    }
}
