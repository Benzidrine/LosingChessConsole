using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosingChessConsoleApp.Models
{
    public class Chessboard
    {

        public Chessboard()
        {

        }

        int[,] ChessboardArray = new int[,] { };


        public  bool IsSquareBlack (int x, int y)
        {
            return true;
        }


        private  string BoardXAxis(int x)
        {
            switch (x)
            {
                case 0: return "A";
                case 1: return "B";
                case 2: return "C";
                case 3: return "D";
                case 4: return "E";
                case 5: return "F";
                case 6: return "G";
                case 7: return "H";
                default: return "0";
            }
        }

        private  string BoardYAxis(int y)
        {
            switch (y)
            {
                case 0: return "8";
                case 1: return "7";
                case 2: return "6";
                case 3: return "5";
                case 4: return "4";
                case 5: return "3";
                case 6: return "2";
                case 7: return "1";
                default: return "0";
            }
        }

        private  int BoardXAxis(String x)
        {
            switch (x)
            {
                case "A": return 0;
                case "B": return 1;
                case "C": return 2;
                case "D": return 3;
                case "E": return 4;
                case "F": return 5;
                case "G": return 6;
                case "H": return 7;
                default: return 0;
            }
        }

        private  int BoardYAxis(String y)
        {
            switch (y)
            {
                case "8": return 0;
                case "7": return 1;
                case "6": return 2;
                case "5": return 3;
                case "4": return 4;
                case "3": return 5;
                case "2": return 6;
                case "1": return 7;
                default: return 0;
            }
        }



    }
}
