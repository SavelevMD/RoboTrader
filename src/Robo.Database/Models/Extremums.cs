using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Robo.Database.Models
{
    [Table("extremums")]
    public partial class Extremums
    {
        [Column("id")]
        public long Id { get; set; }
        [Column("extremum_value")]
        public double? ExtremumValue { get; set; }
        [Required]
        [Column("extremum_type")]
        [StringLength(25)]
        public string ExtremumType { get; set; }
        [Column("value_def_time", TypeName = "timestamp with time zone")]
        public DateTime? ValueDefTime { get; set; }
        [Column("line_id")]
        public long? LineId { get; set; }
    }
}
