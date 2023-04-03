using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyBird
{
    internal class Scenery
    {
        public Bitmap sprite;
        public int x, y;
        public double parallaxFactor; //Makes it so that scenery can be given the illusion of "distance" in a 3rd dimension.

        public Scenery(Bitmap _sprite, int _x, int _y, double _parallaxFactor)
        {
            this.sprite = _sprite;
            this.x = _x;
            this.y = _y;
            this.parallaxFactor = _parallaxFactor;
        }


        public bool OutOfBounds(double pX, double pY)
        {
            return this.x - (pX * this.parallaxFactor) + this.sprite.Width < 0; // Checks if the object is no longer visible.
        }
    }
}
