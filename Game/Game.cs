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
        int playerSpeed = 5;
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

        private void playerCollision(string tag)
        {
            //wykrywa kolizje gracza z pictureboxami z tagiem przekazanym jako argument funkcji
            foreach (Control x in Player.Parent.Controls)
            {
                if (x is PictureBox && x.Tag == tag)
                {
                    if (Player.Bounds.IntersectsWith(x.Bounds))
                    {
                        if (Player.Right > x.Left && Player.Right < x.Left + (playerSpeed + 1) && Player.Left < x.Left)
                        {
                            goRight = false;
                        }
                        if (Player.Left < x.Right && Player.Left > x.Right - (playerSpeed + 1) && Player.Right > x.Right)
                        {
                            goLeft = false;
                        }
                        if (Player.Bottom >= x.Top && Player.Bottom < x.Top + (playerSpeed + 1) && Player.Top < x.Top)
                        {
                            goDown = false;
                        }
                        if (Player.Top <= x.Bottom && Player.Top > x.Bottom - (playerSpeed + 1) && Player.Bottom > x.Bottom)
                        {
                            goUp = false;
                        }
                    }
                }
            }
        }

        private void pushMovableObjects()
        {
            foreach (Control x in Map.Controls)
            {
                if (x is PictureBox && x.Tag == "movable_object")
                {
                    if (Player.Bounds.IntersectsWith(x.Bounds))
                    {
                        if (Player.Right > x.Left && Player.Right < x.Left + (playerSpeed + 5) && Player.Left < x.Left)
                        { 
                            x.Left += playerSpeed;
                        }
                        if (Player.Left < x.Right && Player.Left > x.Right - (playerSpeed + 5) && Player.Right > x.Right)
                        {
                            x.Left -= playerSpeed;
                        }
                        if (Player.Bottom >= x.Top && Player.Bottom < x.Top + (playerSpeed + 5) && Player.Top < x.Top)
                        {
                            x.Top += playerSpeed;
                        }
                        if (Player.Top <= x.Bottom && Player.Top > x.Bottom - (playerSpeed + 5) && Player.Bottom > x.Bottom)
                        {
                            x.Top -= playerSpeed;
                        }
                    }
                }
            }
        }

        //wykrywa kolizje ruchomych obiektow z innymi obiektami
        private void MovableObjectsCollision()
        {
            //poniewaz ruch ruchomych obiektow jest zalezny tylko od ruchu gracza, podczas kolizji blokujemy ruch gracza w danym kierunku 
            foreach (Control mv in Map.Controls) //mv = movable, objekt ktory sie rusza i ma sie zatrzymac 
            {
                if (mv is PictureBox && mv.Tag == "movable_object")
                {
                    foreach (Control st in Map.Controls) //st = static, po kolizji mv z tym obiektem zatrzymywany jest ruch
                    {
                        if (st is PictureBox)
                        {
                            if (st.Tag == "wall" || st.Tag == "door_closed" || st.Tag == "movable_object")
                            {
                                if (mv.Bounds.IntersectsWith(st.Bounds))
                                {
                                    if (mv.Right > st.Left && mv.Left < st.Left)
                                    {
                                        //gdy dojdzie do kolizji i player sie odsunie, to musimy troche odsunac ruchomy 
                                        //objekt od statycznego, zeby przestal wykrywac kolizje, w innym przypadku ruch bedzie zablokowany nawet po odejsciu playera od obiektu
                                        goRight = false;
                                        if (!Player.Bounds.IntersectsWith(mv.Bounds))
                                        {
                                            mv.Left -= playerSpeed;
                                        }
                                    }
                                    if (mv.Left < st.Right && mv.Right > st.Right)
                                    {
                                        goLeft = false;
                                        if (!Player.Bounds.IntersectsWith(mv.Bounds))
                                        {
                                            mv.Left += playerSpeed;
                                        }
                                    }
                                    if (mv.Bottom >= st.Top && mv.Top < st.Top)
                                    {
                                        goDown = false;
                                        if (!Player.Bounds.IntersectsWith(mv.Bounds))
                                        {
                                            mv.Top -= playerSpeed;
                                        }
                                    }
                                    if (mv.Top <= st.Bottom &&  mv.Bottom > st.Bottom)
                                    {
                                        goUp = false;
                                        if (!Player.Bounds.IntersectsWith(mv.Bounds))
                                        {
                                            mv.Top += playerSpeed;
                                        }
                                    }
                                }
                            }
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
                Map.Left -= playerSpeed;
                Player.Left += playerSpeed;
            }
            if (goLeft)
            {
                Map.Left += playerSpeed;
                Player.Left -= playerSpeed;
            }
            if (goUp)
            {
                Map.Top += playerSpeed;
                Player.Top -= playerSpeed;
            }
            if (goDown)
            {
                Map.Top -= playerSpeed;
                Player.Top += playerSpeed;
            }

        }

        private void DoorsInteraction()
        {
            foreach (Control x in Player.Parent.Controls)
            {
                if (x is PictureBox && x.Tag == "door_closed")
                {
                    //jesli gracz dotyka drzwi i nacisnie spacje, to drzwi sie usuwaja 
                    if (Player.Bounds.IntersectsWith(x.Bounds) && action)
                    {
                        x.Dispose();
                    }
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
           
            MovableObjectsCollision();
            pushMovableObjects();
            DoorsInteraction();
            ItemInteraction();
            playerCollision("wall");
            playerCollision("door_closed");
            MovePlayer();




        }


    }
}
