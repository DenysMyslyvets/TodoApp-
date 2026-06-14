namespace TodoApp.Models;

// Rozhraní definující operace pro práci s úkoly
public interface ITaskRepository
{
    IEnumerable<TaskItem> GetAll();
    TaskItem? GetById(int id);
    void Add(TaskItem task);
    void Update(TaskItem task);
    void Delete(int id);
    void Save();
}