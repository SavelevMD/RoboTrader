using System;

namespace Models.Business
{
    public class TasksHistoryModel
    {
        public long Id { get; set; }
        public string TaskName { get; set; }
        public string State { get; set; }
        public DateTime TimeOfChangeState { get; set; }
        public int TaskId { get; set; }
        public TaskModel Task { get; set; }
        public string StateJson { get; set; }
    }
}
