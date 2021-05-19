using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Robo.Database.Models
{
    [Table("cur_exch_identifers")]
    public partial class CurExchIdentifers
    {
        [Column("cur_id")]
        public int CurId { get; set; }
        [Required]
        [Column("name", TypeName = "character varying")]
        public string Name { get; set; }
        [Column("exch_id")]
        public int ExchId { get; set; }
    }
}
