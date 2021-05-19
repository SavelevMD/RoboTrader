using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Robo.Database.Models
{
    [Table("extremums_lines")]
    public partial class ExtremumsLines
    {
        [Column("id")]
        public long Id { get; set; }
        [Column("currency_id")]
        public long CurrencyId { get; set; }
        [Column("start_date", TypeName = "timestamp with time zone")]
        public DateTime? StartDate { get; set; }
        [Column("end_date", TypeName = "timestamp with time zone")]
        public DateTime? EndDate { get; set; }
        [Required]
        [Column("line_name", TypeName = "character varying")]
        public string LineName { get; set; }
        [Column("trend")]
        public char Trend { get; set; }
        [Column("counter")]
        public int? Counter { get; set; }
    }
}
