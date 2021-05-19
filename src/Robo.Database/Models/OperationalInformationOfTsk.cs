using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Robo.Database.Models
{
    [Table("operational_information_of_tsk")]
    public partial class OperationalInformationOfTsk
    {
        [Required]
        [Column("task_name", TypeName = "character varying")]
        public string TaskName { get; set; }
        [Required]
        [Column("curr_pair_name", TypeName = "character varying")]
        public string CurrPairName { get; set; }
        [Required]
        [Column("op_data", TypeName = "json")]
        public string OpData { get; set; }
    }
}
