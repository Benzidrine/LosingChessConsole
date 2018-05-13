using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosingChessConsoleApp.Models
{
    

    public class BasePiece
    {
        private string PieceName(int val)
        {
            switch (val)
            {
                case 1: return "Pawn";
                case 2: return "Bishop";
                case 3: return "Knight";
                case 4: return "Rook";
                case 5: return "Queen";
                case 6: return "King";
                default: return null;
            }
        }

        private int PieceName(string val)
        {
            switch (val)
            {
                case "Pawn"     : return 1 ;
                case "Bishop"   : return 2 ;
                case "Knight"   : return 3 ;
                case "Rook"     : return 4 ;
                case "Queen"    : return 5 ;
                case "King"     : return 6 ;
                default         : return null;
            }
        }

        int Value;
        var Position;
        bool Color;

        public BasePiece()
        {
            bool MovePiece();

        }
    }

}