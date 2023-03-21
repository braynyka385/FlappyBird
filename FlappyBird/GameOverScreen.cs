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
        SoundPlayer sound;

        public GameOverScreen()
        {
            InitializeComponent();
            string path = Application.StartupPath;
            string path2 = path.Substring(0, path.Length - 10);
            deathSound.Open(new Uri(path2 + "\\Resources\\death.wav"));
            sound = new SoundPlayer(path2 + "\\Resources\\death.wav");
            deathSound.Volume = 1;
            deathSound.Play();
            //sound.Play();
        }
    }
}
