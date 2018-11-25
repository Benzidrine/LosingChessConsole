using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LosingChessConsoleApp.Models;
using LosingChessConsoleApp.Management;

namespace LosingChessConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;

            Chessboard chessboard = new Chessboard();

            Console.Write(PresentationManager.PresentBoard(chessboard));


            Console.WriteLine("Press any key to exit.");

            Console.ReadKey();
        }
    }
}
