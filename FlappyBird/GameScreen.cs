﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;

namespace FlappyBird
{
    public partial class GameScreen : UserControl
    {
        Player player;
        Scenery[] bottomScenery = new Scenery[6];
        Scenery[] clouds = new Scenery[8];  
        List<Obstacle> obstacles = new List<Obstacle>();
        MediaPlayer flapSound = new MediaPlayer();
        MediaPlayer music = new MediaPlayer();
        string path = Application.StartupPath;
        public static int screenHeight;
        int score;
        int obsGap = 800;

        Random random = new Random();

        public GameScreen()
        {
            InitializeComponent();

            path = path.Substring(0, path.Length - 10);
            flapSound.Open(new Uri(path + "\\Resources\\flap.wav"));
            music.Volume = 2;
            music.Open(new Uri(path + "\\Resources\\flappysongextended.wav"));
            music.Play();

            Bitmap[] playerSprites = new Bitmap[3];
            playerSprites[0] = new Bitmap(Properties.Resources.bird1l);
            playerSprites[1] = new Bitmap(Properties.Resources.bird2l);
            playerSprites[2] = new Bitmap(Properties.Resources.bird3l);


            screenHeight = this.Height;
            player = new Player(this.Height / 2, 7, 100, playerSprites);
            for(int i = 1; i <= 3; i++)
            {
                Obstacle o = new Obstacle(obsGap * i);
                obstacles.Add(o);

                
            }
            for(int i = 0; i < bottomScenery.Length; i++)
            {
                Scenery s = new Scenery(Properties.Resources.ground, (i) * Properties.Resources.ground.Width,
                    this.Height - Properties.Resources.ground.Height,
                    0.7);
                bottomScenery[i] = s;
            }

            for(int i = 0; i < clouds.Length; i++)
            {
                Scenery cloud = new Scenery(Properties.Resources.cloud2,
                    random.Next(0, this.Width),
                    random.Next(0, this.Height/2),
                    1.125);

                Bitmap original = cloud.sprite;
                Bitmap resized = new Bitmap(original, new Size(original.Width / 6, original.Height / 6));
                cloud.sprite = resized;

                clouds[i] = cloud;
            }
            score = 0;
        }

        private void GameOver()
        {
            player.x = 110;
            player.y = 110;
            music.Stop();
            gameTimer.Stop();
            gameTimer.Enabled = false;
            
            Form1.ChangeScreen(this, new GameOverScreen());
            

        }
        private void gameTimer_Tick(object sender, EventArgs e)
        {
            player.Animate(gameTimer.Interval);
            player.ApplyGravity(0.8);
            player.Move();
            player.boundingBox.X = this.Width / 2 - player.currentSprite.Width / 2 + player.boundingBox.X;

            for(int i = obstacles.Count - 1; i >= 0; i--)
            {
                Rectangle checkBox = player.boundingBox;
                checkBox.X -= this.Width/2 - (int)player.x;
                if (obstacles[i].Contains(checkBox) || player.y > this.Height || player.y < 0 - player.currentSprite.Height * 2)
                {
                    GameOver();
                }

                if (obstacles[i].Score(player.x))
                {
                    score++;
                    player.HasScored();
                }
                if(obstacles[i].x - player.x + this.Width/2 + obstacles[i].width < 0)
                {
                    obstacles.RemoveAt(i);
                    Obstacle o = new Obstacle(obstacles[obstacles.Count - 1].x + obsGap);
                    obstacles.Add(o);
                }
                

            }
            for (int i = 0; i < bottomScenery.Length; i++)
            {
                if (bottomScenery[i].OutOfBounds(player.x, player.y))
                {
                    bottomScenery[i].x += (int)(bottomScenery[i].sprite.Width * (bottomScenery.Length));
                }
            }

            for (int i = 0; i < clouds.Length; i++)
            {
                if (clouds[i].OutOfBounds(player.x, player.y))
                {
                    clouds[i].x +=  this.Width + clouds[i].sprite.Width * 2;
                    clouds[i].y = random.Next(0, this.Height / 2);
                }
            }
            label1.Text = score.ToString();
            
            Refresh();
        }

        private void DrawScenery(Graphics g, Scenery s)
        {
            g.DrawImage(s.sprite, s.x - (int)(player.x * s.parallaxFactor), s.y);
        }
        private void GameScreen_Paint(object sender, PaintEventArgs e)
        {
            foreach (Scenery s in bottomScenery)
            {
                DrawScenery(e.Graphics, s);
            }
            
            foreach (Scenery s in clouds)
            {
                DrawScenery(e.Graphics, s);
            }
            e.Graphics.DrawImage(player.currentSprite, this.Width / 2 - player.currentSprite.Width / 2, (int)player.y);
            foreach (Obstacle o in obstacles)
            {
                Rectangle topDrawRect = o.topOb;
                topDrawRect.X -= (int)player.x - this.Width/2;
                Rectangle bottomDrawRect = o.bottomOb;
                bottomDrawRect.X -= (int)player.x - this.Width/2;
                e.Graphics.FillRectangle(Obstacle.sb, topDrawRect);
                e.Graphics.FillRectangle(Obstacle.sb, bottomDrawRect);
            }

            
        }

        private void GameScreen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Space:
                    gameTimer.Enabled = true;
                    instructLabel.Visible = false;
                    player.Flap();
                    flapSound.Stop();
                    flapSound.Play();
                    break;
            }
        }
    }
}
