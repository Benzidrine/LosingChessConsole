using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LosingChessConsoleApp.Models;

namespace LosingChessConsoleApp.Management
{
    public class Player
    {
        public static List<Choice> EnforcedMove(Chessboard chessboard, int AIColor)
        {
            List<Choice> Decisions = new List<Choice>();

            //Take out captured pieces
            List<BasePiece> PlayerPieces = chessboard.ListOfPieces.Where(x => x.Color != AIColor && x.HasBeenCaptured == false).ToList();
            //Take out all but Player controlled pieces
            List<BasePiece> AIpieces = chessboard.ListOfPieces.Where(x => x.Color == AIColor && x.HasBeenCaptured == false).ToList();
            //Check if piece can capture
            foreach (BasePiece bp in PlayerPieces)
            {
                var castPiece = ExplicitCast.castAsCorrectPiece(bp);
                foreach (BasePiece pp in AIpieces)
                {
                    Position InjectPosition = pp.Position;
                    if (castPiece.ValidCaptureInjection(InjectPosition, chessboard.ListOfPieces))
                    {
                        //New Choice
                        Choice ch = new Choice();
                        ch.Weight = pp.Value;
                        ch.Original = bp.Position;
                        ch.New = pp.Position;
                        Decisions.Add(ch);
                    }
                }
            }

            return Decisions;
        }
    }
}
