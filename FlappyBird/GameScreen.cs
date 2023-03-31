using System;
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
        List<Obstacle> birds = new List<Obstacle>();
        MediaPlayer flapSound = new MediaPlayer();
        MediaPlayer music = new MediaPlayer();
        string path = Application.StartupPath;
        public static int screenHeight;
        public static int score;
        int obsGap = 800;

        int level = 0;

        Random random = new Random();

        public GameScreen()
        {
            InitializeComponent();


            Obstacle.flagSprite = ScaleBitmap(Properties.Resources.chinaflag, 10);

            path = path.Substring(0, path.Length - 10);
            flapSound.Open(new Uri(path + "\\Resources\\flap.wav"));
            music.Volume = 2;
            music.Open(new Uri(path + "\\Resources\\flappysongextended.wav"));
            music.Play();

            Bitmap[] playerSprites = new Bitmap[3];
            playerSprites[0] = new Bitmap(ScaleBitmap(Properties.Resources.chinabloon3l, 1));
            playerSprites[1] = new Bitmap(ScaleBitmap(Properties.Resources.chinabloon1l, 1));
            playerSprites[2] = new Bitmap(ScaleBitmap(Properties.Resources.chinabloon2l, 1));


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

                cloud.sprite = ScaleBitmap(cloud.sprite, 6);

                clouds[i] = cloud;
            }
            score = 0;//
        }

        public Bitmap ScaleBitmap(Bitmap original, double byAmt)
        {
            return new Bitmap(original, new Size((int)(original.Width / byAmt), (int)(original.Height / byAmt)));
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

        private void ChangeLevel()
        {
            switch (level)
            {
                case 1:
                    for(int i = 0; i < bottomScenery.Length; i++)
                    {
                        bottomScenery[i] = new Scenery(Properties.Resources.ocean2, i * Properties.Resources.ocean2.Width, this.Height - 200, 0.875);
                    }
                    for(int i = 0; i < 7; i++)
                    {
                        Obstacle o = new Obstacle((int)(random.Next(0, this.Width) + this.Width + player.x));
                        o.y = random.Next(0, this.Height);
                        birds.Add(o);
                        
                    }
                    Obstacle.sprite = Properties.Resources.bird;
                    break;
            }
        }

        private void ObstacleLogic()
        {

        }
        private void gameTimer_Tick(object sender, EventArgs e)
        {
            if(score >= 10000000000000 && level == 0)
            {
                level++;
                ChangeLevel();
            }
            player.Animate(gameTimer.Interval);
            player.ApplyGravity(0.8);
            player.Move();
            player.boundingBox.X = this.Width / 2 - player.currentSprite.Width / 2 + player.boundingBox.X;
            if(level != 1)
            {
                for (int i = obstacles.Count - 1; i >= 0; i--)
                {
                    Rectangle checkBox = player.boundingBox;
                    checkBox.X -= this.Width / 2 - (int)player.x;
                    if (obstacles[i].Contains(checkBox) || player.y > this.Height || player.y < 0 - player.currentSprite.Height * 2)
                    {
                        GameOver();
                    }

                    if (obstacles[i].Score(player.x))
                    {
                        score++;
                        player.HasScored();
                    }
                    if (obstacles[i].x - player.x + this.Width / 2 + obstacles[i].width + Obstacle.flagSprite.Width < 0)
                    {
                        obstacles.RemoveAt(i);
                        Obstacle o = new Obstacle(obstacles[obstacles.Count - 1].x + obsGap);
                        obstacles.Add(o);
                    }


                }
            }
            else
            {
                for (int i = obstacles.Count - 1; i >= 0; i--)
                {
                    Rectangle checkBox = player.boundingBox;
                    checkBox.X -= this.Width / 2 - (int)player.x;
                    if (obstacles[i].Contains(checkBox) || player.y > this.Height || player.y < 0 - player.currentSprite.Height * 2)
                    {
                        GameOver();
                    }

                    if (obstacles[i].Score(player.x))
                    {
                        score++;
                        player.HasScored();
                    }
                    if (obstacles[i].x - player.x + this.Width / 2 + obstacles[i].width + Obstacle.flagSprite.Width < 0)
                    {
                        obstacles.RemoveAt(i);
                        Obstacle o = new Obstacle(obstacles[obstacles.Count - 1].x + obsGap);
                        obstacles.Add(o);
                    }


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
                    clouds[i].x +=  this.Width + clouds[i].sprite.Width;
                    clouds[i].y = random.Next(0, this.Height / 2);
                }
            }
            
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
                if(level != 0)
                    DrawScenery(e.Graphics, s);

            }
            
            foreach (Scenery s in clouds)
            {
                DrawScenery(e.Graphics, s);
            }
            e.Graphics.DrawImage(player.currentSprite, this.Width / 2 - player.currentSprite.Width / 2, (int)player.y);
            if(level != 1)
            {
                foreach (Obstacle o in obstacles)
                {
                    Rectangle topDrawRect = o.topOb;
                    topDrawRect.X -= (int)player.x - this.Width / 2;
                    Rectangle bottomDrawRect = o.bottomOb;
                    bottomDrawRect.X -= (int)player.x - this.Width / 2;
                    e.Graphics.FillRectangle(Obstacle.sb, topDrawRect);
                    e.Graphics.FillRectangle(Obstacle.sb, bottomDrawRect);
                    e.Graphics.DrawImage(Obstacle.flagSprite, bottomDrawRect.X + bottomDrawRect.Width, bottomDrawRect.Y);
                }
            }
            else
            {
                foreach (Obstacle o in birds)
                {
                    Rectangle birdRect = o.topOb;
                    birdRect.X -= (int)player.x - this.Width / 2;
                    
                    e.Graphics.DrawImage(Obstacle.sprite, birdRect.X + birdRect.Width, birdRect.Y);
                }
            }
            e.Graphics.DrawString(score.ToString(), Form1.flappyFont, new SolidBrush(System.Drawing.Color.FromArgb(255, 0, 0)), new Point(this.Width / 2 - 25, 75));



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
