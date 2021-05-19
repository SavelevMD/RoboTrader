using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Robo.Database.Models
{
    [Table("exchanges")]
    public partial class Exchanges
    {
        [Column("id")]
        public long Id { get; set; }
        [Column("code")]
        [StringLength(5)]
        public string Code { get; set; }
        [Required]
        [Column("name")]
        [StringLength(80)]
        public string Name { get; set; }
        [Column("status")]
        public int? Status { get; set; }
    }
}
