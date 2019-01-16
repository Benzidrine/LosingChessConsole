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
        public bool MakeDecision(Chessboard chessboard, int AIColor)
        {
            bool MadeMove = false;

            //Check if AI must take a piece
            MadeMove = EnforcedMove(MadeMove, chessboard, AIColor);
            //If AI doesn't have to take then make a move
            if (MadeMove == false)
            {
                MadeMove = MakeMove(MadeMove, chessboard, AIColor);
            }

            return MadeMove;
        }

        public bool MakeMove(bool MadeMove, Chessboard chessboard, int AIColor)
        {
            List<int> IDsThatCanMove = new List<int>();
            List<BasePiece> AIpiecesThatCanMove = new List<BasePiece>();

            //Take out captured pieces
            List <BasePiece> AIpieces = chessboard.ListOfPieces.Where(x => x.HasBeenCaptured == false).ToList();
            //Take out all but AI controlled pieces
            AIpieces = chessboard.ListOfPieces.Where(x => x.Color == AIColor).ToList();

            foreach(BasePiece bp in AIpieces)
            {
                var castPiece = ExplicitCast.castAsCorrectPiece(bp);
                if (castPiece.CanMove(chessboard)) AIpiecesThatCanMove.Add(bp);
            }

            if (AIpiecesThatCanMove.Count() == 0) return false;

            AIpiecesThatCanMove.OrderBy(x => x.Value);

            Position pos = AIpiecesThatCanMove[0].Position;
            Position newpos = AIpiecesThatCanMove[0].LongestPositionToMoveTo(chessboard);
            int id = AIpiecesThatCanMove[0].ID;

            chessboard.movePiece(pos, newpos);

            return true;
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
            // remove piece that is captured then loop through and change position of aiPiece
            chessboard.ListOfPieces.RemoveAll(p => p.Position == Decision.New);
            BasePiece sbp = chessboard.ListOfPieces.FirstOrDefault(p => p.Position == Decision.Original);
            sbp.HasNotMoved = false;
            sbp.Position = Decision.New;
        }
    }
}
