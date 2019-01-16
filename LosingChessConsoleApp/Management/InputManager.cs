using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LosingChessConsoleApp.Models;

namespace LosingChessConsoleApp.Management
{
    public class InputManager
    {
        private int _turn = 0;
        private bool _moveMade = false;
        private int _AIcolor = PieceColor.Black;

        public bool InputLoop(Chessboard chessboard)
        {
            //Check if won
            if (chessboard.ListOfPieces.Where(x => x.Color == _AIcolor && x.HasBeenCaptured == false).Count() == 0)
            {
                Console.WriteLine("You have achieved a great victory over the odds to become a true chess god of absolute greatness");
            }

            //Check if lost
            if (chessboard.ListOfPieces.Where(x => x.Color != _AIcolor && x.HasBeenCaptured == false).Count() == 0)
            {
                Console.WriteLine("You have lost sorry");
            }

            //AI responds to move
            bool AImoved = false;
            AI aI = new AI();
            if (_moveMade) AImoved = aI.MakeDecision(chessboard, _AIcolor);

            //Check if stalemate
            if (_moveMade && !AImoved)
            {
                Console.WriteLine("Unfortunately your hopes have been dashed via a stalemate");
            }

            //Show board after player and AI move
            if (_moveMade) Console.WriteLine(PresentationManager.PresentBoard(chessboard));

            //Reset variables
            _moveMade = false;
            bool breakLoop = false;

            //Check if player must move to capture a piece
            List<Choice> Decisions = Player.EnforcedMove(chessboard, _AIcolor);
            if (Decisions.Count > 0)
            {
                Console.WriteLine("Player must move to capture:");
                foreach (Choice c in Decisions)
                {
                    Console.WriteLine("MOVE " + getInputCodeFromPosition(c.Original) + " " + getInputCodeFromPosition(c.New));
                }
            }

            string input = cReadLine().ToUpper();

            if (input == "MOVE")
            {
                Console.WriteLine("Location of Piece to be moved:");

                string LocationOfPiece = cReadLine();

                //todo: check string length

                if (LocationOfPiece.Length != 2)
                {
                    Console.WriteLine("No!");
                    return breakLoop;
                }
                else
                {
                    Position fromPosition = getPositionFromInputCode(LocationOfPiece);

                    Console.WriteLine("Location to move to:");

                    string LocationToMoveTo = cReadLine();

                    if (LocationOfPiece.Length != 2)
                    {
                        Console.WriteLine("No!");
                        return breakLoop;
                    }
                    else
                    {
                        Position toPosition = getPositionFromInputCode(LocationToMoveTo);

                        var bp = ExplicitCast.castAsCorrectPiece(chessboard.getPiece(fromPosition));

                        //todo better checking
                        if (bp.Position == new Position(0, 0))
                        {
                            Console.WriteLine("No!");
                            return breakLoop;
                        }
                        else
                        {
                            bp.NewPosition = toPosition;
                            //todo check if piece exists in space and can be captured then run capture code instead of move code
                            //ie getpiece from toPosition then check color then validCapture instead
                            bool validMove = bp.ValidMove();
                            if (validMove)
                            {
                                if (chessboard.movePiece(fromPosition, toPosition)) _moveMade = !_moveMade;
                            }
                            else if (bp.ValidCapture() && chessboard.SquareOccupied(toPosition))
                            {
                                if (chessboard.movePiece(fromPosition, toPosition)) _moveMade = !_moveMade;
                            }
                        }
                    }
                }
                if (!_moveMade) Console.WriteLine("Illegal Move");

                InputLoop(chessboard);
            }
            // MOVE A2 A3
            else if (input.Contains("MOVE") && input.Length == 10)
            {
                string LocationOfPiece = input.Substring(5, 2);
                string LocationToMoveTo = input.Substring(8, 2);
                Position fromPosition = getPositionFromInputCode(LocationOfPiece);
                Position toPosition = getPositionFromInputCode(LocationToMoveTo);

                var bp = ExplicitCast.castAsCorrectPiece(chessboard.getPiece(fromPosition));

                if (bp.Position == new Position(0, 0))
                {
                    Console.WriteLine("No!");
                    return breakLoop;
                }
                else
                {
                    bp.NewPosition = toPosition;
                    //todo check if piece exists in space and can be captured then run capture code instead of move code
                    //ie getpiece from toPosition then check color then validCapture instead
                    bool validMove = bp.ValidMove();
                    if (validMove)
                    {
                        if (chessboard.movePiece(fromPosition, toPosition)) _moveMade = !_moveMade;
                    }
                    else if (bp.ValidCapture() && chessboard.SquareOccupied(toPosition))
                    {
                        if (chessboard.movePiece(fromPosition, toPosition)) _moveMade = !_moveMade;
                    }
                }
                if (!_moveMade) Console.WriteLine("Illegal Move");
                InputLoop(chessboard);
            }
            else if (input == "LIST")
            {
                List<BasePiece> ListOfBasePieces = chessboard.ListOfPieces;
                ListOfBasePieces.OrderBy(x => x.Position.Y).OrderBy(x => x.Position.Y);
                foreach (BasePiece bp in ListOfBasePieces)
                {
                    Console.WriteLine("Name: " + bp.ColorIs() + " " + ((BasePiece.PieceName)bp.Type).ToString() + " Position: " + ((ColumnEnum.Letter)bp.Position.X).ToString() + " " + (9 - bp.Position.Y));
                }
                InputLoop(chessboard);
            }
            else if (input == "PRESENT" || input == "P")
            {
                Console.WriteLine(PresentationManager.PresentBoard(chessboard));
                InputLoop(chessboard);
            }
            else if (input == "RESET")
            {
                chessboard = new Chessboard();
                InputLoop(chessboard);
            }
            else if (input == "EXIT")
            {
                return breakLoop;
            }
            else if (input == "HELP" || input == "?")
            {
                PresentationManager.HelpMessage();
                InputLoop(chessboard);
            }
            else
            {
                Console.WriteLine("ILLEGAL COMMAND");
                InputLoop(chessboard);
            }
            return breakLoop;
        }

        public static Position getPositionFromInputCode(string input)
        {
            Position pos = new Position(0, 0);

            int Letter = ColumnEnum.returnInt(input.Substring(0, 1));

            if (Letter == -1)
            {
                return pos;
            }

            int numberOfRow = 0;
            bool successfulParse = int.TryParse(input.Substring(1, 1), out numberOfRow);

            if (!successfulParse)
            {
                return pos;
            }
            else
            {
                // invert number 
                numberOfRow = 9 - numberOfRow;
            }

            pos.X = Letter;
            pos.Y = numberOfRow;

            return pos;
        }

        public string getInputCodeFromPosition(Position pos)
        {
            string Letter = ((ColumnEnum.Letter)pos.X).ToString();
            string NumberRow = (9 - pos.Y).ToString();
            return Letter + NumberRow;
        }

        public static string cReadLine()
        {
            return Console.ReadLine().Trim().ToUpper();
        }
    }

}
