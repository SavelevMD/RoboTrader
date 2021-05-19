using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Robo.Database.Models
{
    [Table("users")]
    public partial class Users
    {
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("name")]
        [StringLength(10)]
        public string Name { get; set; }
        [Required]
        [Column("pswd")]
        [StringLength(10)]
        public string Pswd { get; set; }
    }
}
