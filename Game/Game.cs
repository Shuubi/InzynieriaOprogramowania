using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace Game
{
    public partial class Game : Form
    {
        int mouseX, mouseY;

        PlayerCharacter protagonist;

        public Game()
        {
            InitializeComponent();

            protagonist = new PlayerCharacter(Player);
        }

        //input
        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W || e.KeyCode == Keys.Up)
            {
                protagonist.goUp = true;
            }
            if (e.KeyCode == Keys.S || e.KeyCode == Keys.Down)
            {
                protagonist.goDown = true;
            }
            if (e.KeyCode == Keys.A || e.KeyCode == Keys.Left)
            {
                protagonist.goLeft = true;
            }
            if (e.KeyCode == Keys.D || e.KeyCode == Keys.Right)
            {
                protagonist.goRight = true;
            }
            if (e.KeyCode == Keys.Space)
            {
                protagonist.action = true;
            }
            if (e.KeyCode == Keys.E)
            {
                protagonist.fire = true;
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W || e.KeyCode == Keys.Up)
            {
                protagonist.goUp = false;
            }
            if (e.KeyCode == Keys.S || e.KeyCode == Keys.Down)
            {
                protagonist.goDown = false;
            }
            if (e.KeyCode == Keys.A || e.KeyCode == Keys.Left)
            {
                protagonist.goLeft = false;
            }
            if (e.KeyCode == Keys.D || e.KeyCode == Keys.Right)
            {
                protagonist.goRight = false;
            }
            if (e.KeyCode == Keys.Space)
            {
                protagonist.action = false;
            }
            if (e.KeyCode == Keys.E)
            {
                protagonist.fire = false;
            }
        }


        public void Rotation()
        {
            Player.Image = Image.FromFile(@"Images\player.jpg");
            Image img = Player.Image;
            switch (protagonist.playerRotation)
            {
                case PlayerCharacter.Directions.Right:
                    img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    Player.Image = img;
                    break;
                case PlayerCharacter.Directions.Left:
                    img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    Player.Image = img;
                    break;
                case PlayerCharacter.Directions.Up:
                    Player.Image = img;
                    break;
                case PlayerCharacter.Directions.Down:
                    img.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    Player.Image = img;
                    break;
                default: break;
            }
        }

        private void ItemInteraction()
        {

            foreach (Control thisPictureBox in Player.Parent.Controls)
            {
                // If this control is not a picture box, keep looping
                if (!(thisPictureBox is PictureBox)) continue;

                var thisPictureBoxTag = (string)thisPictureBox.Tag;
                if (thisPictureBoxTag == null) continue;

                if (thisPictureBoxTag.Equals("carrot"))
                {
                    if (Player.Bounds.IntersectsWith(thisPictureBox.Bounds))
                    {
                        if (protagonist.action)
                        {
                            statusLabel.Text = "You got a carrot, yay!";
                            protagonist.Items.InsertItem("Carrot");
                            thisPictureBox.Dispose();
                        }
                    }
                }
                if (thisPictureBoxTag.Equals("NPC"))
                {
                    if (Player.Bounds.IntersectsWith(thisPictureBox.Bounds))
                    {
                        if (protagonist.action)
                        {
                            if (protagonist.Items.FindItem("Carrot") == null || protagonist.Items.FindItem("Carrot").amount < 2)
                            {
                                statusLabel.Text = "More carrots!";
                            }
                            else
                            {
                                statusLabel.Text = "You completed the quest!";
                                protagonist.Items.RemoveItem("Carrot");
                                thisPictureBox.Dispose();
                            }
                        }
                    }
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            protagonist.HandlePlayerCharacter();
            ItemInteraction();
            Rotation();

            protagonist.MovePlayer();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            foreach (Control f in Player.Parent.Controls)
            {
                if (f is PictureBox && (f.Tag == "fireball" || f.Tag == "burning_object"))
                {
                    f.Dispose();
                }
            }
        }

    }
}
