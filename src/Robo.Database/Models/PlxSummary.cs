using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Robo.Database.Models
{
    [Table("plx_summary")]
    public partial class PlxSummary
    {
        [Required]
        [Column("curr_name", TypeName = "character varying")]
        public string CurrName { get; set; }
        [Required]
        [Column("summary", TypeName = "character varying")]
        public string Summary { get; set; }
        [Required]
        [Column("ma", TypeName = "character varying")]
        public string Ma { get; set; }
        [Required]
        [Column("ti", TypeName = "character varying")]
        public string Ti { get; set; }
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("period", TypeName = "character varying")]
        public string Period { get; set; }
        [Column("push_req_time", TypeName = "timestamp with time zone")]
        public DateTime PushReqTime { get; set; }
    }
}
