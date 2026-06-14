using TodoApp.Exceptions;
using TodoApp.Models;

namespace TodoApp.Services;

public class TaskService
{
    private readonly ITaskRepository _repository;

    public TaskService(ITaskRepository repository)
    {
        _repository = repository;
    }

    public TaskItem AddTask(string title, string description, Priority priority)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Název úkolu nesmí být prázdný.");

        var task = new TaskItem
        {
            Title = title.Trim(),
            Description = description.Trim(),
            Priority = priority,
            IsCompleted = false
        };

        _repository.Add(task);
        return task;
    }

    public void MarkDone(int id)
    {
        TaskItem task = _repository.GetById(id) ?? throw new TaskNotFoundException(id);
        task.IsCompleted = true;
        task.CompletedAt = DateTime.Now;
        _repository.Update(task);
    }

    public void DeleteTask(int id)
    {
        _ = _repository.GetById(id) ?? throw new TaskNotFoundException(id);
        _repository.Delete(id);
    }

    public IEnumerable<TaskItem> GetAll()
    {
        return _repository.GetAll();
    }

    public IEnumerable<TaskItem> GetByStatus(bool completed)
    {
        return _repository.GetAll().Where(t => t.IsCompleted == completed);
    }

    public IEnumerable<TaskItem> GetByPriority(Priority priority)
    {
        return _repository.GetAll().Where(t => t.Priority == priority);
    }

    public IEnumerable<TaskItem> GetSortedByPriority()
    {
         return _repository.GetAll().OrderByDescending(t => t.Priority);
    }

    public IEnumerable<TaskItem> GetSortedByTitle()
    {
        return _repository.GetAll().OrderBy(t => t.Title);
    }

     public (int total, int done, int pending) GetStats()
    {
        var all = _repository.GetAll().ToList();
        int done = all.Count(t => t.IsCompleted);
        return (all.Count, done, all.Count - done);
    }
}
