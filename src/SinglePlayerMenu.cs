using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;

namespace NEA___Maze_Wiz
{
    class SinglePlayerMenu
    {
        private int[,] maze;
        private int playerX, playerY;
        private const char PlayerSymbol = 'O';
        private DateTime startTime;
        private bool hintUsed = false;
        private List<(int x, int y)> hintPath = null;

        public SinglePlayerMenu(int[,] maze)
        {
            this.maze = maze;
            (playerX, playerY) = FindStartPoint();
        }

        public void Display()
        {
            Console.Clear();
            Console.WriteLine("=== Single Player Mode ===");
            Console.WriteLine("Use arrow keys to move. Press Q to quit. Press H for a hint (one-time use).");
            Console.WriteLine("Centering maze...");

            PrintCentered("Loading maze...");
            Thread.Sleep(1000);

            Console.Clear();
            Console.WriteLine("=== Single Player Mode ===");
            Console.WriteLine("Use arrow keys to move. Press Q to quit. Press H for a hint (one-time use).");

            PrintMazeWithPlayer();

            // Timer starts
            startTime = DateTime.Now;

            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                Console.SetCursorPosition(0, 2);
                Console.Write(new string(' ', Console.WindowWidth)); //to remove extra characters, replaces them with ' '

                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        TryMovePlayer(playerX, playerY - 1);
                        break;
                    case ConsoleKey.DownArrow:
                        TryMovePlayer(playerX, playerY + 1);
                        break;
                    case ConsoleKey.LeftArrow:
                        TryMovePlayer(playerX - 1, playerY);
                        break;
                    case ConsoleKey.RightArrow:
                        TryMovePlayer(playerX + 1, playerY);
                        break;
                    case ConsoleKey.H:
                        if (!hintUsed)
                        {
                            Hint();
                            hintUsed = true;
                        }
                        break;
                    case ConsoleKey.Q:
                        Console.WriteLine("Exiting...");
                        Thread.Sleep(1000);
                        MainMenu mainMenu = new MainMenu();
                        mainMenu.Display();
                        return;
                    default:
                        continue;
                }

                PrintMazeWithPlayer();

                if (playerX == maze.GetLength(1) - 2 && playerY == maze.GetLength(0) - 2)
                {
                    TimeSpan elapsedTime = DateTime.Now - startTime;
                    Console.WriteLine($"Congratulations! You reached the end of the maze in {elapsedTime.TotalSeconds:F2} seconds.");
                    Console.WriteLine("Press SPACE key to return to the main menu.");
                    while (Console.ReadKey(true).Key != ConsoleKey.Spacebar)
                    {

                    }
                        MainMenu mainMenu = new MainMenu();
                       mainMenu.Display();
                       return;
                  }
             }
        }

        private void PrintMazeWithPlayer()
        {
            int mazeWidth = maze.GetLength(1) * 2;

             int consoleWidth = Console.WindowWidth;
             int leftMargin = (consoleWidth - mazeWidth) / 2;

            Console.SetCursorPosition(0, 0);
            Console.WriteLine("=== Single Player Mode ===");
            Console.WriteLine("Use arrow keys to move. Press Q to quit. Press H for a hint (one-time use).");

            for (int i = 0; i < maze.GetLength(0); i++)
            {
                Console.SetCursorPosition(leftMargin, Console.CursorTop);

                for (int j = 0; j < maze.GetLength(1); j++)
                {
                     if (i == playerY && j == playerX)
                     {
                         Console.Write(PlayerSymbol + " ");
                     }
                     else if (hintPath != null && hintPath.Contains((j, i)))
                     {
                         Console.Write("+" + " ");
                     }
                     else
                     {
                         Console.Write(GetCellRepresentation(maze[i, j]));
                     }
                }
                Console.WriteLine();
            }

            if (hintUsed)
            {
                hintPath = null;
              }
        }

        private void PrintCentered(string text)
        {
            int consoleWidth = Console.WindowWidth;
            int leftMargin = (consoleWidth - text.Length) / 2;
            Console.SetCursorPosition(leftMargin, Console.CursorTop);
            Console.WriteLine(text);
        }

        private string GetCellRepresentation(int cell)
        {
            switch (cell)
            {
                case 0: return "██";
                case 1: return "  ";
                case 2: return "S ";
                case 3: return "E ";
                default: return "  ";
            }
        }

        private (int, int) FindStartPoint()
        {
            for (int i = 0; i < maze.GetLength(0); i++)
            {
                 for (int j = 0; j < maze.GetLength(1); j++)
                 {
                     if (maze[i, j] == 2)
                     {
                         return (j, i);
                     }
                 }
            }
            return (-1, -1); // Return invalid coordinates if start point not found
        }

        private void TryMovePlayer(int newX, int newY)
        {
            // Check if the new position is within bounds and is a valid path (not a wall)
            if (newX >= 0 && newX < maze.GetLength(1) &&
                 newY >= 0 && newY < maze.GetLength(0) &&
                 maze[newY, newX] != 0)
            {
                      playerX = newX;
                     playerY = newY;
                }
           }

           public void Hint()
           {
               SolveMazeMenu solver = new SolveMazeMenu(maze);

                (int startX, int startY) = solver.FindStartPoint();
                (int endX, int endY) = solver.FindEndPoint();

                if (startX == -1 || startY == -1 || endX == -1 || endY == -1)
                {
                     Console.WriteLine("Invalid maze. Cannot find start or end point.");
                     return;
                }

            bool[,] visited = new bool[maze.GetLength(0), maze.GetLength(1)];

            if (solver.DFS(startX, startY, endX, endY, visited, true)) //runs DFS to find a valid path and highlights it temporarily
            {

            }
            else
            {
                Console.WriteLine("No path found.");
            }
            PrintMazeWithPlayer();
        }
    }
}
