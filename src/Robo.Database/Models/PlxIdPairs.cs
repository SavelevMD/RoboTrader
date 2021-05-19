using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Robo.Database.Models
{
    [Table("plx_id_pairs")]
    public partial class PlxIdPairs
    {
        [Column("id")]
        public long Id { get; set; }
        [Required]
        [Column("name", TypeName = "character varying")]
        public string Name { get; set; }
    }
}
