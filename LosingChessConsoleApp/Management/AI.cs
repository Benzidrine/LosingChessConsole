using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LosingChessConsoleApp.Models;

namespace LosingChessConsoleApp.Management
{
    public class AI
    {
        public void MakeDecision(Chessboard chessboard, int AIColor)
        {
            bool MadeMove = false;

            MadeMove = EnforcedMove(MadeMove, chessboard, AIColor);
        }

        public bool EnforcedMove(bool MadeMove, Chessboard chessboard, int AIColor)
        {
            List<Choice> Decisions = new List<Choice>();

            //Take out captured pieces
            List<BasePiece> AIpieces = chessboard.ListOfPieces.Where(x => x.HasBeenCaptured == false).ToList();
            //Take out all but AI controlled pieces
            AIpieces = chessboard.ListOfPieces.Where(x => x.Color == AIColor).ToList();
            //Take out all but Player controlled pieces
            List<BasePiece> Playerpieces = chessboard.ListOfPieces.Where(x => x.Color != AIColor).ToList();
            //Check if piece can capture
            foreach (BasePiece bp in AIpieces)
            {
                var castPiece = ExplicitCast.castAsCorrectPiece(bp);
                foreach (BasePiece pp in Playerpieces)
                {
                    if (castPiece.ValidCaptureInjection(pp.Position))
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

            if (Decisions.Count() > 0)
            {
                //Take most valuable piece
                //todo: better determination
                Choice Decision = Decisions.OrderByDescending(item => item.Weight).First();
                MakeCaptureMove(Decision, chessboard);
                MadeMove = true;
            }

            return MadeMove;
        }
        
        private void MakeCaptureMove(Choice Decision, Chessboard chessboard)
        {
            //todo: implement
            // loop through and set piece to is captured then loop through and change position of aiPiece
        }
    }
}
