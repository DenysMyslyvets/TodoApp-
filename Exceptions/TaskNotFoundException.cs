namespace TodoApp.Exceptions;

public class TaskNotFoundException : Exception
{
    public int TaskId { get; }

    public TaskNotFoundException(int taskId)
        : base("Úkol s ID " + taskId + " nebyl nalezen.")
    {
        TaskId = taskId;
    }
}
