using System.Text.Json;
using TodoApp.Exceptions;
using TodoApp.Models;

namespace TodoApp.Services;

public class JsonTaskRepository : ITaskRepository
{
    private readonly string _filePath;
    private List<TaskItem> _tasks;
    private int _nextId;

    private static readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true,
        Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
    };

    public JsonTaskRepository(string filePath = "tasks.json")
    {
        _filePath = filePath;
        _tasks = Load();
        _nextId = _tasks.Count > 0 ? _tasks.Max(t => t.Id) + 1 : 1;
    }

    public IEnumerable<TaskItem> GetAll()
    {
        return _tasks;
    }

    public TaskItem? GetById(int id)
    {
        return _tasks.FirstOrDefault(t => t.Id == id);
    }

    public void Add(TaskItem task)
    {
        task.Id = _nextId++;
        task.CreatedAt = DateTime.Now;
        _tasks.Add(task);
        Save();
    }

    public void Update(TaskItem task)
    {
        int index = _tasks.FindIndex(t => t.Id == task.Id);
        if (index == -1)
            throw new TaskNotFoundException(task.Id);

        _tasks[index] = task;
        Save();
    }

    public void Delete(int id)
    {
        TaskItem? task = _tasks.FirstOrDefault(t => t.Id == id)
            ?? throw new TaskNotFoundException(id);

        _tasks.Remove(task);
        Save();
    }

    public void Save()
    {
        try
        {
            string json = JsonSerializer.Serialize(_tasks, _options);
            File.WriteAllText(_filePath, json);
        }
        catch (IOException ex)
        {
             Console.WriteLine("Chyba při ukládání: " + ex.Message);
        }
    }

    private List<TaskItem> Load()
    {
        try
        {
            if (!File.Exists(_filePath))
                return new List<TaskItem>();

            string json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<TaskItem>>(json, _options) ?? new List<TaskItem>();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Chyba při načítání dat: " + ex.Message);
            return new List<TaskItem>();
        }
    }
}
