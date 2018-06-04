using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LosingChessConsoleApp.Models;

namespace LosingChessConsoleApp.Models
{
    public class Chessboard
    {

        public Chessboard()
        {

        }

        int[,] ChessboardArray = new int[,] { };

        List<BasePiece> ListOfPieces = new List<BasePiece>();

        public bool IsOdd( int num)
        {
            return (num % 2 == 0) ? false : true;
        }

        public bool MovePiece()
        {
            return true;
        }

        public  bool IsSquareBlack (int x, int y)
        {
            bool returnVal = false;
            if (IsOdd(x))
            {
                if (!IsOdd(y))
                {
                    returnVal = true;
                }
            }
            else if (IsOdd(y))
            {
                returnVal = true;
            }
            
            return returnVal;
        }


        private  string BoardXAxis(int x)
        {
            switch (x)
            {
                case 1: return "A";
                case 2: return "B";
                case 3: return "C";
                case 4: return "D";
                case 5: return "E";
                case 6: return "F";
                case 7: return "G";
                case 8: return "H";
                default: return "0";
            }
        }

        private  string BoardYAxis(int y)
        {
            switch (y)
            {
                case 1: return "8";
                case 2: return "7";
                case 3: return "6";
                case 4: return "5";
                case 5: return "4";
                case 6: return "3";
                case 7: return "2";
                case 8: return "1";
                default: return "0";
            }
        }

        private  int BoardXAxis(string x)
        {
            switch (x)
            {
                case "A": return 1;
                case "B": return 2;
                case "C": return 3;
                case "D": return 4;
                case "E": return 5;
                case "F": return 6;
                case "G": return 7;
                case "H": return 8;
                default: return 0;
            }
        }

        private  int BoardYAxis(string y)
        {
            switch (y)
            {
                case "8": return 1;
                case "7": return 2;
                case "6": return 3;
                case "5": return 4;
                case "4": return 5;
                case "3": return 6;
                case "2": return 7;
                case "1": return 8;
                default: return 0;
            }
        }


    }
}
