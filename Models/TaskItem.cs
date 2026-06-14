namespace TodoApp.Models;

public enum Priority
{
    Low,
    Medium,
    High
}

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Priority Priority { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
     public DateTime? CompletedAt { get; set; }

    public override string ToString()
    {
         string status = IsCompleted ? "[X]" : "[ ]";
        string priority = Priority switch
        {
            Priority.High   => "[!!!]",
            Priority.Medium => "[!! ]",
            Priority.Low    => "[!  ]",
            _               => "[   ]"
        };
        return status + " " + priority + " #" + Id + " " + Title;
    }
}
