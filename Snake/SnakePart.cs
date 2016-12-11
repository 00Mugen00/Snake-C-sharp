using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    public class SnakePart
    {
        public int x { get; set; }
        public int y { get; set; }
        public SnakePart(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
