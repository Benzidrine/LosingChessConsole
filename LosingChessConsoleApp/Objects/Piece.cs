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

        private int? PieceName(string val)
        {
            switch (val)
            {
                case "Pawn": return 1;
                case "Bishop": return 2;
                case "Knight": return 3;
                case "Rook": return 4;
                case "Queen": return 5;
                case "King": return 6;
                default: return null;
            }
        }

        public int Value;
        public Position Position; //x y values for board position
        public int Color;  // 1 for black, -1 for white
        public bool HasNotMoved = true;

        public BasePiece(Position Position, int Color)
        {            

        }

        public virtual bool MovePiece(Position MoveTo) {
            return false; 
        }
        public virtual bool TakePiece(Position MoveTo) {
            return false;
        }
    }

    public class Pawn : BasePiece
    {
        public Pawn(Position position, int color) : base(position, color)
        {
            Color = color;
            Position = position;
            Value = 1;

        }

        public override bool MovePiece(Position positionToMove)
        {
            bool returnval = false;

            if (Position.X == positionToMove.X)
            {
                if ((Position.Y == (positionToMove.Y + 2 * Color)) && HasNotMoved) { returnval = true; }
                else if (Position.Y == (positionToMove.Y + Color)) { returnval = true; }
            }

            return returnval;
        }

        public override bool TakePiece(Position positionToTake)
        {
            bool returnval = false;
            if ((Position.X == positionToTake.X + 1) || (Position.X == positionToTake.X - 1))
            {
                if (Position.Y == (positionToTake.Y + Color)) { returnval = true; }
            }
            return returnval;
        }

    }
}