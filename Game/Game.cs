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
        bool goLeft = false;
        bool goRight = false;
        bool goUp = false;
        bool goDown = false;
        bool action = false;
        PlayerCharacter protagonist = new PlayerCharacter();

        public Game()
        {

            InitializeComponent();
        }

        //input
        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W || e.KeyCode == Keys.Up)
            {
                goUp = true;
            }
            if (e.KeyCode == Keys.S || e.KeyCode == Keys.Down)
            {
                goDown = true;
            }
            if (e.KeyCode == Keys.A || e.KeyCode == Keys.Left)
            {
                goLeft = true;
            }
            if (e.KeyCode == Keys.D || e.KeyCode == Keys.Right)
            {
                goRight = true;
            }
            if (e.KeyCode == Keys.Space)
            {
                action = true;
            }

        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W || e.KeyCode == Keys.Up)
            {
                goUp = false;
            }
            if (e.KeyCode == Keys.S || e.KeyCode == Keys.Down)
            {
                goDown = false;
            }
            if (e.KeyCode == Keys.A || e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }
            if (e.KeyCode == Keys.D || e.KeyCode == Keys.Right)
            {
                goRight = false;
            }
            if (e.KeyCode == Keys.Space)
            {
                action = false;
            }
        }

        private void Collision(string tag)
        {
            //wykrywa kolizje z pictureboxami z tagiem przekazanym jako argument funkcji
            foreach (Control x in Player.Parent.Controls)
            {
                if (x is PictureBox && x.Tag == tag)
                {
                    if (Player.Bounds.IntersectsWith(x.Bounds))
                    {
                        if (Player.Right > x.Left && Player.Left < x.Left)
                        {
                            goRight = false;
                        }
                        if (Player.Left < x.Right && Player.Right > x.Right)
                        {
                            goLeft = false;
                        }
                        if (Player.Left + Player.Width > x.Left && Player.Left + Player.Width < x.Left + x.Width + Player.Width && Player.Bottom >= x.Top && Player.Top <= x.Top)
                        {
                            goDown = false;
                        }
                        if (Player.Bottom > x.Bottom && Player.Top <= x.Bottom && Player.Top > x.Top && Player.Left + Player.Width > x.Left && Player.Left + Player.Width < x.Left + x.Width + Player.Width)
                        {
                            goUp = false;
                        }
                    }
                }
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
                        if (action)
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
                        if (action)
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


       
        //ruch gracza/kamery (przesuwanie calej mapy, gracz zostaje na srodku)
        private void MovePlayer()
        {
            if (goRight)
            {
                Map.Left -= 5;
                Player.Left += 5;
            }
            if (goLeft)
            {
                Map.Left += 5;
                Player.Left -= 5;
            }
            if (goUp)
            {
                Map.Top += 5;
                Player.Top -= 5;
            }
            if (goDown)
            {
                Map.Top -= 5;
                Player.Top += 5;
            }

        }

        private void DoorsInteraction()
        {
            foreach (Control x in Player.Parent.Controls)
            {
                if (x is PictureBox && x.Tag == "door_closed")
                {
                    //jesli gracz dotyka drzwi i nacisnie spacje, to drzwi sie usuwaja 
                    if(Player.Bounds.IntersectsWith(x.Bounds) && action)
                    {
                        x.Dispose();
                    }
                }
            }
        }

      
        private void timer1_Tick(object sender, EventArgs e)
        {

            DoorsInteraction();
            ItemInteraction();
            Collision("wall");
            Collision("door_closed");
            MovePlayer();

        }

        
    }
}
