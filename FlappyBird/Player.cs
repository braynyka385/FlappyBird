using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlappyBird
{
    internal class Player
    {
        public double x, y;
        double xSpeed, ySpeed, terminalVelocity;
        Bitmap[] sprites;
        public Bitmap currentSprite;
        public Rectangle boundingBox;
        int spriteIndex;
        int spriteTimer;
        int spriteTimerCeiling;
        public bool animationEnabled = false;
        private byte bbO = 6;

        public Player(double _y, double xSpeed, int _spriteTimerCeiling, Bitmap[] _sprites)
        {
            this.y = _y;
            this.x = 0;
            this.xSpeed = xSpeed;
            this.ySpeed = 0;
            this.sprites = _sprites;
            this.spriteIndex = 0;
            this.spriteTimer = 0;
            this.spriteTimerCeiling = _spriteTimerCeiling;
            currentSprite = sprites[spriteIndex];

            terminalVelocity = 25;
            
        }
        public void Flap()
        {
            ySpeed -= 12;
            if (ySpeed < -terminalVelocity)
                ySpeed = -terminalVelocity;
            animationEnabled = true;
        }
        public void ApplyGravity(double pull)
        {
            ySpeed += pull;
            if(ySpeed > terminalVelocity)
            {
                ySpeed = terminalVelocity;
            }
        }
        public void Move()
        {
            x += xSpeed;
            y += ySpeed;
            boundingBox = new Rectangle(bbO, (int)y + bbO, currentSprite.Width - bbO - bbO, currentSprite.Height - bbO - bbO);
        }
        public void HasScored()
        {
            xSpeed *= 1.02;
        }
        public void Animate(int timeElapsed)
        {
            if (!animationEnabled)
                return;
            spriteTimer+=timeElapsed;
            if(spriteTimer >= spriteTimerCeiling)
            {
                spriteTimer = 0;
                spriteIndex++;
                if(spriteIndex >= sprites.Length)
                {
                    spriteIndex = 0;
                }
                if(spriteIndex == 1)
                {
                    animationEnabled = false;
                }
                currentSprite = sprites[spriteIndex];
            }

        }
    }
}
