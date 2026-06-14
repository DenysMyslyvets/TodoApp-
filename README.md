# TODO Správce úkolů

Konzolová aplikace pro správu osobních úkolů s ukládáním dat do JSON souboru. Napsáno v C# (.NET 8).

## Popis aplikace

Aplikace umožňuje přidávat úkoly, označovat je jako splněné, mazat je, filtrovat a řadit. Data se automaticky ukládají do souboru `tasks.json` a po restartu aplikace jsou stále k dispozici.

## Spuštění

```bash
dotnet run
```

## Ovládání

Po spuštění zadávejte příkazy do řádku `todo>`:

| Příkaz | Popis | Příklad |
|---|---|---|
| `add` | Přidat nový úkol | `add` |
| `list` | Zobrazit všechny úkoly | `list` |
| `done <id>` | Označit úkol jako splněný | `done 3` |
| `delete <id>` | Smazat úkol | `delete 5` |
| `filter <volba>` | Filtrovat úkoly | `filter high` |
| `sort <volba>` | Seřadit úkoly | `sort priority` |
| `stats` | Statistiky | `stats` |
| `help` | Nápověda | `help` |
| `exit` | Ukončit program | `exit` |

### Možnosti filtru
- `done` — splněné úkoly
- `pending` — nesplněné úkoly
- `high` / `medium` / `low` — podle priority

### Možnosti řazení
- `priority` — podle priority (od nejvyšší)
- `title` — abecedně

## Struktura projektu

```
TodoApp/
├── Models/
│   ├── TaskItem.cs            # Model úkolu s enum Priority
│   └── ITaskRepository.cs     # Rozhraní pro repozitář
├── Services/
│   ├── JsonTaskRepository.cs  # Ukládání do JSON souboru
│   └── TaskService.cs         # Logika aplikace (filtrování, řazení, LINQ)
├── Exceptions/
│   └── TaskNotFoundException.cs  # Vlastní výjimka
├── UI/
│   └── ConsoleUI.cs           # REPL rozhraní s barvami konzole
├── Program.cs                 # Vstupní bod
└── README.md
```

## Použité OOP koncepty

- **Rozhraní** — `ITaskRepository` definuje kontrakt pro práci s daty
- **Zapouzdření** — všechna pole jsou privátní, přístup přes vlastnosti
- **Oddělení zodpovědností** — samostatné třídy pro data, logiku a UI
- **Vlastní výjimka** — `TaskNotFoundException`
- **LINQ** — `Where`, `OrderBy`, `OrderByDescending` pro filtrování a řazení

## Použití AI

Při práci na projektu jsem využíval umělou inteligenci jako pomoc při psaní kódu.

### Použité prompty

1. *"přelož mi toto zadání"* — přeložení zadání z češtiny do ruštiny pro lepší pochopení
2. *"jaké zadání bude nejjednodušší"* — pomoc při výběru zadání
3. *"vytvoř základní strukturu projektu pro TODO aplikaci v C#"* — vygenerování kostry projektu
4. *"přidej filtrování a řazení pomocí LINQ"* — rozšíření funkcionality
5. *"oprav texty v konzoli na češtinu"* — úprava textů v aplikaci

Vygenerovaný kód jsem procházel, upravoval a rozumím tomu, jak funguje.
