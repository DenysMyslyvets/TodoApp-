using TodoApp.Services;
using TodoApp.UI;

// Vstupní bod aplikace – zde se skládají všechny vrstvy dohromady

var repository = new JsonTaskRepository("tasks.json"); // Datová vrstva (JSON soubor)
var service = new TaskService(repository);             // Aplikační logika
var ui = new ConsoleUI(service);                       // Uživatelské rozhraní (CLI)

// Spuštění aplikace
ui.Run();