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
        Scenery[] bottomScenery = new Scenery[3];
        List<Obstacle> obstacles = new List<Obstacle>();
        MediaPlayer flapSound = new MediaPlayer();
        MediaPlayer music = new MediaPlayer();
        string path = Application.StartupPath;
        public static int screenHeight;
        int score;
        int obsGap = 800;
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
            score = 0;
        }

        private void GameOver()
        {

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
            label1.Text = score.ToString();
            
            Refresh();
        }

        private void GameScreen_Paint(object sender, PaintEventArgs e)
        {
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
