using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosingChessConsoleApp.Models
{


    public class BasePiece
    {
        public enum PieceName { Pawn = 1, Bishop, Knight, Rook, Queen, King };

        public int Type;
        public int Value;
        public Position Position, NewPosition; //x y values for board position and position to move to
        public int Color;  // 1 for black, -1 for white
        public bool HasNotMoved = true;
        public bool MustCapture = false;
        public bool HasBeenCaptured = false;
        public List<Position> Path;

        public string ColorIs()
        {
            if (Color == -1) return "Black";
            return "White";
        }

        public BasePiece(Position Position, int Color)
        {
            Path = new List<Position>();
        }

        public string letterExpression()
        {
            string letter = "Z";
            switch (Type)
            {
                case 1:
                    letter = "P";
                    break;
                case 2:
                    letter = "B";
                    break;
                case 3:
                    letter = "H";
                    break;
                case 4:
                    letter = "R";
                    break;
                case 5:
                    letter = "Q";
                    break;
                case 6:
                    letter = "K";
                    break;
            }

            if (Color == PieceColor.White)
            {
                letter = letter.ToLower();
            }

            return letter;
        }

        public virtual bool ValidMove()
        {
            return false;
        }

        public virtual bool ValidMoveInjection(Position newpos, List<BasePiece> ListOfPieces)
        {
            return false;
        }

        public virtual bool ValidCapture()
        {
            return false;
        }

        public virtual bool ValidCaptureInjection(Position newpos, List<BasePiece> ListOfPieces)
        {
            return false;
        }

        public bool SetPath()
        {
            Path.Clear();
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

        public bool PathIsClear(List<BasePiece> ListOfPieces, Position StartPos, Position EndPos)
        {
            bool returnval = false;
            string type = "";
            if (StartPos.X == EndPos.X && StartPos.Y != EndPos.Y) type = "Vertical";
            if (StartPos.X != EndPos.X && StartPos.Y == EndPos.Y) type = "Horizontal";
            if (StartPos.X != EndPos.X && StartPos.Y != EndPos.Y) type = "Diagonal";
            if (type == "Vertical")
            {
                int sign = Math.Sign(EndPos.Y - StartPos.Y);
                for (int i = 1; i < 10; i++)
                {
                    if (StartPos.Y + (sign * i) == EndPos.Y) return true;
                    foreach (BasePiece bp in ListOfPieces)
                    {
                        if (bp.Position.X == StartPos.X && bp.Position.Y == (StartPos.Y + (sign * i))) return false;
                    }
                }
            }
            if (type == "Horizontal")
            {
                int sign = Math.Sign(EndPos.X - StartPos.X);
                for (int i = 1; i < 10; i++)
                {
                    if (StartPos.X + (sign * i) == EndPos.X) return true;
                    foreach (BasePiece bp in ListOfPieces)
                    {
                        if (bp.Position.Y == StartPos.Y && bp.Position.X == (StartPos.X + (sign * i))) return false;
                    }
                }
            }
            if (type == "Diagonal")
            {

                int signX = Math.Sign(EndPos.X - StartPos.X);
                int signY = Math.Sign(EndPos.Y - StartPos.Y);

                //(7,7) (8,6) 7 - 8 == 7 - 6
                //Check if truly 45 degree diagonal
                if (Math.Abs(StartPos.X - EndPos.X) != Math.Abs(StartPos.Y - EndPos.Y)) return false;
                for (int i = 1; i < 10; i++)
                {
                    if (StartPos.X + (signX * i) == EndPos.X && StartPos.Y + (signY * i) == EndPos.Y) return true;
                    foreach (BasePiece bp in ListOfPieces)
                    {
                        if (bp.Position.Y == (StartPos.Y + (signY * i)) && bp.Position.X == (StartPos.X + (signX * i))) return false;
                    }
                }
            }
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
            Type = 1;

        }

        public override bool ValidMove()
        {
            bool returnval = false;

            if (Position.X == NewPosition.X)
            {
                if ((NewPosition.Y == (Position.Y - 2 * Color)) && HasNotMoved) { returnval = true; }
                else if (NewPosition.Y == (Position.Y - Color)) { returnval = true; }
            }

            return returnval;
        }

        public override bool ValidMoveInjection(Position newpos, List<BasePiece> ListOfPieces)
        {
            if (!PathIsClear(ListOfPieces, Position, newpos)) return false;
            return this.ValidMove();
        }

        public override bool ValidCapture()
        {
            bool returnval = false;
            if ((Position.X == NewPosition.X + 1) || (Position.X == NewPosition.X - 1))
            {
                if (NewPosition.Y == (Position.Y - Color)) { returnval = true; }
            }
            return returnval;
        }

        public override bool ValidCaptureInjection(Position newpos, List<BasePiece> ListOfPieces)
        {
            NewPosition = newpos;
            return ValidCapture();
        }
    }

    public class Bishop : BasePiece
    {
        public Bishop(Position position, int color) : base(position, color)
        {
            Color = color;
            Position = position;
            Value = 3;
            Type = 2;

        }

        public override bool ValidMove()
        {
            bool returnval = false;
            int xTestVal = Math.Abs(Math.Abs(Position.X) - Math.Abs(NewPosition.X));
            int yTestVal = Math.Abs(Math.Abs(Position.Y) - Math.Abs(NewPosition.Y));

            if (xTestVal == yTestVal) { returnval = true; }

            return returnval;
        }

        public override bool ValidMoveInjection(Position newpos, List<BasePiece> ListOfPieces)
        {
            if (!PathIsClear(ListOfPieces, Position, newpos)) return false;
            return this.ValidMove();
        }

        public override bool ValidCapture()
        {
            return this.ValidMove();
        }


        public override bool ValidCaptureInjection(Position newpos, List<BasePiece> ListOfPieces)
        {
            NewPosition = newpos;
            if (!PathIsClear(ListOfPieces,Position,newpos)) return false;
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
            Type = 3;

        }

        public override bool ValidMove()
        {
            bool returnval = false;
            int xTestVal = Math.Abs(Math.Abs(Position.X) - Math.Abs(NewPosition.X));
            int yTestVal = Math.Abs(Math.Abs(Position.Y) - Math.Abs(NewPosition.Y));

            if ((xTestVal == 1 && yTestVal == 2) || (xTestVal == 1 && yTestVal == 2)) { returnval = true; }

            return returnval;
        }

        public override bool ValidMoveInjection(Position newpos, List<BasePiece> ListOfPieces)
        {
            return this.ValidMove();
        }

        public override bool ValidCapture()
        {
            return this.ValidMove();
        }

        public override bool ValidCaptureInjection(Position newpos, List<BasePiece> ListOfPieces)
        {
            NewPosition = newpos;
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
            Type = 4;

        }

        public override bool ValidMove()
        {
            bool returnval = false;
            int xTestVal = Math.Abs(Math.Abs(Position.X) - Math.Abs(NewPosition.X));
            int yTestVal = Math.Abs(Math.Abs(Position.Y) - Math.Abs(NewPosition.Y));

            if ((xTestVal == 0) || (yTestVal == 0)) { returnval = true; }

            return returnval;
        }

        public override bool ValidMoveInjection(Position newpos, List<BasePiece> ListOfPieces)
        {
            if (!PathIsClear(ListOfPieces, Position, newpos)) return false;
            return this.ValidMove();
        }

        public override bool ValidCapture()
        {
            return this.ValidMove();
        }

        public override bool ValidCaptureInjection(Position newpos, List<BasePiece> ListOfPieces)
        {
            NewPosition = newpos;
            if (!PathIsClear(ListOfPieces, Position, newpos)) return false;
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
            Type = 5;

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

        public override bool ValidMoveInjection(Position newpos, List<BasePiece> ListOfPieces)
        {
            if (!PathIsClear(ListOfPieces, Position, newpos)) return false;
            return this.ValidMove();
        }

        public override bool ValidCapture()
        {
            return this.ValidMove();
        }

        public override bool ValidCaptureInjection(Position newpos, List<BasePiece> ListOfPieces)
        {
            NewPosition = newpos;
            if (!PathIsClear(ListOfPieces, Position, newpos)) return false;
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
            Type = 6;

        }

        public override bool ValidMove()
        {
            bool returnval = false;
            int xTestVal = Math.Abs(Math.Abs(Position.X) - Math.Abs(NewPosition.X));
            int yTestVal = Math.Abs(Math.Abs(Position.Y) - Math.Abs(NewPosition.Y));

            if ((xTestVal == 1 && yTestVal == 1) || (xTestVal + yTestVal == 1)) { returnval = true; }

            return returnval;
        }

        public override bool ValidMoveInjection(Position newpos, List<BasePiece> ListOfPieces)
        {
            if (!PathIsClear(ListOfPieces, Position, newpos)) return false;
            return this.ValidMove();
        }

        public override bool ValidCapture()
        {
            return this.ValidMove();
        }

        public override bool ValidCaptureInjection(Position newpos, List<BasePiece> ListOfPieces)
        {
            NewPosition = newpos;
            if (!PathIsClear(ListOfPieces, Position, newpos)) return false;
            return this.ValidMove();
        }
    }

}