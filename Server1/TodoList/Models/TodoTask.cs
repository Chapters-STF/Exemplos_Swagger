namespace TodoList.Models
{
    public class TodoTask
    {
        /// <summary>Todo Task ID</summary>

        public int TodoTaskId { get; set; }
        /// <summary>Whatever you would use to describe the task</summary>
        public string Desc { get; set; }
        /// <summary>The task Status ( To do, In Progress, Done)</summary>
        public string Status { get; set; }

    }

}