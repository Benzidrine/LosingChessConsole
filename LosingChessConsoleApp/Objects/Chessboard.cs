using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LosingChessConsoleApp.Models;
using LosingChessConsoleApp.Management;

namespace LosingChessConsoleApp.Models
{
    public class Chessboard
    {
        List<Position> Squares = new List<Position>();
        public List<BasePiece> ListOfPieces = new List<BasePiece>();
        private int IDAssign = 1;

        public Chessboard()
        {
            CreateBoard();
            SetBoard();
        }

        public List<Position> _Squares { get { return Squares; } }

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

        public bool movePiece(Position pos, Position newPos)
        {
            bool MoveMade = false;
            List<BasePiece> NewListOfPieces = ListOfPieces;

            if (SquareOccupied(newPos))
            {
                foreach (BasePiece piece in ListOfPieces)
                {
                    if (pos.X == piece.Position.X && pos.Y == piece.Position.Y)
                    {
                        var CastPiece = ExplicitCast.castAsCorrectPiece(piece);
                        bool isValid = CastPiece.ValidCaptureInjection(newPos, ListOfPieces);
                        if (isValid)
                        {
                            //Set Captured Piece For Removal
                            foreach (BasePiece RemovedPiece in ListOfPieces)
                            {
                                if (RemovedPiece.Position.X == newPos.X && RemovedPiece.Position.Y == newPos.Y) RemovedPiece.HasBeenCaptured = true;
                            }
                            piece.Position = newPos;
                            piece.HasNotMoved = false;
                            MoveMade = true;
                        }
                    }
                }
            }
            else
            {
                foreach (BasePiece piece in ListOfPieces)
                {
                    if (pos.X == piece.Position.X && pos.Y == piece.Position.Y)
                    {
                        var CastPiece = ExplicitCast.castAsCorrectPiece(piece);
                        bool isValid = CastPiece.ValidMoveInjection(newPos, ListOfPieces);
                        if (isValid)
                        {
                            piece.Position = newPos;
                            MoveMade = true;
                        }
                    }
                }
            }
            ListOfPieces.RemoveAll(x => x.HasBeenCaptured);
            return MoveMade;
        }

        public BasePiece getPiece(Position pos)
        {
            BasePiece retPiece = new BasePiece(-1,new Position(0,0),PieceColor.Black);

            foreach (BasePiece piece in ListOfPieces)
            {
                if (pos.X == piece.Position.X && pos.Y == piece.Position.Y)
                {
                    retPiece = piece;
                    break;
                }
            }
            return retPiece;
        }

        public bool SquareOccupied(Position pos)
        {
            bool returnval = false;

            foreach (BasePiece piece in ListOfPieces)
            {
                if (pos.X == piece.Position.X && pos.Y == piece.Position.Y)
                {
                    returnval = true;
                    break;
                }
            }
            return returnval;
        }


        public bool IsOdd( int num)
        {
            return (num % 2 == 0) ? false : true;
        }

        public bool SetBoard()
        {
            //Create and place black pawns
            for (int x = 1; x <= 8; ++x)
            {
                Position pos = new Position(x, 2);
                Pawn newPawn = new Pawn(IDAssign++, pos, PieceColor.Black);
                ListOfPieces.Add(newPawn);
            }
            //Create and place white pawns
            for (int x = 1; x <= 8; ++x)
            {
                Position pos = new Position(x, 7);
                Pawn newPawn = new Pawn(IDAssign++, pos, PieceColor.White);
                ListOfPieces.Add(newPawn);
            }
            // row tuple (row, color)
            var Rows = new List<Tuple<int, int>>() { Tuple.Create(1, PieceColor.Black), Tuple.Create(8, PieceColor.White) };
            List<int> Rooks = new List<int>() { 1, 8 };
            List<int> Knights = new List<int>() { 2, 7 };
            List<int> Bishops = new List<int>() { 3, 6 };
            
            foreach (var row in Rows)
            {
                foreach (var rook in Rooks)
                {
                    Position pos = new Position(rook, row.Item1);
                    Rook newRook = new Rook(IDAssign++, pos, row.Item2);
                    ListOfPieces.Add(newRook);
                }
                foreach (var knight in Knights)
                {
                    Position pos = new Position(knight, row.Item1);
                    Knight newKnight = new Knight(IDAssign++, pos, row.Item2);
                    ListOfPieces.Add(newKnight);
                }
                foreach (var bishop in Bishops)
                {
                    Position pos = new Position(bishop, row.Item1);
                    Bishop newBishop = new Bishop(IDAssign++,pos, row.Item2);
                    ListOfPieces.Add(newBishop);
                }
             }
            Position BQueenPos = new Position(4, 1);
            Position BKingPos = new Position(5, 1);
            Position WQueenPos = new Position(5, 8);
            Position WKingPos = new Position(4, 8);

            Queen BQueen = new Queen(IDAssign++, BQueenPos, PieceColor.Black);
            King BKing = new King(IDAssign++, BKingPos, PieceColor.Black);
            Queen WQueen = new Queen(IDAssign++, WQueenPos, PieceColor.White);
            King WKing = new King(IDAssign++, WKingPos, PieceColor.White);

            ListOfPieces.Add(BQueen);
            ListOfPieces.Add(BKing);
            ListOfPieces.Add(WQueen);
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
