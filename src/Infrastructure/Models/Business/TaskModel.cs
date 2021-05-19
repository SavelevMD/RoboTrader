using System;

namespace Models.Business
{
    public class TaskModel
    {
        public int Id { get; set; }
        public string TaskName { get; set; }
        public string Body { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsActive { get; set; }
    }
}
