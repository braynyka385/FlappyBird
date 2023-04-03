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

            terminalVelocity = 22;
            
        }
        public void Flap()
        { //Sets y speed to be lower when balloon "flaps" (terminology rolled over from when it was still a bird)
            ySpeed -= 10.5;
            if (ySpeed < -terminalVelocity)
                ySpeed = -terminalVelocity;
            animationEnabled = true;
        }
        public void ApplyGravity(double pull)
        { // raises y speed over time to sim gravity
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
        { //increases speed as score increases. Makes game hard over time
            xSpeed *= 1.02;
        }
        public void Animate(int timeElapsed)
        { //Runs through "flap" frames when "flapping"
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
