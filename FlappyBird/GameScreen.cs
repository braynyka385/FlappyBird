using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlappyBird
{
    public partial class GameScreen : UserControl
    {
        Player player;
        Scenery[] bottomScenery = new Scenery[3];
        public GameScreen()
        {
            InitializeComponent();
            Bitmap[] playerSprites = new Bitmap[3];
            playerSprites[0] = new Bitmap(Properties.Resources.bird1l);
            playerSprites[1] = new Bitmap(Properties.Resources.bird2l);
            playerSprites[2] = new Bitmap(Properties.Resources.bird3l);

            player = new Player(this.Height / 2, 5, 100, playerSprites);
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            player.Animate(gameTimer.Interval);   
            Refresh();
        }

        private void GameScreen_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(player.currentSprite, this.Width / 2 - player.currentSprite.Width / 2, (int)player.y);
        }

        private void GameScreen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Space:
                    gameTimer.Enabled = true;
                    instructLabel.Visible = false;
                    player.animationEnabled = true;
                    break;
            }
        }
    }
}
