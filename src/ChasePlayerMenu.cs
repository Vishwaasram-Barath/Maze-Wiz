using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;

namespace NEA___Maze_Wiz
{
    class ChasePlayerMenu
    {
        private int[,] maze;
        private int playerX, playerY, difficultylevel, numAI, aiMoveFrequency;
        private int playerMoves = 0;
        private const char AIsymbol = 'X';
        private const char PlayerSymbol = 'O';
        private char ChaseTrigger = '@';
        private List<(int x, int y)> aiPositions;
        private (int x, int y) chaseTriggerPosition;
         private bool powerUpUsed, powerUpActive, chaseTriggered = false;
        private (int x, int y) WallPosition = (-1, -1);
        public ChasePlayerMenu(int[,] maze)
        {
            this.maze = maze;
            (playerX, playerY) = FindStartPoint();
            aiPositions = new List<(int x, int y)>();
        }
        public void Display()
        {
            Console.Clear();

            Console.WriteLine("Choose difficulty level:");
            Console.WriteLine("1. Easy (AI moves every 5 moves you make)");
            Console.WriteLine("2. Medium (AI moves every 3 moves you make)");
            Console.WriteLine("3. Hard (AI moves every 1 move you make)");
            difficultylevel = int.Parse(Console.ReadLine());
            switch (difficultylevel)
            {
                case 1: aiMoveFrequency = 5; break;
                case 2: aiMoveFrequency = 3; break;
                case 3: aiMoveFrequency = 1; break;
                default: break;
            }

            Console.Write("Enter number of AI enemies (1-3): ");
            numAI = int.Parse(Console.ReadLine());
            switch (numAI)
            {
                case 1: numAI = 1; break;
                case 2: numAI = 2; break;
                case 3: numAI = 3; break;
                default: numAI = 1; break;
            }

            InitialiseAIPositions();

            chaseTriggerPosition = CalculateChaseTrigger();

            PlayChaseMode();
        }
        private void InitialiseAIPositions()
        {
            aiPositions.Add(FindStartPoint());
               Random random = new Random();
              for (int i = 1; i < numAI; i++)
              {
                  int x, y;
                  do
                  {
                      x = random.Next(maze.GetLength(1));
                      y = random.Next(maze.GetLength(0));

                   } while (maze[y, x] != 1 || aiPositions.Contains((x, y)));
                   aiPositions.Add((x, y));
              }
          }
          private (int x, int y) CalculateChaseTrigger()
          {
              SolveMazeMenu solver = new SolveMazeMenu(maze);
              (int startX, int startY) = solver.FindStartPoint();
              (int endX, int endY) = solver.FindEndPoint();

              List<(int, int)> path = solver.GetPath(startX, startY, endX, endY);
              if (path.Count > 0)
              {
                  int triggerPoint = (int)(path.Count * 0.9);
                  return path[triggerPoint];
              }
              else
              {
                  Console.WriteLine("No valid path found.");
              }
              return (-1, -1);
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
        private string GetCellRepresentation(int cell)
        {
            switch (cell)
            {
                case 0: return "██";
                case 1: return " ";
                case 2: return "S ";
                case 3: return "E ";
                default: return " ";
            }
        }
        private void PrintMaze()
        {
            int mazeWidth = maze.GetLength(1) * 2;
            int consoleWidth = Console.WindowWidth;
            int leftMargin = (consoleWidth - mazeWidth) / 2;
            Console.SetCursorPosition(0, 0);

            for (int i = 0; i < maze.GetLength(0); i++)
            {
                Console.SetCursorPosition(leftMargin, Console.CursorTop);

                for (int j = 0; j < maze.GetLength(1); j++)
                {
                     if (i == playerY && j == playerX)
                     {
                         Console.Write(PlayerSymbol + " ");
                     }
                     else if (aiPositions.Contains((j, i)))
                     {
                         Console.Write(AIsymbol + " ");
                     }
                     else if (i == chaseTriggerPosition.y && j == chaseTriggerPosition.x)
                     {
                         Console.ForegroundColor = ConsoleColor.Red;
                         Console.Write(ChaseTrigger + " ");
                         Console.ResetColor();
                     }
                     else
                     {
                         Console.Write(GetCellRepresentation(maze[i, j]));
                     }
                }
                   Console.WriteLine();
             }
        }

        private bool ValidMove(int x, int y)
        {
            if (powerUpActive && maze[y, x] == 0)
            {
                WallPosition = (x, y);
                return true;
            }
            return x >= 0 && x < maze.GetLength(1) && y >= 0 && y < maze.GetLength(0) && maze[y, x] != 0;
        }

        private void TryMovePlayer(int newX, int newY)
        {
            if (powerUpActive)
            {
                (int nextX, int nextY) = FindNextPathTile(newX, newY, playerX, playerY);

                if (nextX != -1 && nextY != -1)
                {
                    playerX = nextX;
                    playerY = nextY;
                    Console.WriteLine("You have used your power-up ");
                    powerUpActive = false;
                }
            }

            else
            {
                playerX = newX;
                playerY = newY;
            }
        }
        private (int, int) FindNextPathTile(int wallX, int wallY, int playerX, int playerY)
        {
            int dX = wallX - playerX;
            int dY = wallY - playerY;
            int nX = wallX + dX;
            int nY = wallY + dY;
            if (ValidMove(nX, nY))
            {
                 return (nX, nY);
            }
            return (-1, -1);
        }
        private void MoveAI()
        {
            SolveMazeMenu solver = new SolveMazeMenu(maze);
            for (int i = 0; i < aiPositions.Count; i++)
            {
                (int aiX, int aiY) = aiPositions[i];
                List<(int x, int y)> aiPath = solver.GetPath(aiX, aiY, playerX, playerY); //calculates the shortest path to the player using DFS
                //AI moves 1 step closer to the player if a valid path is found
                if (aiPath.Count > 1)
                {
                    aiPositions[i] = aiPath[aiPath.Count - 2];
                }
            }
        }

        private void PlayChaseMode()
        {
            Console.Clear();
            Console.WriteLine("=== Chase Player Mode ===");
            Console.Write("Use arrow keys to move. Press Q to quit. The chase will begin once you reach ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("@");
            Console.ResetColor();
            Console.WriteLine("\nPress F to use power-up ( One-time use )");
            PrintMaze();

            int playermoves = 0;

            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                bool playerMoved = false;

                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (ValidMove(playerX, playerY - 1))
                        {
                               TryMovePlayer(playerX, playerY - 1);
                              playerMoved = true;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (ValidMove(playerX, playerY + 1))
                        {
                             TryMovePlayer(playerX, playerY + 1);
                             playerMoved = true;
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (ValidMove(playerX - 1, playerY))
                        {
                             TryMovePlayer(playerX - 1, playerY);
                             playerMoved = true;
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if (ValidMove(playerX + 1, playerY))
                        {
                             TryMovePlayer(playerX + 1, playerY);
                             playerMoved = true;
                        }
                        break;
                    case ConsoleKey.Q:
                        MainMenu mainmenu = new MainMenu();
                        mainmenu.Display();
                        break;
                    case ConsoleKey.F:
                        if (!powerUpUsed)
                        {
                             powerUpActive = true;
                             powerUpUsed = true;
                             Console.WriteLine("Power-up activated! You can pass through a wall.");
                        }
                        break;
                    default:
                        continue;
                }

                if (chaseTriggered && playerMoved)
                {
                    playermoves++;
                }
                 PrintMaze();

                if (chaseTriggered && playerMoved && playermoves % aiMoveFrequency == 0)
                {
                    MoveAI();
                    PrintMaze();

                    if (playerX == maze.GetLength(1) - 2 && playerY == maze.GetLength(0) - 2)
                    {
                        Console.WriteLine("\nCongratulations! You have successfully reached the end point without getting caught! Press SPACE to return to main menu");
                        Console.Write(new string(' ', Console.WindowWidth));
                        while (Console.ReadKey(true).Key != ConsoleKey.Spacebar) { }
                        MainMenu mainMenu = new MainMenu();
                        mainMenu.Display();
                        break;
                    }
                }

                for (int i = 0; i < aiPositions.Count; i++)
                {
                    if (chaseTriggered)
                    {
                        if (aiPositions[i] == (playerX, playerY))
                        {
                            Console.Write(new string(' ', Console.WindowWidth));
                            Console.WriteLine($"AI {i + 1} caught you! Game over.");
                            Console.WriteLine("Press SPACE to return to the main menu.");
                            while (Console.ReadKey(true).Key != ConsoleKey.Spacebar) { }
                            MainMenu mainMenu = new MainMenu();
                            mainMenu.Display();
                            return;
                        }
                    }
                }

                if (!chaseTriggered && playerX == chaseTriggerPosition.x && playerY == chaseTriggerPosition.y)
                    {
                        chaseTriggered = true;
                        Console.WriteLine("You are now getting chased!");
                        ChaseTrigger = ' ';
                   }
              }
         }
    }
}
