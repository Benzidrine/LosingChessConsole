using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosingChessConsoleApp.Models
{
    //Obviously an enum in practice but to avoid explicit casts I have placed in like this to help the compiler
    public static class PieceColor
    {
        public const int Black = -1;
        public const int White = 1;
    }
}
