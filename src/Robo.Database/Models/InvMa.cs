using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Robo.Database.Models
{
    [Table("inv_ma")]
    public partial class InvMa
    {
        [Column("id")]
        public long Id { get; set; }
        [Column("period", TypeName = "character varying")]
        public string Period { get; set; }
        [Column("sValue")]
        public double? SValue { get; set; }
        [Column("sAction", TypeName = "character varying")]
        public string SAction { get; set; }
        [Column("exValue")]
        public double? ExValue { get; set; }
        [Column("exAction", TypeName = "character varying")]
        public string ExAction { get; set; }
        [Column("time_shot", TypeName = "timestamp(6) with time zone")]
        public DateTime? TimeShot { get; set; }
    }
}
