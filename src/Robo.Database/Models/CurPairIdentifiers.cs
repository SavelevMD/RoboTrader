using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Robo.Database.Models
{
    [Table("cur_pair_identifiers")]
    public partial class CurPairIdentifiers
    {
        [Column("cur_id_1")]
        public int? CurId1 { get; set; }
        [Column("cur_id_2")]
        public int? CurId2 { get; set; }
        [Required]
        [Column("curr_name", TypeName = "character varying")]
        public string CurrName { get; set; }
        [Required]
        [Column("tech_analysis_link", TypeName = "character varying")]
        public string TechAnalysisLink { get; set; }
        [Column("cid")]
        public int? Cid { get; set; }
    }
}
