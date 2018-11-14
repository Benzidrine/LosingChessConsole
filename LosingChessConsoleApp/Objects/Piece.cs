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
        public Position Position, NewPosition; //x y values for board position and position to move to
        public int Color;  // 1 for black, -1 for white
        public bool HasNotMoved = true;
        public bool MustCapture = false;
        public List<Position> Path;

        public BasePiece(Position Position, int Color)
        {            

        }

        public virtual bool ValidMove()
        {
            return false; 
        }
        public virtual bool ValidCapture()
        {
            return false;
        }

        public bool SetPath()
        {
            this.Path.Clear();
            bool returnval = false;
            //find number of squares between position and new position
            Position diff = new Position();
            diff.Diff(Position, NewPosition);
            // find sign of x/y values in diff
            Position sign = new Position();
            sign.Sign(diff);

            Position counter = new Position(Position);

            while ((counter.X != NewPosition.X) && (counter.Y != NewPosition.Y))
            {
                counter.Add(sign);
                if ((counter.X != NewPosition.X) && (counter.Y != NewPosition.Y))
                {
                    Path.Add(counter);
                }
            }
            if (this.Path.Count() > 0)
            {
                returnval = true;
            }
            return returnval;
        }

        public bool PathIsClear(List<BasePiece> ListOfPieces)
        {
            bool returnval = true;
            if (this.SetPath())
            {
                foreach (BasePiece piece in ListOfPieces)
                {
                    foreach (Position pos in Path)
                        if (piece.Position == pos)
                        { returnval = false; }
                }
            }
            else { returnval = false; }
            return returnval;
        }

        //todo - MovePiece() and  RemovePiece() to handle changing a pieces position on the board, 
        // and removing a piece from the board after a capture.
    }

    public class Pawn : BasePiece
    {
        public Pawn(Position position, int color) : base(position, color)
        {
            Color = color;
            Position = position;
            Value = 1;

        }

        public override bool ValidMove()
        {
            bool returnval = false;

            if (Position.X == NewPosition.X)
            {
                if ((Position.Y == (NewPosition.Y + 2 * Color)) && HasNotMoved) { returnval = true; }
                else if (Position.Y == (NewPosition.Y + Color)) { returnval = true; }
            }

            return returnval;
        }

        public override bool ValidCapture()
        {
            bool returnval = false;
            if ((Position.X == NewPosition.X + 1) || (Position.X == NewPosition.X - 1))
            {
                if (Position.Y == (NewPosition.Y + Color)) { returnval = true; }
            }
            return returnval;
        }

    }

    public class Bishop : BasePiece
    {
        public Bishop(Position position, int color) : base(position, color)
        {
            Color = color;
            Position = position;
            Value = 3;

        }

        public override bool ValidMove()
        {
            bool returnval = false;
            int xTestVal = Math.Abs(Math.Abs(Position.X) - Math.Abs(NewPosition.X));
            int yTestVal = Math.Abs(Math.Abs(Position.Y) - Math.Abs(NewPosition.Y));

            if (xTestVal == yTestVal) { returnval = true; }

            return returnval;
        }


        public override bool ValidCapture()
        {
            return this.ValidMove();  
        }
    }

    public class Knight : BasePiece
    {
        public Knight(Position position, int color) : base(position, color)
        {
            Color = color;
            Position = position;
            Value = 3;

        }

        public override bool ValidMove()
        {
            bool returnval = false;
            int xTestVal = Math.Abs(Math.Abs(Position.X) - Math.Abs(NewPosition.X));
            int yTestVal = Math.Abs(Math.Abs(Position.Y) - Math.Abs(NewPosition.Y));

            if ((xTestVal == 1 && yTestVal == 2) || (xTestVal == 1 && yTestVal == 2)) { returnval = true; }

            return returnval;
        }

        public override bool ValidCapture()
        {
            return this.ValidMove();
        }
    }

    public class Rook : BasePiece
    {
        public Rook(Position position, int color) : base(position, color)
        {
            Color = color;
            Position = position;
            Value = 5;

        }

        public override bool ValidMove()
        {
            bool returnval = false;
            int xTestVal = Math.Abs(Math.Abs(Position.X) - Math.Abs(NewPosition.X));
            int yTestVal = Math.Abs(Math.Abs(Position.Y) - Math.Abs(NewPosition.Y));

            if ((xTestVal == 0) || (yTestVal == 0)) { returnval = true; }

            return returnval;
        }

        public override bool ValidCapture()
        {
            return this.ValidMove();
        }
    }

    public class Queen : BasePiece
    {
        public Queen(Position position, int color) : base(position, color)
        {
            Color = color;
            Position = position;
            Value = 10;

        }

        public override bool ValidMove()
        {
            bool returnval = false;
            int xTestVal = Math.Abs(Math.Abs(Position.X) - Math.Abs(NewPosition.X));
            int yTestVal = Math.Abs(Math.Abs(Position.Y) - Math.Abs(NewPosition.Y));

            if (xTestVal == yTestVal) { returnval = true; }
            else if ((xTestVal == 0) || (yTestVal == 0)) { returnval = true; }

            return returnval;
        }

        public override bool ValidCapture()
        {
            return this.ValidMove();
        }
    }

    public class King : BasePiece
    {
        public King(Position position, int color) : base(position, color)
        {
            Color = color;
            Position = position;
            Value = 100;

        }

        public override bool ValidMove()
        {
            bool returnval = false;
            int xTestVal = Math.Abs(Math.Abs(Position.X) - Math.Abs(NewPosition.X));
            int yTestVal = Math.Abs(Math.Abs(Position.Y) - Math.Abs(NewPosition.Y));

            if ((xTestVal == 1 && yTestVal == 1) || (xTestVal + yTestVal == 1)) { returnval = true; }

            return returnval;
        }

        public override bool ValidCapture()
        {
            return this.ValidMove();
        }
    }

}