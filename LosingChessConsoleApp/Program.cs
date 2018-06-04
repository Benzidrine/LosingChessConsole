using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LosingChessConsoleApp.Models;

namespace LosingChessConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(UI.Welcome("Phil is a super Cunt"));
            Console.WriteLine("Press any key to exit.");
            UI.testConsoleWriteLine();            

            Console.ReadKey();
        }
    }
}
