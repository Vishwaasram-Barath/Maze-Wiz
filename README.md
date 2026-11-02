# Maze Wiz (A-level Computer Science NEA)

A console-based maze game written in C#. It generates mazes, lets you solve them yourself or watch an
A* search solve them, and includes a chase mode with AI opponents, hints and power-ups. Mazes can be
saved to and loaded from files.

## Run
Requires the [.NET 10.0 SDK](https://dotnet.microsoft.com/download).

```bash
dotnet run
```

Use a real terminal window (not an IDE output pane): the program uses `Console.ReadKey` and cursor control.

## Layout
```
src/    C# source, one class per file
docs/   NEA write-up
```
