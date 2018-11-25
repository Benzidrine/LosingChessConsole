using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LosingChessConsoleApp.Models;

namespace LosingChessConsoleApp.Management
{
    public static class PresentationManager
    {
        public static string PresentBoard(Chessboard chessboard)
        {
            string DisplayString = "";

            //Check for instantition
            if (chessboard.ListOfPieces.Count == 0) return DisplayString;

            int i = 1;
            List<Position> OrderedSquares = chessboard._Squares.OrderBy(c => c.X).ToList();
            OrderedSquares = chessboard._Squares.OrderBy(c => c.Y).ToList();
            foreach (Position pos in OrderedSquares)
            {
                bool ContainsPiece = false;
                foreach (BasePiece p in chessboard.ListOfPieces)
                {
                    if (p.Position.X == pos.X && p.Position.Y == pos.Y)
                    {
                        DisplayString += p.Type;
                        //DisplayString += "p";
                        ContainsPiece = !ContainsPiece;
                    }
                }
                if (!ContainsPiece)
                {
                    if (chessboard.IsSquareBlack(pos.X,pos.Y))
                    {
                        DisplayString += "_";
                    }
                    else
                    {
                        DisplayString += "-";
                    }
                }
                i++;
                if (i == 9) DisplayString += "\n";
                if (i == 9) i = 1;
            }

            return DisplayString;
        }


        public static string PresentPieces(Chessboard chessboard)
        {
            string DisplayString = "";
            foreach (BasePiece p in chessboard.ListOfPieces)
            {
                DisplayString += "Piece Name:" + p.Type + " Position X:" + p.Position.X + " Position Y:" + p.Position.Y + "\n";

            }
            return DisplayString;
        }
    }
}
