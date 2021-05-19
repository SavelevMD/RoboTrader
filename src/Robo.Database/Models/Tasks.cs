using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Robo.Database.Models
{
    [Table("tasks")]
    public partial class Tasks
    {
        public Tasks()
        {
            TasksHistories = new HashSet<TasksHistories>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("body", TypeName = "json")]
        public string Body { get; set; }
        [Column("create_date", TypeName = "timestamp with time zone")]
        public DateTime CreateDate { get; set; }
        [Column("is_active")]
        public bool IsActive { get; set; }
        [Required]
        [Column("task_name")]
        public string TaskName { get; set; }

        [InverseProperty("Task")]
        public virtual ICollection<TasksHistories> TasksHistories { get; set; }
    }
}
