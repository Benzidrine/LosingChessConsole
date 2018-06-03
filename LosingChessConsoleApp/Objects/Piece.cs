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

        string name;
        int Value;
        Tuple<int, int> Position; //x y values for board position
        int Color;  // 1 for black, -1 for white
        bool HasNotMoved = true;
        
        


        public BasePiece(Tuple<> Position, bool Color)
        {
            

        }

        public virtual bool MovePiece() { }
        public virtual bool TakePiece() { }
        



    }

    public class Pawn : BasePiece
    {
        public Pawn(Tuple<int, int> position, int color) : base(position, color)
        {
            Color = color;
            Position = position;
            Value = 1;

        }

        public override bool MovePiece(Tuple<int, int> MoveTo)
        {
            bool returnval = false;

            if (MoveTo.Item1 = Position.Item1)
            {
                if ((MoveTo.Item2 = (Position.Item2 + 2 * Color)) && HasNotMoved) { returnval = true; }
                else if (MoveTo.Item2 = (Position.Item2 + Color)) { returnval = true; }
            }

            return returnval;
        }

        public override bool TakePiece(Tuple<int, int> MoveTo)
        {
            bool returnval = false;
            if ((MoveTo.Item1 = Position.Item1 + 1) || (MoveTo.Item1 = Position.Item1 - 1))
            {
                if (MoveTo.Item2 = (Position.Item2 + Color)) { returnval = true; }
            }
                return returnval;
        }

    }

}