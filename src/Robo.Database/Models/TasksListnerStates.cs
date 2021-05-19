using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Robo.Database.Models
{
    [Table("tasks_listner_states")]
    public partial class TasksListnerStates
    {
        [Key]
        public long Id { get; set; }
        public int TaskId { get; set; }
        public DateTime ReceiptTime { get; set; }
        [Column(TypeName = "jsonb")]
        public string State { get; set; }
    }
}
