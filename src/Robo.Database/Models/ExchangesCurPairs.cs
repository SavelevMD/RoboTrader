using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Robo.Database.Models
{
    [Table("exchanges_cur_pairs")]
    public partial class ExchangesCurPairs
    {
        [Column("id")]
        public long Id { get; set; }
        [Column("exchange_id")]
        public long ExchangeId { get; set; }
        [Column("ext_id")]
        [StringLength(10)]
        public string ExtId { get; set; }
        [Required]
        [Column("name")]
        [StringLength(25)]
        public string Name { get; set; }
        [Column("status")]
        public int? Status { get; set; }
    }
}
