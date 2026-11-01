using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;

namespace NEA___Maze_Wiz
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            MainMenu mainMenu = new MainMenu();
            mainMenu.Display();
        }
    }
}
