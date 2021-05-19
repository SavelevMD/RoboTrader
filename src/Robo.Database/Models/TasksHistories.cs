using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Robo.Database.Models
{
    [Table("tasks_histories")]
    public partial class TasksHistories
    {
        [Key]
        public long Id { get; set; }
        public string TaskName { get; set; }
        public string State { get; set; }
        public DateTime TimeOfChangeState { get; set; }
        public int TaskId { get; set; }
        [Column(TypeName = "jsonb")]
        public string StateJson { get; set; }

        [ForeignKey(nameof(TaskId))]
        [InverseProperty(nameof(Tasks.TasksHistories))]
        public virtual Tasks Task { get; set; }
    }
}
