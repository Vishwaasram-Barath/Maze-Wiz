# Maze Wiz (A-level Computer Science NEA)

A console-based maze game written in C#. It generates mazes, lets you solve them yourself or watch DFS, BFS or A* solve them, and includes a chase mode with AI opponents, hints and a power-up. Mazes can be saved to and loaded from files.

## How to run it (Visual Studio Code)

1. Install the [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) (the SDK, not just the runtime).
2. Install [Visual Studio Code](https://code.visualstudio.com/) and add the **C# Dev Kit** extension (Extensions panel, `Ctrl+Shift+X`).
3. Download this repository (green **Code** button, then **Download ZIP** and unzip it) or clone it:
   ```
   git clone https://github.com/Vishwaasram-Barath/Maze-Wiz.git
   ```
4. In VS Code choose **File > Open Folder** and open the project folder. Click "Yes, I trust the authors" if asked.
5. Press **F5** to build and start the game. It opens in its own terminal window.
6. Maximise that window before generating a maze so large mazes fit on screen.

If F5 asks you to choose an environment, pick **.NET Core** / **Maze Wiz**. The game needs a real terminal for keyboard input, which is why `.vscode/launch.json` is set to `externalTerminal`.

## Controls

| Key | Action |
| --- | --- |
| Arrow keys | Move the player |
| `H` | Hint (single player, one use) |
| `F` | Pass through a wall (chase mode, one use) |
| `Q` | Quit to the main menu |
| `SPACE` | Continue / return to the menu |

## Project layout

```
src/        C# source, one class per file
.vscode/    VS Code build and run settings
```
