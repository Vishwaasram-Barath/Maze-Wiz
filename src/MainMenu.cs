using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;

namespace NEA___Maze_Wiz
{
    class MainMenu
    {
        private int[,] maze;

        public void Display()
        {
            Console.Clear();
            Console.WriteLine("=== Main Menu ===");
            Console.WriteLine("1. Generate Maze");
            Console.WriteLine("2. Load Maze");
            Console.WriteLine("5. Exit");

            HandleInput();
        }

        private void HandleInput()
        {
            switch (Console.ReadLine())
            {
                case "1":
                     GenerateMazeMenu generateMazeMenu = new GenerateMazeMenu();
                     maze = generateMazeMenu.Display();
                     PostGenerationMenu();
                       break;
                  case "2":
                      LoadMaze();
                      break;
                  case "5":
                      Environment.Exit(0);
                      break;
                  default:
                      Console.WriteLine("Invalid choice. Try again.");
                      Display();
                      break;
             }
        }

        private void PostGenerationMenu()
        {
            Console.Clear();
            Console.WriteLine("=== Maze Generated ===");
            Console.WriteLine("1. Play Maze in Single Player Mode");
            Console.WriteLine("2. Play Maze in Chase Player Mode");
            Console.WriteLine("3. Solve Maze");
            Console.WriteLine("4. Save or Load Maze");
            Console.WriteLine("5. Return to Main Menu");

             HandlePostGenerationInput();
        }

        private void HandlePostGenerationInput()
        {
            switch (Console.ReadLine())
            {
                case "1":
                    SinglePlayerMenu singlePlayerMenu = new SinglePlayerMenu(maze);
                    singlePlayerMenu.Display();
                    break;
                case "2":
                    ChasePlayerMenu chasePlayerMenu = new ChasePlayerMenu(maze);
                    chasePlayerMenu.Display();
                    break;
                case "3":
                    SolveMazeMenu solveMazeMenu = new SolveMazeMenu(maze);
                    solveMazeMenu.Display();
                    break;
                case "4":
                       SaveOrLoadMaze();
                      PostGenerationMenu();
                      break;
                  case "5":
                      Display();
                      break;
                  default:
                      Console.WriteLine("Invalid choice. Try again.");
                      PostGenerationMenu();
                      break;
             }
        }

        private void SaveOrLoadMaze()
        {
            Console.Clear();
            Console.WriteLine("=== Save or Load Maze ===");
            Console.WriteLine("1. Save Maze");
            Console.WriteLine("2. Load Maze");
            string choice = Console.ReadLine();

             switch (choice)
             {
                 case "1":
                     SaveMaze();
                     break;
                 case "2":
                     LoadMaze();
                     break;
                 default:
                     Console.WriteLine("Invalid choice. Try again.");
                     SaveOrLoadMaze();
                     break;
             }
        }

        private void SaveMaze()
        {
            Console.Write("Enter the file name to save the maze: ");
            string fileName = Console.ReadLine();
            try
            {
                 using (StreamWriter writer = new StreamWriter(fileName))
                 {
                     for (int i = 0; i < maze.GetLength(0); i++)
                     {
                         for (int j = 0; j < maze.GetLength(1); j++)
                           {
                               writer.Write(maze[i, j] + " ");
                          }
                          writer.WriteLine();
                     }
                }
                Console.WriteLine("Maze saved successfully.");
                Console.WriteLine("Press SPACE key to enter Post Generation Menu");
                while (Console.ReadKey(true).Key != ConsoleKey.Spacebar)
                {

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving maze: {ex.Message}");
            }
        }

        private void LoadMaze()
        {
            Console.WriteLine("If you have changed your mind, Press F1 to return to the main menu");
            Console.Write("Enter the file name to load the maze: ");
            string fileName = "";

            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.F1)
                {
                    MainMenu mainMenu = new MainMenu();
                    mainMenu.Display();
                    return;
                }
                else if (keyInfo.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }
                else if (keyInfo.Key == ConsoleKey.Backspace && fileName.Length > 0)
                {
                    fileName = fileName.Substring(0, fileName.Length - 1);
                      Console.Write("\b \b"); //moves cursor back, erases a char and then moves back again when BACKSPACE is pressed
                 }
                 else if (!char.IsControl(keyInfo.KeyChar)) //checks for valid characters to append to fileName
                 {
                     fileName += keyInfo.KeyChar;
                     Console.Write(keyInfo.KeyChar);
                 }
            }

            try
            {
                 using (StreamReader sr = new StreamReader(fileName))
                 {
                     string line;
                     List<string[]> rows = new List<string[]>();

                     while ((line = sr.ReadLine()) != null)
                     {
                         string[] parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                         rows.Add(parts);
                     }

                     int Rows = rows.Count;
                     int Cols = rows[0].Length;

                     maze = new int[Rows, Cols];

                     for (int i = 0; i < Rows; i++)
                     {
                         for (int j = 0; j < Cols; j++)
                         {
                             maze[i, j] = int.Parse(rows[i][j]);
                         }
                     }
                 }

                 Console.WriteLine("Maze loaded successfully.");
                 Console.WriteLine("Press SPACE key to enter Post Generation Menu.");
                 while (Console.ReadKey(true).Key != ConsoleKey.Spacebar)
                 {

                 }
                 PostGenerationMenu();
              }
             catch (FileNotFoundException)
             {
                 Console.WriteLine("File not found.");
                 Console.ReadKey();
                 LoadMaze();
             }
             catch (Exception ex)
             {
                 Console.WriteLine($"Error loading maze: {ex.Message}");
                 Console.ReadKey();
                 LoadMaze();
             }
        }
    }
}
