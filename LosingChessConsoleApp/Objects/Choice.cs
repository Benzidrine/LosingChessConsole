using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LosingChessConsoleApp.Models;

namespace LosingChessConsoleApp.Models
{
    // Potential AI Decision
    public class Choice
    {
        public int Weight { get; set; }
        public Position Original { get; set; }
        public Position New { get; set; }
    }
}
