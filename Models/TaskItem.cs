namespace TodoApp.Models;

// Úrovně priority úkolu
public enum Priority
{
    Low,
    Medium,
    High
}

// Model reprezentující jeden úkol
public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Priority Priority { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }

    // Vrací textovou reprezentaci úkolu pro výpis v konzoli
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