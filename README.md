# TodoApp

Konzolová aplikace pro správu osobních úkolů napsaná v C# (.NET).

## Popis

Tato aplikace umožňuje spravovat úkoly v konzoli.  
Úkoly se ukládají do JSON souboru a po restartu aplikace zůstávají zachovány.

---


## Funkce

- přidání úkolu
- smazání úkolu
- označení úkolu jako hotového
- zobrazení seznamu úkolů
- filtrování úkolů (hotové / nehotové / podle priority)
- řazení úkolů
- statistiky
- ukládání dat do souboru
- validace vstupů a ošetření chyb

---

## Ovládání

- `add` – přidat úkol
- `list` – zobrazit všechny úkoly
- `done <id>` – označit úkol jako hotový
- `delete <id>` – smazat úkol
- `filter <done|pending|high|medium|low>` – filtrování
- `sort <priority|title>` – řazení
- `stats` – statistiky
- `help` – nápověda
- `exit` – ukončení programu

---

## Spuštění


```bash

dotnet run
```

```Save
 Pokud jste si stáhli projekt a otevřeli ho v JetBrains, klikněte na zelené tlačítko Run
```
---
## Použití AI

Při tvorbě projektu jsem využíval AI jako pomocný nástroj.  
AI mi pomohla rozdělit projekt do jednotlivých částí, navrhnout strukturu složek a třídy, ve kterých jsem následně psal vlastní kód.


