using TodoApp.Exceptions;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp.UI;

// Konzolové uživatelské rozhraní aplikace
public class ConsoleUI
{
    private readonly TaskService _service;
    private bool _running = true;

    public ConsoleUI(TaskService service)
    {
        _service = service;
    }

    // Hlavní smyčka aplikace
    public void Run()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("=== TODO Správce úkolů ===");
        Console.ResetColor();
        Console.WriteLine("Napište 'help' pro seznam příkazů.\n");

        while (_running)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("todo> ");
            Console.ResetColor();

            string? input = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(input)) continue;

            string[] parts = input.Split(' ', 2);
            string command = parts[0].ToLower();
            string args = parts.Length > 1 ? parts[1] : string.Empty;

            try
            {
                ExecuteCommand(command, args);
            }
            catch (TaskNotFoundException ex)
            {
                PrintError(ex.Message);
            }
            catch (ArgumentException ex)
            {
                PrintError(ex.Message);
            }
            catch (Exception ex)
            {
                PrintError("Neočekávaná chyba: " + ex.Message);
            }
        }
    }

    // Zpracování příkazů
    private void ExecuteCommand(string command, string args)
    {
        switch (command)
        {
            case "add":    CmdAdd(); break;
            case "list":   CmdList(); break;
            case "done":   CmdDone(args); break;
            case "delete": CmdDelete(args); break;
            case "filter": CmdFilter(args); break;
            case "sort":   CmdSort(args); break;
            case "stats":  CmdStats(); break;
            case "help":   CmdHelp(); break;
            case "exit":   CmdExit(); break;
            default:
                PrintError("Neznámý příkaz '" + command + "'. Napište 'help'.");
                break;
        }
    }

    // Přidání úkolu
    private void CmdAdd()
    {
        Console.Write("Název úkolu: ");
        string title = Console.ReadLine() ?? string.Empty;

        Console.Write("Popis (nepovinné): ");
        string description = Console.ReadLine() ?? string.Empty;

        Priority priority = AskPriority();

        var task = _service.AddTask(title, description, priority);
        PrintSuccess("Úkol #" + task.Id + " byl přidán.");
    }

    // Zobrazení všech úkolů
    private void CmdList()
    {
        PrintTaskList(_service.GetAll().ToList(), "Všechny úkoly");
    }

    // Označení jako hotovo
    private void CmdDone(string args)
    {
        if (!int.TryParse(args, out int id))
        {
            PrintError("Zadejte platné ID.");
            return;
        }

        _service.MarkDone(id);
        PrintSuccess("Úkol #" + id + " dokončen.");
    }

    // Smazání úkolu
    private void CmdDelete(string args)
    {
        if (!int.TryParse(args, out int id))
        {
            PrintError("Zadejte platné ID.");
            return;
        }

        Console.Write("Opravdu smazat? (a/n): ");
        string confirm = Console.ReadLine()?.Trim().ToLower() ?? "n";
        if (confirm != "a")
        {
            Console.WriteLine("Zrušeno.");
            return;
        }

        _service.DeleteTask(id);
        PrintSuccess("Úkol smazán.");
    }

    // Filtrace
    private void CmdFilter(string args)
    {
        switch (args.ToLower())
        {
            case "done":
                PrintTaskList(_service.GetByStatus(true).ToList(), "Hotové");
                break;
            case "pending":
                PrintTaskList(_service.GetByStatus(false).ToList(), "Nehotové");
                break;
            case "high":
                PrintTaskList(_service.GetByPriority(Priority.High).ToList(), "High");
                break;
            case "medium":
                PrintTaskList(_service.GetByPriority(Priority.Medium).ToList(), "Medium");
                break;
            case "low":
                PrintTaskList(_service.GetByPriority(Priority.Low).ToList(), "Low");
                break;
            default:
                PrintError("Neplatný filtr.");
                break;
        }
    }

    // Třídění
    private void CmdSort(string args)
    {
        switch (args.ToLower())
        {
            case "priority":
                PrintTaskList(_service.GetSortedByPriority().ToList(), "Podle priority");
                break;
            case "title":
                PrintTaskList(_service.GetSortedByTitle().ToList(), "Podle názvu");
                break;
            default:
                PrintError("Neplatné třídění.");
                break;
        }
    }

    // Statistiky
    private void CmdStats()
    {
        var (total, done, pending) = _service.GetStats();
        Console.WriteLine($"Celkem: {total}, Hotovo: {done}, Zbývá: {pending}");
    }

    // Nápověda
    private void CmdHelp()
    {
        Console.WriteLine("add, list, done, delete, filter, sort, stats, help, exit");
    }

    // Ukončení
    private void CmdExit()
    {
        _running = false;
    }

    // Výběr priority
    private Priority AskPriority()
    {
        Console.Write("Priorita (1-3): ");
        string input = Console.ReadLine() ?? "";

        return input switch
        {
            "1" => Priority.Low,
            "3" => Priority.High,
            _ => Priority.Medium
        };
    }

    // Výpis seznamu
    private void PrintTaskList(List<TaskItem> tasks, string title)
    {
        Console.WriteLine("\n=== " + title + " ===");
        foreach (var t in tasks)
        {
            Console.WriteLine(t);
        }
    }

    // OK zpráva
    private static void PrintSuccess(string msg)
    {
        Console.WriteLine("OK: " + msg);
    }

    // Error zpráva
    private static void PrintError(string msg)
    {
        Console.WriteLine("Chyba: " + msg);
    }
}