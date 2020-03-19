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
        const int WindowSizeX = 900;
        const int WindowSizeY = 600;
        bool goLeft = false;
        bool goRight = false;
        bool goUp = false;
        bool goDown = false;

        public Game()
        {
            InitializeComponent();
        }

        //ruch gracza sterowany klawiatura
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
        }

        private void Collision()
        {
            //wykrywa kolizje ze scianami i zatrzymuje ruch gracza w danym kierunku
            foreach (Control x in Player.Parent.Controls)
            {
                if (x is PictureBox && x.Tag == "wall")
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

        private void ChangeLocation()
        {   
            //zmienia lokacje po kolizji z danymi drzwiami 
            //kazda lokacja to osobny panel, przy zmianie lokacji ukrywa obecny panel, dodaje gracza do docelowego panelu i go pokazuje


            foreach (Control x in Player.Parent.Controls)
            {
                if (x is PictureBox && x.Tag == "door")
                {
                    if (Player.Bounds.IntersectsWith(x.Bounds))
                    {
                        Player.Parent.Visible = false;
                       
                        if (x.Name == "Door1")
                        {
                            PanelLevel1.Controls.Add(Player);
                        }
                        else if (x.Name == "Door2")
                        {
                            PanelLevel2.Controls.Add(Player);
                        }
                        else if (x.Name == "Door3")
                        {
                            PanelLevel3.Controls.Add(Player);
                        }
                        else if (x.Name == "Door4")
                        {
                            PanelLevel4.Controls.Add(Player);
                        }
                        else
                        {
                            PanelHub.Controls.Add(Player);
                        }
                        Player.Parent.Location = new Point(0, 0);
                        Player.Parent.Size = new Size(WindowSizeX, WindowSizeY);
                        Player.Parent.Visible = true;
                        Player.Location = new Point(434, 485);
                    }
                }
            }
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            ChangeLocation();
            Collision();
 
            if (goRight)
            {
                Player.Left += 5;
            }
            if (goLeft)
            {
                Player.Left -= 5;
            }
            if (goUp)
            {
                Player.Top -= 5;
            }
            if (goDown)
            {
                Player.Top += 5;
            }

        }


    }
}
