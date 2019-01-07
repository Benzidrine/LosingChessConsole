using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosingChessConsoleApp.Models
{
    public class ColumnEnum
    {
    public enum Letter  {A = 1,B,C,D,E,F,G,H};

    public static int returnInt(string letter)
        {
            int ret = -1;
            letter = letter.ToLower();
            switch(letter)
            {
                case "a":
                    ret = 1;
                    break;
                case "b":
                    ret = 2;
                    break;
                case "c":
                    ret = 3;
                    break;
                case "d":
                    ret = 4;
                    break;
                case "e":
                    ret = 5;
                    break;
                case "f":
                    ret = 6;
                    break;
                case "g":
                    ret = 7;
                    break;
                case "h":
                    ret = 8;
                    break;
            }
            return ret;
        }
    }
}
