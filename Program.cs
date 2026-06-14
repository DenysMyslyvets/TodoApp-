using TodoApp.Services;
using TodoApp.UI;

var repository = new JsonTaskRepository("tasks.json");
var service = new TaskService(repository);
var ui = new ConsoleUI(service);

ui.Run();
