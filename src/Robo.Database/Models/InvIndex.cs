using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Robo.Database.Models
{
    [Table("inv_index")]
    public partial class InvIndex
    {
        [Column("id")]
        public long Id { get; set; }
        [Column("name", TypeName = "character varying")]
        public string Name { get; set; }
        [Column("value")]
        public double? Value { get; set; }
        [Column("action", TypeName = "character varying")]
        public string Action { get; set; }
        [Column("time_shot", TypeName = "timestamp with time zone")]
        public DateTime? TimeShot { get; set; }
    }
}
