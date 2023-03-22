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
        private bool hasScored = false;
        public int x;
        public int y;
        public int yGap;
        public int width = 50;
        public static SolidBrush sb = new SolidBrush(Color.GreenYellow);
        static Random random = new Random();

        public Rectangle topOb;
        public Rectangle bottomOb;

        public Obstacle(int _x)
        {
            this.x = _x;
            this.y = random.Next(000, 450);
            this.yGap = random.Next(170, 300);

            topOb = new Rectangle(x, 0, width, y);
            bottomOb = new Rectangle(x, y + yGap, width, GameScreen.screenHeight);
        }
        public bool Contains(Rectangle r)
        {
            if(topOb.IntersectsWith(r) || bottomOb.IntersectsWith(r))
                return true;
            return false;
        }
        public bool Score(double pX)
        {
            if(hasScored)
                return false;
            hasScored = pX > x;
            return hasScored;
        }
    }
}
