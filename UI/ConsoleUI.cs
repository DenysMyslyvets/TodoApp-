using TodoApp.Exceptions;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp.UI;

public class ConsoleUI
{
    private readonly TaskService _service;
    private bool _running = true;

    public ConsoleUI(TaskService service)
    {
        _service = service;
    }

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

    private void CmdAdd()
    {
        Console.Write("Název úkolu: ");
        string title = Console.ReadLine() ?? string.Empty;

        Console.Write("Popis (nepovinné): ");
        string description = Console.ReadLine() ?? string.Empty;

        Priority priority = AskPriority();

        var task = _service.AddTask(title, description, priority);
        PrintSuccess("Úkol #" + task.Id + " '" + task.Title + "' byl přidán.");
    }

    private void CmdList()
    {
        var tasks = _service.GetAll().ToList();
        PrintTaskList(tasks, "Všechny úkoly");
    }

    private void CmdDone(string args)
    {
        if (!int.TryParse(args, out int id))
        {
            PrintError("Zadejte platné ID. Příklad: done 3");
            return;
        }

        _service.MarkDone(id);
        PrintSuccess("Úkol #" + id + " byl označen jako splněný.");
    }

    private void CmdDelete(string args)
    {
        if (!int.TryParse(args, out int id))
        {
            PrintError("Zadejte platné ID. Příklad: delete 3");
            return;
        }

        Console.Write("Opravdu smazat úkol #" + id + "? (a/n): ");
        string confirm = Console.ReadLine()?.Trim().ToLower() ?? "n";
        if (confirm != "a")
        {
            Console.WriteLine("Zrušeno.");
            return;
        }

        _service.DeleteTask(id);
        PrintSuccess("Úkol #" + id + " byl smazán.");
    }

    private void CmdFilter(string args)
    {
        switch (args.ToLower())
        {
            case "done":
                PrintTaskList(_service.GetByStatus(true).ToList(), "Splněné úkoly");
                break;
            case "pending":
                PrintTaskList(_service.GetByStatus(false).ToList(), "Nesplněné úkoly");
                break;
            case "high":
                PrintTaskList(_service.GetByPriority(Priority.High).ToList(), "Vysoká priorita");
                break;
            case "medium":
                PrintTaskList(_service.GetByPriority(Priority.Medium).ToList(), "Střední priorita");
                break;
            case "low":
                PrintTaskList(_service.GetByPriority(Priority.Low).ToList(), "Nízká priorita");
                break;
            default:
                PrintError("Možnosti: done | pending | high | medium | low");
                break;
        }
    }

    private void CmdSort(string args)
    {
        switch (args.ToLower())
        {
            case "priority":
                PrintTaskList(_service.GetSortedByPriority().ToList(), "Seřazeno podle priority");
                break;
            case "title":
                PrintTaskList(_service.GetSortedByTitle().ToList(), "Seřazeno abecedně");
                break;
            default:
                PrintError("Možnosti: priority | title");
                break;
        }
    }

    private void CmdStats()
    {
        var (total, done, pending) = _service.GetStats();
        Console.WriteLine();
        Console.WriteLine("=== Statistiky ===");
        Console.WriteLine("Celkem úkolů:  " + total);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Splněno:       " + done);
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Zbývá:         " + pending);
        Console.ResetColor();
        if (total > 0)
        {
            int percent = (int)((double)done / total * 100);
            Console.WriteLine("Hotovo:        " + percent + "%");
        }
        Console.WriteLine();
    }

    private void CmdHelp()
    {
        Console.WriteLine();
        Console.WriteLine("=== Příkazy ===");
        Console.WriteLine("  add              Přidat nový úkol");
        Console.WriteLine("  list             Zobrazit všechny úkoly");
        Console.WriteLine("  done <id>        Označit úkol jako splněný");
        Console.WriteLine("  delete <id>      Smazat úkol");
        Console.WriteLine("  filter <volba>   Filtrovat: done|pending|high|medium|low");
        Console.WriteLine("  sort <volba>     Seřadit: priority|title");
        Console.WriteLine("  stats            Zobrazit statistiky");
        Console.WriteLine("  help             Zobrazit tuto nápovědu");
        Console.WriteLine("  exit             Ukončit program");
        Console.WriteLine();
    }

    private void CmdExit()
    {
        Console.WriteLine("Na shledanou!");
        _running = false;
    }

    private Priority AskPriority()
    {
        while (true)
        {
            Console.Write("Priorita (1-Nízká, 2-Střední, 3-Vysoká) [výchozí 2]: ");
            string input = Console.ReadLine()?.Trim() ?? "";

            if (string.IsNullOrEmpty(input)) return Priority.Medium;

            if (int.TryParse(input, out int choice))
            {
                if (choice == 1) return Priority.Low;
                if (choice == 2) return Priority.Medium;
                if (choice == 3) return Priority.High;
            }

            PrintError("Zadejte 1, 2 nebo 3.");
        }
    }

    private void PrintTaskList(List<TaskItem> tasks, string title)
    {
        Console.WriteLine();
        Console.WriteLine("=== " + title + " (" + tasks.Count + ") ===");

        if (tasks.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("  Seznam je prázdný.");
            Console.ResetColor();
        }
        else
        {
            foreach (var task in tasks)
            {
                if (task.IsCompleted)
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                else if (task.Priority == Priority.High)
                    Console.ForegroundColor = ConsoleColor.Red;
                else if (task.Priority == Priority.Medium)
                    Console.ForegroundColor = ConsoleColor.Yellow;
                else
                    Console.ForegroundColor = ConsoleColor.White;

                Console.WriteLine("  " + task);
                Console.ResetColor();

                if (!string.IsNullOrWhiteSpace(task.Description))
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("     " + task.Description);
                    Console.ResetColor();
                }
            }
        }
        Console.WriteLine();
    }

    private static void PrintSuccess(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("OK: " + msg);
        Console.ResetColor();
    }

    private static void PrintError(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Chyba: " + msg);
        Console.ResetColor();
    }
}
