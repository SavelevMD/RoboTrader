using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Robo.Database.Models
{
    [Table("tasks_lists")]
    public partial class TasksLists
    {
        public int? TaskId { get; set; }
        public string TaskName { get; set; }
        public string CurrencyPair { get; set; }
        [Column(TypeName = "timestamp with time zone")]
        public DateTime? CreateDate { get; set; }
        public long? OperationsCount { get; set; }
        public DateTime? LastOperTime { get; set; }
        public string KillCommand { get; set; }
    }
}
