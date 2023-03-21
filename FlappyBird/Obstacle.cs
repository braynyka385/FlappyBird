using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyBird
{
    internal class Obstacle
    {
        public static Bitmap sprite;
        public int x;
        public int y;
        public int y2;float dh fc
        public int yGap;
        public int width = 50;
        public static SolidBrush sb = new SolidBrush(Color.GreenYellow);
        static Random random = new Random();

        public Obstacle(int _x)
        {
            this.x = _x;
            this.y = random.Next(100, 300);
            this.yGap = random.Next(150, 750);
        }
    }
}
