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
        public bool InputLoop(Chessboard chessboard)
        {
            bool breakLoop = false;

            string input = cReadLine();

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

                        var bp = castAsCorrectPiece(chessboard.getPiece(fromPosition));

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
                                chessboard.movePiece(fromPosition, toPosition);
                            }
                        }
                    }
                }

                InputLoop(chessboard);
            }
            else if (input == "PRESENT")
            {
                Console.WriteLine(PresentationManager.PresentBoard(chessboard));
                InputLoop(chessboard);
            }

            return breakLoop;
        }

        public static dynamic castAsCorrectPiece(BasePiece bp)
        {
            if (bp.Type == 1) return (Pawn)bp;
            if (bp.Type == 2) return (Bishop)bp;
            if (bp.Type == 3) return (Knight)bp;
            if (bp.Type == 4) return (Rook)bp;
            if (bp.Type == 5) return (Queen)bp;
            if (bp.Type == 6) return (King)bp;
            return bp;
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

        public static string cReadLine()
        {
            return Console.ReadLine().Trim().ToUpper();
        }
    }

}
