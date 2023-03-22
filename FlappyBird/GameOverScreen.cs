using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlappyBird
{
    public partial class GameOverScreen : UserControl
    {
        System.Windows.Media.MediaPlayer deathSound = new System.Windows.Media.MediaPlayer();

        public GameOverScreen()
        {
            InitializeComponent();
            string path = Application.StartupPath;
            path = path.Substring(0, path.Length - 10);
            deathSound.Open(new Uri(path + "\\Resources\\death.wav"));
            deathSound.Volume = 1;
            deathSound.Play();
        }
    }
}
