using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;

namespace NEA___Maze_Wiz
{
    class SolveMazeMenu
    {
        private int[,] maze;
        private int width, height;
        private Random random = new Random();
        private List<(int x, int y)> path;
        private DateTime startTime;

        public SolveMazeMenu(int[,] maze)
        {
             this.maze = maze;
             this.height = maze.GetLength(0);
             this.width = maze.GetLength(1);
             this.path = new List<(int x, int y)>();
        }

        public void Display()
        {
             Console.Clear();
                Console.WriteLine("=== Solve Maze ===");

               (int startX, int startY) = FindStartPoint();
               (int endX, int endY) = FindEndPoint();

               if (startX == -1 || startY == -1 || endX == -1 || endY == -1)
               {
                    Console.WriteLine("Invalid maze. Cannot find start or end point.");
                    Console.WriteLine("Press SPACE key to return to the main menu.");
                    while (Console.ReadKey(true).Key != ConsoleKey.Spacebar)
                    {

                    }
                    MainMenu menu = new MainMenu();
                    menu.Display();
                    return;
               }

               Console.WriteLine("Choose an algorithm to solve the maze:");
               Console.WriteLine("1. Depth-First Search (DFS)");
               Console.WriteLine("2. Breadth-First Search (BFS)");
               Console.WriteLine("3. A* Search");

               int mazeWidth = maze.GetLength(1) * 2;

               int consoleWidth = Console.WindowWidth;
               int leftMargin = (consoleWidth - mazeWidth) / 2;

            for (int i = 0; i < height; i++)
            {
                Console.SetCursorPosition(leftMargin, Console.CursorTop);
                for (int j = 0; j < width; j++)
                {
                     Console.SetCursorPosition(leftMargin + j * 2, i + 2);

                     if (maze[i, j] == 1)
                     {
                         Console.Write(" ");
                     }
                     else if (maze[i, j] == 2)
                     {
                         Console.Write("S" + " ");
                      }
                     else if (maze[i, j] == 3)
                     {
                         Console.Write("E" + " ");
                     }
                     else
                     {
                         Console.Write("██");
                     }
                 }
                 Console.WriteLine();
            }

            char choice = Console.ReadKey(true).KeyChar;
            startTime = DateTime.Now;
            Console.WriteLine("Solving maze...");
            bool found = false;

            switch (choice)
            {
                case '1':
                    found = SolveWithDFS(startX, startY, endX, endY);
                    break;
                case '2':
                    found = SolveWithBFS(startX, startY, endX, endY);
                    break;
                case '3':
                    found = SolveWithAStar(startX, startY, endX, endY);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Returning to main menu.");
                     break;
            }

            if (found)
            {
                TimeSpan ElapsedTime = DateTime.Now - startTime;
                Console.WriteLine($"Maze solved in {ElapsedTime.TotalSeconds:F2} seconds!");
            }
            else
            {
                Console.WriteLine("No path found in the maze.");
            }

            Console.WriteLine("Press SPACE key to return to the main menu.");
             while (Console.ReadKey(true).Key != ConsoleKey.Spacebar)
             {

             }
             MainMenu mainMenu = new MainMenu();
             mainMenu.Display();
        }

        public (int, int) FindStartPoint()
        {
            for (int i = 0; i < height; i++)
            {
                 for (int j = 0; j < width; j++)
                 {
                     if (maze[i, j] == 2)
                     {
                         return (j, i);
                     }
                 }
            }
            return (-1, -1); // Return invalid coordinates if start point not found
        }

        public (int, int) FindEndPoint()
        {
            for (int i = 0; i < height; i++)
            {
                 for (int j = 0; j < width; j++)
                 {
                     if (maze[i, j] == 3)
                     {
                         return (j, i);
                     }
                 }
            }
            return (-1, -1); // Return invalid coordinates if endpoint not found
        }

        private bool SolveWithDFS(int startX, int startY, int endX, int endY)
        {
            bool[,] visited = new bool[height, width];
            return DFS(startX, startY, endX, endY, visited, true);
        }

        public bool DFS(int x, int y, int endX, int endY, bool[,] visited, bool displayPath)
        {
            if (x == endX && y == endY)
            {
                 if (!displayPath)
                 {
                     path.Add((x, y));
                 }
                 return true;
            }

            visited[y, x] = true;

            // right down left up
            int[] dx = { 1, 0, -1, 0 };
            int[] dy = { 0, 1, 0, -1 };

            for (int i = 0; i < 4; i++)
            {
                 int nx = x + dx[i];
                 int ny = y + dy[i];

                 if (IsValid(nx, ny) && !visited[ny, nx] && (maze[ny, nx] == 1 || maze[ny, nx] == 3))
                 {
                     if (displayPath)
                     {
                         DisplayMazeWithCurrentPath(visited);
                         Thread.Sleep(10);
                     }

                     if (DFS(nx, ny, endX, endY, visited, displayPath))
                     {
                         if (!displayPath)
                         {
                             path.Add((x, y));
                         }
                         return true;
                     }
                 }
            }
            return false;
        }
        public List<(int, int)> GetPath(int startX, int startY, int endX, int endY)
         {
            path.Clear();
            if (DFS(startX, startY, endX, endY, new bool[maze.GetLength(0), maze.GetLength(1)], false))
            {
                return path;
            }
            return null;
        }
        private bool SolveWithBFS(int startX, int startY, int endX, int endY)
        {
            bool[,] visited = new bool[height, width];
            Queue<(int, int)> queue = new Queue<(int, int)>();
            queue.Enqueue((startX, startY));
            visited[startY, startX] = true;

            while (queue.Count > 0)
            {
                (int x, int y) = queue.Dequeue();

                if (x == endX && y == endY)
                {
                    return true;
                }

                // right down left up
                int[] dx = { 1, 0, -1, 0 };
                int[] dy = { 0, 1, 0, -1 };

                for (int i = 0; i < 4; i++)
                {
                    int nx = x + dx[i];
                    int ny = y + dy[i];

                    if (IsValid(nx, ny) && !visited[ny, nx] && (maze[ny, nx] == 1 || maze[ny, nx] == 3))
                    {
                        queue.Enqueue((nx, ny));
                        visited[ny, nx] = true;

                        DisplayMazeWithCurrentPath(visited);

                        Thread.Sleep(10);
                    }
                }
            }

             return false;
        }

        private bool SolveWithAStar(int startX, int startY, int endX, int endY)
        {
            bool[,] visited = new bool[height, width];
            PriorityQueue<(int, int), int> priorityQueue = new PriorityQueue<(int, int), int>();
            priorityQueue.Enqueue((startX, startY), 0);
            visited[startY, startX] = true;

            while (priorityQueue.Count > 0)
            {
                (int x, int y) = priorityQueue.Dequeue();

                if (x == endX && y == endY)
                {
                    return true;
                }

                // right down left up
                int[] dx = { 1, 0, -1, 0 };
                int[] dy = { 0, 1, 0, -1 };

                for (int i = 0; i < 4; i++)
                {
                    int nx = x + dx[i];
                    int ny = y + dy[i];

                    if (IsValid(nx, ny) && !visited[ny, nx] && (maze[ny, nx] == 1 || maze[ny, nx] == 3))
                    {
                        int priority = Math.Abs(nx - endX) + Math.Abs(ny - endY); //Manhattan Distance for priority
                        priorityQueue.Enqueue((nx, ny), priority); //nodes with the lowest estimated cost (f-cost) are processed first)
                        visited[ny, nx] = true;

                        DisplayMazeWithCurrentPath(visited);

                        Thread.Sleep(10);
                    }
                }
            }
              return false;
        }

        private bool IsValid(int x, int y)
        {
            return x >= 0 && x < width && y >= 0 && y < height;
        }

        private void DisplayMazeWithCurrentPath(bool[,] visited)
        {
            int mazeWidth = maze.GetLength(1) * 2;

             int consoleWidth = Console.WindowWidth;
             int leftMargin = (consoleWidth - mazeWidth) / 2;

            for (int i = 0; i < height; i++)
            {
                Console.SetCursorPosition(leftMargin, Console.CursorTop);
                for (int j = 0; j < width; j++)
                {
                     Console.SetCursorPosition(leftMargin + j * 2, i + 2);

                     if (visited[i, j])
                     {
                         Console.ForegroundColor = ConsoleColor.Cyan;
                         Console.Write("+" + " ");
                         Console.ResetColor();
                     }
                     else if (maze[i, j] == 1)
                     {
                         Console.Write(" ");
                     }
                     else if (maze[i, j] == 2)
                     {
                         Console.Write("S" + " ");
                     }
                     else if (maze[i, j] == 3)
                     {
                         Console.Write("E" + " ");
                     }
                     else
                     {
                         Console.Write("██");
                     }
                }
                   Console.WriteLine();
             }
        }
    }
}
