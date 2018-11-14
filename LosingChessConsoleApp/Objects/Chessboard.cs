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
        //I made the board as a list of positions... do we need to make it an array?
        int[,] ChessboardArray = new int[,] { };

        List<Position> Squares = new List<Position>();
        public bool CreateBoard()
        {
            for(int x = 1; x <= 8; ++x)
            {
                for(int y = 1; y <= 8; ++y)
                {
                    Position newPos = new Position(x, y);
                    Squares.Add(newPos);
                }
            }
            return true;
        }

        public bool SquareOccupied(Position pos)
        {
            bool returnval = false;

            foreach (BasePiece piece in ListOfPieces)
            {
                if (pos == piece.Position)
                {
                    returnval = true;
                    break;
                }
            }
            return returnval;
        }

        List<BasePiece> ListOfPieces = new List<BasePiece>();

        public bool IsOdd( int num)
        {
            return (num % 2 == 0) ? false : true;
        }

        public bool SetBoard()
        {
            for (int x = 1; x <= 8; ++x)
            {
                Position pos = new Position(x, 2);
                Pawn newPawn = new Pawn(pos, -1);
                ListOfPieces.Add(newPawn);
            }
            for (int x = 1; x <= 8; ++x)
            {
                Position pos = new Position(x, 7);
                Pawn newPawn = new Pawn(pos, 1);
                ListOfPieces.Add(newPawn);
            }
            // row tuple (row, color)
            var Rows = new List<Tuple<int, int>>() { Tuple.Create(1, 1), Tuple.Create(8, -1) };
            List<int> Rooks = new List<int>() { 1, 8 };
            List<int> Knights = new List<int>() { 2, 7 };
            List<int> Bishops = new List<int>() { 3, 6 };
            
            foreach (var row in Rows)
            {
                foreach (var rook in Rooks)
                {
                    Position pos = new Position(row.Item1, rook);
                    Rook newRook = new Rook(pos, row.Item2);
                    ListOfPieces.Add(newRook);
                }
                foreach (var knight in Knights)
                {
                    Position pos = new Position(row.Item1, knight);
                    Knight newKnight = new Knight(pos, row.Item2);
                    ListOfPieces.Add(newKnight);
                }
                foreach (var bishop in Bishops)
                {
                    Position pos = new Position(row.Item1, bishop);
                    Bishop newBishop = new Bishop(pos, row.Item2);
                    ListOfPieces.Add(newBishop);
                }
             }
            Position BQueenPos = new Position(4, 1);
            Position BKingPos = new Position(5, 1);
            Position WQueenPos = new Position(5, 8);
            Position WKingPos = new Position(4, 8);

            Queen BQueen = new Queen(BQueenPos, 1);
            Queen WQueen = new Queen(WQueenPos, -1);
            King BKing = new King(BKingPos, 1);
            King WKing = new King(WKingPos, -1);

            ListOfPieces.Add(BQueen);
            ListOfPieces.Add(WQueen);
            ListOfPieces.Add(BKing);
            ListOfPieces.Add(WKing);

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
