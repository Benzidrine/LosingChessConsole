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

        public int ID;
        public int Type;
        public int Value;
        public Position Position;
        public Position NewPosition; //x y values for board position and position to move to
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

        public BasePiece(int ID, Position Position, int Color)
        {
            NewPosition = new Position();
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

        public virtual bool CanMove(Chessboard chessboard)
        {
            return false;
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

        public virtual Position LongestPositionToMoveTo(Chessboard chessboard)
        {
            return new Position();
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
        public Pawn(int id, Position position, int color) : base(id, position, color)
        {
            ID = id;
            Color = color;
            Position = position;
            NewPosition = new Position(0,0);
            Value = 1;
            Type = 1;

        }

        public override bool CanMove(Chessboard chessboard)
        {
            Position TestPos = new Position();
            TestPos.Y = (Position.Y - Color);
            TestPos.X = (Position.X);
            if (chessboard.SquareOccupied(TestPos) && !TestPos.WithinBounds()) return false;
            return true;
        }

        public override Position LongestPositionToMoveTo(Chessboard chessboard)
        {
            Position NewPos = new Position();
            if (HasNotMoved)
            {
                NewPos.Y = (Position.Y - (Color * 2));
                NewPos.X = (Position.X);
                if (!chessboard.SquareOccupied(NewPos) && NewPos.WithinBounds()) return NewPos;
            }
            NewPos.Y = (Position.Y - Color);
            NewPos.X = (Position.X);
            if (!chessboard.SquareOccupied(NewPos) && NewPos.WithinBounds()) return NewPos;
            return new Position();
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
            NewPosition = newpos;
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
        public Bishop(int id, Position position, int color) : base(id, position, color)
        {
            ID = id;
            Color = color;
            Position = position;
            NewPosition = new Position(0, 0);
            Value = 3;
            Type = 2;

        }

        public override bool CanMove(Chessboard chessboard)
        {
            Position testPosition = new Position();
            //Check Square in four diagonal axis
            testPosition.Y = (Position.Y + 1);
            testPosition.X = (Position.X + 1);
            if (!chessboard.SquareOccupied(testPosition) && testPosition.WithinBounds()) return true;
            testPosition.Y = (Position.Y + 1);
            testPosition.X = (Position.X - 1);
            if (!chessboard.SquareOccupied(testPosition) && testPosition.WithinBounds()) return true;
            testPosition.Y = (Position.Y - 1);
            testPosition.X = (Position.X - 1);
            if (!chessboard.SquareOccupied(testPosition) && testPosition.WithinBounds()) return true;
            testPosition.Y = (Position.Y - 1);
            testPosition.X = (Position.X + 1);
            if (!chessboard.SquareOccupied(testPosition) && testPosition.WithinBounds()) return true;
            return false;
        }

        public override Position LongestPositionToMoveTo(Chessboard chessboard)
        {
            List<Position> _positions = new List<Position>();
            Position NewPos = new Position();
            for (int i = 0; i < 8; i++)
            {
                NewPos.Y = (Position.Y - (Color * i));
                NewPos.X = (Position.X - (Color * i));
                NewPos.Length = i;
                if (chessboard.SquareOccupied(NewPos) || !NewPos.WithinBounds()) break;
            }
            Position NewPos2 = new Position();
            for (int i = 0; i < 8; i++)
            {
                NewPos2.Y = (Position.Y + (Color * i));
                NewPos2.X = (Position.X - (Color * i));
                NewPos2.Length = i;
                if (chessboard.SquareOccupied(NewPos2) || !NewPos2.WithinBounds()) break;
            }
            Position NewPos3 = new Position();
            for (int i = 0; i < 8; i++)
            {
                NewPos3.Y = (Position.Y - (Color * i));
                NewPos3.X = (Position.X + (Color * i));
                NewPos3.Length = i;
                if (chessboard.SquareOccupied(NewPos3) || !NewPos3.WithinBounds()) break;
            }
            Position NewPos4 = new Position();
            for (int i = 0; i < 8; i++)
            {
                NewPos4.Y = (Position.Y + (Color * i));
                NewPos4.X = (Position.X + (Color * i));
                NewPos4.Length = i;
                if (chessboard.SquareOccupied(NewPos4) || !NewPos4.WithinBounds()) break;
            }
            List<Position> listOfPos = new List<Position>() { NewPos, NewPos2, NewPos3, NewPos4 };
            return listOfPos.OrderByDescending(x => x.Length).ToList()[0];
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
        public Knight(int id, Position position, int color) : base(id, position, color)
        {
            ID = id;
            Color = color;
            Position = position;
            NewPosition = new Position(0, 0);
            Value = 3;
            Type = 3;

        }

        public override Position LongestPositionToMoveTo(Chessboard chessboard)
        {
            //Returns Random
            Position testPosition = new Position();
            testPosition.X = Position.X + 2;
            testPosition.Y = Position.Y + 1;
            if (!chessboard.SquareOccupied(testPosition) && testPosition.WithinBounds()) return testPosition;
            testPosition.X = Position.X + 1;
            testPosition.Y = Position.Y + 2;
            if (!chessboard.SquareOccupied(testPosition) && testPosition.WithinBounds()) return testPosition;
            testPosition.X = Position.X - 1;
            testPosition.Y = Position.Y - 2;
            if (!chessboard.SquareOccupied(testPosition) && testPosition.WithinBounds()) return testPosition;
            testPosition.X = Position.X - 2;
            testPosition.Y = Position.Y - 1;
            if (!chessboard.SquareOccupied(testPosition) && testPosition.WithinBounds()) return testPosition;
            testPosition.X = Position.X + 2;
            testPosition.Y = Position.Y - 1;
            if (!chessboard.SquareOccupied(testPosition) && testPosition.WithinBounds()) return testPosition;
            testPosition.X = Position.X + 1;
            testPosition.Y = Position.Y - 2;
            if (!chessboard.SquareOccupied(testPosition) && testPosition.WithinBounds()) return testPosition;
            testPosition.X = Position.X - 2;
            testPosition.Y = Position.Y + 1;
            if (!chessboard.SquareOccupied(testPosition) && testPosition.WithinBounds()) return testPosition;
            testPosition.X = Position.X - 1;
            testPosition.Y = Position.Y + 2;
            if (!chessboard.SquareOccupied(testPosition) && testPosition.WithinBounds()) return testPosition;
            return testPosition;
        }

        public override bool CanMove(Chessboard chessboard)
        {
            Position testPosition = new Position();
            //Check Every Square Possible... Sigh
            testPosition.X = Position.X + 2;
            testPosition.Y = Position.Y + 1;
            if (!chessboard.SquareOccupied(testPosition) && testPosition.WithinBounds()) return true;
            testPosition.X = Position.X + 1;
            testPosition.Y = Position.Y + 2;
            if (!chessboard.SquareOccupied(testPosition) && testPosition.WithinBounds()) return true;
            testPosition.X = Position.X - 1;
            testPosition.Y = Position.Y - 2;
            if (!chessboard.SquareOccupied(testPosition) && testPosition.WithinBounds()) return true;
            testPosition.X = Position.X - 2;
            testPosition.Y = Position.Y - 1;
            if (!chessboard.SquareOccupied(testPosition) && testPosition.WithinBounds()) return true;
            testPosition.X = Position.X + 2;
            testPosition.Y = Position.Y - 1;
            if (!chessboard.SquareOccupied(testPosition) && testPosition.WithinBounds()) return true;
            testPosition.X = Position.X + 1;
            testPosition.Y = Position.Y - 2;
            if (!chessboard.SquareOccupied(testPosition) && testPosition.WithinBounds()) return true;
            testPosition.X = Position.X - 2;
            testPosition.Y = Position.Y + 1;
            if (!chessboard.SquareOccupied(testPosition) && testPosition.WithinBounds()) return true;
            testPosition.X = Position.X - 1;
            testPosition.Y = Position.Y + 2;
            if (!chessboard.SquareOccupied(testPosition) && testPosition.WithinBounds()) return true;
            return false;
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
        public Rook(int id, Position position, int color) : base(id, position, color)
        {
            ID = id;
            Color = color;
            Position = position;
            NewPosition = new Position(0, 0);
            Value = 5;
            Type = 4;

        }

        public override Position LongestPositionToMoveTo(Chessboard chessboard)
        {
            List<Position> _positions = new List<Position>();
            Position NewPos = new Position();
            for (int i = 0; i < 8; i++)
            {
                NewPos.Y = (Position.Y - (Color * i));
                NewPos.X = (Position.X);
                NewPos.Length = i;
                if (chessboard.SquareOccupied(NewPos) || !NewPos.WithinBounds()) break;
            }
            Position NewPos2 = new Position();
            for (int i = 0; i < 8; i++)
            {
                NewPos2.Y = (Position.Y + (Color * i));
                NewPos2.X = (Position.X);
                NewPos2.Length = i;
                if (chessboard.SquareOccupied(NewPos2) || !NewPos2.WithinBounds()) break;
            }
            Position NewPos3 = new Position();
            for (int i = 0; i < 8; i++)
            {
                NewPos3.Y = (Position.Y);
                NewPos3.X = (Position.X - (Color * i));
                NewPos3.Length = i;
                if (chessboard.SquareOccupied(NewPos3) || !NewPos3.WithinBounds()) break;
            }
            Position NewPos4 = new Position();
            for (int i = 0; i < 8; i++)
            {
                NewPos4.Y = (Position.Y);
                NewPos4.X = (Position.X + (Color * i));
                NewPos4.Length = i;
                if (chessboard.SquareOccupied(NewPos4) || !NewPos4.WithinBounds()) break;
            }
            List<Position> listOfPos = new List<Position>() { NewPos, NewPos2, NewPos3, NewPos4 };
            return listOfPos.OrderByDescending(x => x.Length).ToList()[0];
        }

        public override bool CanMove(Chessboard chessboard)
        {
            //Check Square in four adjacent axis
            Position testPosition = new Position();
            testPosition.X = Position.X + 1;
            testPosition.Y = Position.Y;
            if (!chessboard.SquareOccupied(testPosition) && testPosition.WithinBounds()) return true;
            testPosition.X = Position.X;
            testPosition.Y = Position.Y + 1;
            if (!chessboard.SquareOccupied(testPosition) && testPosition.WithinBounds()) return true;
            testPosition.X = Position.X - 1;
            testPosition.Y = Position.Y;
            if (!chessboard.SquareOccupied(testPosition) && testPosition.WithinBounds()) return true;
            testPosition.X = Position.X;
            testPosition.Y = Position.Y - 1;
            if (!chessboard.SquareOccupied(testPosition) && testPosition.WithinBounds()) return true;
            return false;
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
        public Queen(int id, Position position, int color) : base(id, position, color)
        {
            ID = id;
            Color = color;
            Position = position;
            NewPosition = new Position(0, 0);
            Value = 10;
            Type = 5;

        }

        public override Position LongestPositionToMoveTo(Chessboard chessboard)
        {
            List<Position> _positions = new List<Position>();
            Position NewPos = new Position();
            for (int i = 0; i < 8; i++)
            {
                NewPos.Y = (Position.Y - (Color * i));
                NewPos.X = (Position.X);
                NewPos.Length = i;
                if (chessboard.SquareOccupied(NewPos) || !NewPos.WithinBounds()) break;
            }
            Position NewPos2 = new Position();
            for (int i = 0; i < 8; i++)
            {
                NewPos2.Y = (Position.Y + (Color * i));
                NewPos2.X = (Position.X);
                NewPos2.Length = i;
                if (chessboard.SquareOccupied(NewPos2) || !NewPos2.WithinBounds()) break;
            }
            Position NewPos3 = new Position();
            for (int i = 0; i < 8; i++)
            {
                NewPos3.Y = (Position.Y);
                NewPos3.X = (Position.X - (Color * i));
                NewPos3.Length = i;
                if (chessboard.SquareOccupied(NewPos3) || !NewPos3.WithinBounds()) break;
            }
            Position NewPos4 = new Position();
            for (int i = 0; i < 8; i++)
            {
                NewPos4.Y = (Position.Y);
                NewPos4.X = (Position.X + (Color * i));
                NewPos4.Length = i;
                if (chessboard.SquareOccupied(NewPos4) || !NewPos4.WithinBounds()) break;
            }
            Position NewPos5 = new Position();
            for (int i = 0; i < 8; i++)
            {
                NewPos5.Y = (Position.Y - (Color * i));
                NewPos5.X = (Position.X - (Color * i));
                NewPos5.Length = i;
                if (chessboard.SquareOccupied(NewPos5) || !NewPos5.WithinBounds()) break;
            }
            Position NewPos6 = new Position();
            for (int i = 0; i < 8; i++)
            {
                NewPos6.Y = (Position.Y + (Color * i));
                NewPos6.X = (Position.X - (Color * i));
                NewPos6.Length = i;
                if (chessboard.SquareOccupied(NewPos6) || !NewPos6.WithinBounds()) break;
            }
            Position NewPos7 = new Position();
            for (int i = 0; i < 8; i++)
            {
                NewPos7.Y = (Position.Y - (Color * i));
                NewPos7.X = (Position.X + (Color * i));
                NewPos7.Length = i;
                if (chessboard.SquareOccupied(NewPos7) || !NewPos7.WithinBounds()) break;
            }
            Position NewPos8 = new Position();
            for (int i = 0; i < 8; i++)
            {
                NewPos8.Y = (Position.Y + (Color * i));
                NewPos8.X = (Position.X + (Color * i));
                NewPos8.Length = i;
                if (chessboard.SquareOccupied(NewPos8) || !NewPos8.WithinBounds()) break;
            }
            List<Position> listOfPos = new List<Position>() { NewPos, NewPos2, NewPos3, NewPos4, NewPos5, NewPos6, NewPos7, NewPos8 };
            return listOfPos.OrderByDescending(x => x.Length).ToList()[0];
        }

        public override bool CanMove(Chessboard chessboard)
        {
            Position testPosition = new Position();
            //Check Square in four adjacent axis
            testPosition.X = Position.X + 1;
            testPosition.Y = Position.Y;
            if (!chessboard.SquareOccupied(testPosition) && testPosition.WithinBounds()) return true;
            testPosition.X = Position.X;
            testPosition.Y = Position.Y + 1;
            if (!chessboard.SquareOccupied(testPosition) && testPosition.WithinBounds()) return true;
            testPosition.X = Position.X - 1;
            testPosition.Y = Position.Y;
            if (!chessboard.SquareOccupied(testPosition) && testPosition.WithinBounds()) return true;
            testPosition.X = Position.X;
            testPosition.Y = Position.Y - 1;
            if (!chessboard.SquareOccupied(testPosition) && testPosition.WithinBounds()) return true;
            //Check Square in four diagonal axis
            testPosition.Y = (Position.Y + 1);
            testPosition.X = (Position.X + 1);
            if (!chessboard.SquareOccupied(testPosition) && testPosition.WithinBounds()) return true;
            testPosition.Y = (Position.Y + 1);
            testPosition.X = (Position.X - 1);
            if (!chessboard.SquareOccupied(testPosition) && testPosition.WithinBounds()) return true;
            testPosition.Y = (Position.Y - 1);
            testPosition.X = (Position.X - 1);
            if (!chessboard.SquareOccupied(testPosition) && testPosition.WithinBounds()) return true;
            testPosition.Y = (Position.Y - 1);
            testPosition.X = (Position.X + 1);
            if (!chessboard.SquareOccupied(testPosition) && testPosition.WithinBounds()) return true;
            return false;
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
        public King(int id, Position position, int color) : base(id, position, color)
        {
            ID = id;
            Color = color;
            Position = position;
            NewPosition = new Position(0, 0);
            Value = 100;
            Type = 6;
        }

        public override Position LongestPositionToMoveTo(Chessboard chessboard)
        {
            Position testPosition = new Position();
            testPosition.X = Position.X + 1;
            testPosition.Y = Position.Y;
            if (!chessboard.SquareOccupied(testPosition) && testPosition.WithinBounds()) return testPosition;
            testPosition.X = Position.X;
            testPosition.Y = Position.Y + 1;
            if (!chessboard.SquareOccupied(testPosition) && testPosition.WithinBounds()) return testPosition;
            testPosition.X = Position.X - 1;
            testPosition.Y = Position.Y;
            if (!chessboard.SquareOccupied(testPosition) && testPosition.WithinBounds()) return testPosition;
            testPosition.X = Position.X;
            testPosition.Y = Position.Y - 1;
            if (!chessboard.SquareOccupied(testPosition) && testPosition.WithinBounds()) return testPosition;
            return testPosition;
        }

        public override bool CanMove(Chessboard chessboard)
        {
            Position testPosition = new Position();
            testPosition.X = Position.X + 1;
            testPosition.Y = Position.Y;
            if (!chessboard.SquareOccupied(testPosition) && testPosition.WithinBounds()) return true;
            testPosition.X = Position.X;
            testPosition.Y = Position.Y + 1;
            if (!chessboard.SquareOccupied(testPosition) && testPosition.WithinBounds()) return true;
            testPosition.X = Position.X - 1;
            testPosition.Y = Position.Y;
            if (!chessboard.SquareOccupied(testPosition) && testPosition.WithinBounds()) return true;
            testPosition.X = Position.X;
            testPosition.Y = Position.Y - 1;
            if (!chessboard.SquareOccupied(testPosition) && testPosition.WithinBounds()) return true;
            return false;
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