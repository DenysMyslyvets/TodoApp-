namespace TodoApp.Exceptions;

// Vlastní výjimka vyvolaná v případě, že úkol s daným ID neexistuje
public class TaskNotFoundException : Exception
{
    // ID nenalezeného úkolu
    public int TaskId { get; }

    public TaskNotFoundException(int taskId)
        : base("Úkol s ID " + taskId + " nebyl nalezen.")
    {
        TaskId = taskId;
    }
}