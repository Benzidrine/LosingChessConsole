using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosingChessConsoleApp.Models
{
    public class Position
    {

        public Position()
        {
            X = 0;
            Y = 0;
        }

        public Position (int x, int y)
        {
            X = x;
            Y = y;
        }

        public Position (Position pos)
        {
            X = pos.X;
            Y = pos.Y;
        }

        // makes the position equal to the difference between two positions
        public void Diff(Position pos, Position newPos)
        {
            X = (pos.X - newPos.X);
            Y = (pos.Y - newPos.Y);
        }

        public void Add(Position pos)
        {
            X += pos.X;
            Y += pos.Y;
        }

        // makes the position a unit vector
        public void Sign(Position pos)
        {
            if (pos.X != 0)
            {
                X = (pos.X / (Math.Abs(pos.X)));
            }
            else { X = 0;}
            if (pos.Y != 0)
            {
                Y = (pos.Y / (Math.Abs(pos.Y)));
            }
            else
            {
                Y = 0;
            }
            
        }

        public int X { get; set; }
        public int Y { get; set; }
    }
}
