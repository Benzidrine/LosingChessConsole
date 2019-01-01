using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LosingChessConsoleApp.Models;

namespace LosingChessConsoleApp.Management
{
    class ExplicitCast
    {

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
    }
}
