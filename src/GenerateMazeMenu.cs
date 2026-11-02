using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;

namespace NEA___Maze_Wiz
{
    class GenerateMazeMenu
    {
        private int width, height;
        private int[,] maze;
        private bool instant;
        private bool[,] visited;
        private Stack<(int, int)> stack;
        private Random rand;
        private const char Wall = '█';
        private const char Path = ' ';
        private const string StartPoint = "S ";
        private const string EndPoint = "E ";

        public GenerateMazeMenu(int width, int height, bool instant)
        {
            this.width = width;
            this.height = height;
            maze = new int[this.height, this.width];
            visited = new bool[this.height, this.width];
            stack = new Stack<(int, int)>();
            rand = new Random();
            this.instant = instant;
            InitializeMaze();
        }

        public GenerateMazeMenu() { }

        public int[,] Display()
        {
            Console.Clear();
            Console.WriteLine("=== Generate Maze ===");
            Console.WriteLine("\n Note: Larger maze dimensions may not fit entirely on the screen.");
             Console.WriteLine("Lowering the dimensions will fix this.");
             ChooseDifficulty();
             Console.WriteLine("Press Y for instant generation. Press any other key for animated generation.");
             if (Console.ReadKey(true).Key == ConsoleKey.Y)
             {
                 instant = true;
             }
             else { instant = false; }
             Console.WriteLine("Please maximise the window and press SPACE to proceed");
             while (Console.ReadKey(true).Key != ConsoleKey.Spacebar)
             {

             }
             Console.WriteLine($"\nGenerating maze with width {width} and height {height}...");
             Thread.Sleep(1500);
             GenerateMazeMenu mazemenu = new GenerateMazeMenu(width, height, instant);
             mazemenu.GenerateMaze();

             maze = mazemenu.maze;

             Console.SetCursorPosition(0, height + 2);
             Console.WriteLine("Maze generation complete. Press SPACE to proceed.");
             while (Console.ReadKey(true).Key != ConsoleKey.Spacebar)
             {

             }
             return maze;
        }

        private int GetMazeWidth()
        {
             int inWidth = 0;
             bool validInput = false;
             while (!validInput)
             {
                 Console.Write("\nEnter maze width (10-100): ");
                 int input = int.Parse(Console.ReadLine());
                 if (input >= 10 && input <= 100)
                 {
                     inWidth = input;
                     validInput = true;
                  }
                 else
                 {
                     Console.WriteLine("Invalid Input. Try Again.");
                 }
             }
             return inWidth % 2 == 0 ? inWidth + 1 : inWidth;
        }

        private int GetMazeHeight()
        {
            int inHeight = 0;
            bool validInput = false;
            while (!validInput)
            {
                Console.Write("\nEnter maze height (10-60): ");
                int input = int.Parse(Console.ReadLine());
                if (input >= 10 && input <= 60)
                {
                    inHeight = input;
                    validInput = true;
                }
                else
                {
                    Console.WriteLine("Invalid Input. Try Again.");
                }
            }
            return inHeight % 2 == 0 ? inHeight + 1 : inHeight;
        }

        private void InitializeMaze()
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    maze[i, j] = 0;
                }
            }
        }

        public void GenerateMaze()
        {
            int startX = 1;
            int startY = 1;
            stack.Push((startX, startY)); //Using DFS
            visited[startY, startX] = true;
             maze[startY, startX] = 1;

            if (!instant) PrintMaze(); //don't print at the start if instant mode is active

            while (stack.Count > 0)
            {
                (int x, int y) = stack.Peek();
                List<(int, int)> neighbours = GetUnvisitedNeighbours(x, y);

                if (neighbours.Count > 0)
                {
                    (int nx, int ny) = neighbours[rand.Next(neighbours.Count)];
                    visited[ny, nx] = true;
                    RemoveWall(x, y, nx, ny);
                    stack.Push((nx, ny));

                    if (!instant)
                    {
                        PrintCell((x + nx) / 2, (y + ny) / 2);
                        PrintCell(nx, ny);
                        Thread.Sleep(10);
                    }
                }
                else
                {
                    stack.Pop(); //backtrack if no unvisited neighbours found
                }
            }

            SetStartAndEndPoints();
            if (instant)
            {
                Console.Clear();
                PrintMaze();
            } //print full maze once after generation if instant mode active
        }

        private List<(int, int)> GetUnvisitedNeighbours(int x, int y)
        {
            List<(int,int)> neighbours = new List<(int, int)>();

            if (x > 1 && !visited[y, x - 2]) neighbours.Add((x - 2, y));
                 if (x < width - 2 && !visited[y, x + 2]) neighbours.Add((x + 2, y));
                if (y > 1 && !visited[y - 2, x]) neighbours.Add((x, y - 2));
                if (y < height - 2 && !visited[y + 2, x]) neighbours.Add((x, y + 2));

                return neighbours;
           }

           private void RemoveWall(int x1, int y1, int x2, int y2)
           {
               maze[(y1 + y2) / 2, (x1 + x2) / 2] = 1; //Open path between walls
                maze[y2, x2] = 1;
           }

           private void PrintMaze()
           {
               Console.Clear();
               for (int i = 0; i < height; i++)
               {
                   for (int j = 0; j < width; j++)
                   {
                       Console.Write(GetCellRepresentation(maze[i, j]));
                   }
                   Console.WriteLine();
               }
           }

           private void PrintCell(int x, int y)
           {
               int consoleX = x * 2;
               int consoleY = y;

                Console.SetCursorPosition(consoleX, consoleY);
                Console.Write(GetCellRepresentation(maze[y, x]));
           }

           private string GetCellRepresentation(int cell)
           {
               switch (cell)
               {
                   case 0: return "██";
                   case 1: return "  ";
                   case 2: return StartPoint;
                   case 3: return EndPoint;
                   default: return "  ";
              }
        }

        private void SetStartAndEndPoints()
        {
            maze[1, 1] = 2;
            maze[height - 2, width - 2] = 3;
            PrintCell(1, 1);
            PrintCell(width - 2, height - 2);
        }

        private void ChooseDifficulty()
        {
            bool validInput = false;
            Dictionary<int,(int, int)> Difficulties = new Dictionary<int, (int width, int height)>
            {
                {1, (15, 15) },
                {2, (31,31) },
                {3, (51,51) }
            };
            while (!validInput)
            {
                Console.WriteLine("Choose difficulty:");
                Console.WriteLine("1. Easy (15x15)");
                Console.WriteLine("2. Medium (31x31)");
                Console.WriteLine("3. Hard (51x51)");
                Console.WriteLine("4. Custom");
                Console.Write("Enter your choice (1-4): ");
                int input = int.Parse(Console.ReadLine());
                switch (input)
                {
                    case 1:
                    case 2:
                    case 3:
                        (width, height) = Difficulties[input];
                        validInput = true;
                        break;
                    case 4:
                        width = GetMazeWidth();
                        height = GetMazeHeight();
                        validInput = true;
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please choose a number between 1 and 4.");
                        break;
                   }
             }
        }
    }
}
