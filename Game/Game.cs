﻿using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO; //czytnie plikow
using System.Drawing.Text; //zmiana na wybrany font

namespace Game
{
    public partial class Game : Form
    {
        int mouseX, mouseY;

        //na potrzeby dialogow
        bool cont = false;
        bool reading = false;
        public string path = String.Empty;
        PrivateFontCollection pfc = new PrivateFontCollection();

        PlayerCharacter protagonist;

        public Game()
        {
            InitializeComponent();
            pfc.AddFontFile("VCR.ttf");
            protagonist = new PlayerCharacter(Player,PlayerSpells);
            PlayerSpells.Visible = false;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var top = Player.Top - ((this.Height / 2) - (Player.Height / 2));
            var left = Player.Left - ((this.Width / 2) - (Player.Width / 2));

            Player.Parent.Top = -top;
            Player.Parent.Left = -left;
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            var top = Player.Top - ((this.Height / 2) - (Player.Height / 2));
            var left = Player.Left - ((this.Width / 2) - (Player.Width / 2));

            Player.Parent.Top = -top;
            Player.Parent.Left = -left;
        }

        //input
        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (pnlText.Visible != true && pnlInv.Visible != true) //wylaczenie ruchu przy czytaniu dialogow
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
                if (e.KeyCode == Keys.E)
                {
                    protagonist.castSpell = true;
                }
                if (e.KeyCode == Keys.D1)
                {
                    protagonist.currentSpell = PlayerCharacter.Spells.Earth;
                }
                if (e.KeyCode == Keys.D2)
                {
                    protagonist.currentSpell = PlayerCharacter.Spells.Fire;
                }
                if (e.KeyCode == Keys.D3)
                {
                    protagonist.currentSpell = PlayerCharacter.Spells.Ice;
                }
                if (e.KeyCode == Keys.Q)
                {
                    protagonist.currentSpell = PlayerCharacter.Spells.None;
                }
            }  

            if (e.KeyCode == Keys.Space)
            {
                protagonist.action = true;
                if (reading)
                    cont = true;
            }

            if (e.KeyCode == Keys.I)
            {
                if (pnlInv.Visible)
                    pnlInv.Visible = false;
                else
                {
                    updateInv();
                    pnlInv.BringToFront();
                    pnlInv.Visible = true;
                }
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
                protagonist.castSpell = false;
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
                if (thisPictureBoxTag.Equals("stick"))
                {
                    if (Player.Bounds.IntersectsWith(thisPictureBox.Bounds))
                    {
                        if (protagonist.action)
                        {
                            protagonist.Items.InsertItem("Stick");
                            thisPictureBox.Dispose();
                        }
                    }
                }
                if (thisPictureBoxTag.Equals("coin"))
                {
                    if (Player.Bounds.IntersectsWith(thisPictureBox.Bounds))
                    {
                        if (protagonist.action)
                        {
                            protagonist.Items.InsertItem("Coin");
                            thisPictureBox.Dispose();
                        }
                    }
                }
                if (thisPictureBoxTag.Equals("pot"))
                {
                    if (Player.Bounds.IntersectsWith(thisPictureBox.Bounds))
                    {
                        if (protagonist.action)
                        {
                            protagonist.Items.InsertItem("Pot");
                            thisPictureBox.Dispose();
                        }
                    }
                }
                if (thisPictureBoxTag.Equals("sugar"))
                {
                    if (Player.Bounds.IntersectsWith(thisPictureBox.Bounds))
                    {
                        if (protagonist.action)
                        {
                            protagonist.Items.InsertItem("Sugar");
                            thisPictureBox.Dispose();
                        }
                    }
                }
                if (thisPictureBoxTag.Equals("crystal"))
                {
                    if (Player.Bounds.IntersectsWith(thisPictureBox.Bounds))
                    {
                        if (protagonist.action)
                        {
                            protagonist.Items.InsertItem("crystal");
                            thisPictureBox.Dispose();
                        }
                    }
                }

                if (thisPictureBoxTag.Equals("NPC"))
                {
                    if (reading != true)
                    {
                        if (Player.Bounds.IntersectsWith(thisPictureBox.Bounds))
                        {
                            if (protagonist.action)
                            {
                                protagonist.Items.InsertItem("Stick");
                                thisPictureBox.Dispose();
                            }
                        }
                    }
                }
                if (thisPictureBoxTag.Equals("coin"))
                {
                    if (Player.Bounds.IntersectsWith(thisPictureBox.Bounds))
                    {
                        if (reading != true)
                        {
                            if (Player.Bounds.IntersectsWith(thisPictureBox.Bounds))
                            {
                                if (protagonist.action)
                                {
                                    protagonist.Items.InsertItem("Coin");
                                    thisPictureBox.Dispose();
                                }
                            }
                        }
                    }
                }
                if (thisPictureBoxTag.Equals("pot"))
                {
                    if (Player.Bounds.IntersectsWith(thisPictureBox.Bounds))
                    {
                        if (protagonist.action)
                        {
                            protagonist.Items.InsertItem("Pot");
                            thisPictureBox.Dispose();
                        }
                    }
                }
                if (thisPictureBoxTag.Equals("sugar"))
                {
                    if (Player.Bounds.IntersectsWith(thisPictureBox.Bounds))
                    {
                        if (protagonist.action)
                        {
                            protagonist.Items.InsertItem("Sugar");
                            thisPictureBox.Dispose();
                        }
                    }
                }
                if (thisPictureBoxTag.Equals("crystal"))
                {
                    if (Player.Bounds.IntersectsWith(thisPictureBox.Bounds))
                    {
                        if (protagonist.action)
                        {
                            protagonist.Items.InsertItem("crystal");
                            thisPictureBox.Dispose();
                        }
                    }
                }

                //wczytywanie dialogow dla wszystkich npc
                if (thisPictureBoxTag.Equals("NPC"))
                {
                    if (reading != true)
                    {
                        if (Player.Bounds.IntersectsWith(thisPictureBox.Bounds))
                        {
                            if (protagonist.action)
                            {
                                //dobranie path zaleznej od nazwy npc
                                //ewentualnie przerzucic na switch jesli bedzie duzo npc
                                if (thisPictureBox.Name == "George")
                                    path = "../Resources/Dialogs/George.txt";

                                else if (thisPictureBox.Name == "Bunny")
                                {
                                    if (protagonist.Items.FindItem("Carrot") == null || protagonist.Items.FindItem("Carrot").amount < 2)
                                    {
                                        statusLabel.Text = "More carrots!";
                                        path = "../Resources/Dialogs/NPCIncompleteQuest.txt";
                                    }
                                    else
                                    {
                                        statusLabel.Text = "You completed the quest!";
                                        path = "../Resources/Dialogs/NPCFinishedQuest.txt";
                                        protagonist.Items.RemoveItem("Carrot");
                                        thisPictureBox.Dispose();
                                    }
                                }
                                //przekazanie path do funckji wczytujacej dialogi
                                StreamReader sr = new StreamReader(path);
                                LoadDialog(sr);
                                sr.Close();
                            }
                        }
                    }
                }
            }
        }

        //czytanie pojedynczej linii z pliku z mozliwoscia przewijania
        private void ReadLine(string ln)
        {
            bool readAll = false;
            //Animacja Tekstu
            string ln2 = String.Empty;
            for (int i = 0; i < ln.Length; i++)
            {
                ln2 = ln2.Insert(ln2.Length, ln[i].ToString());
                lblDialog.Text = ln2;
                Application.DoEvents();
                System.Threading.Thread.Sleep(50);
                if (cont) //przewinięcie
                    break;
                if (i == (ln.Length) - 1) //jesli wczytany cały dialog, nie wczytuj w następnym if
                    readAll = true;
            }

            if (readAll != true) //jeśli przewinięto, wczytaj cały dialog na raz
            {
                lblDialog.Text = ln;
                cont = false;
            }
        }

        //pokazywanie panelu z dialogami, przejscie do funkcji czytajacej linie z pliku i zamkniecie okna
        private void LoadDialog(StreamReader sr)
        {
            pnlText.BringToFront();
            pnlText.Visible = true;
            lblDialog.Font = new Font(pfc.Families[0], 16);
            string ln = String.Empty;

            reading = true;
            while ((ln = sr.ReadLine()) != null)
            {

                ReadLine(ln);
                while (cont != true)
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(50);
                }
                cont = false;
            }

            lblDialog.Text = "";
            pnlText.Visible = false;
            reading = false;
            System.Threading.Thread.Sleep(100); //zbyt powolne wciśnięcie spacji sprawiało że dialog jednocześnie kończył się i zaczynał od nowa, sleep ma temu przeciwdziałać
        }

        private void updateInv()
        {
            Item cur;
            string path;
            int amount;
            for (int i=0;i< protagonist.Items.ListSize(); i++)
            {
                cur = protagonist.Items.ReturnItem(i);
                path = "../Resources/Items/" + cur.name + ".jpg"; //nazwa itemu jest takze nazwa pliku
                switch (i) //dodac jesli zwiekszy sie liczba dostepnych itemow w grze
                {
                    case 0: 
                        item1.LoadAsync(@path);
                        item1Lbl.Text = (cur.amount).ToString();
                        break;
                    case 1:
                        item2.LoadAsync(@path);
                        item2Lbl.Text = (cur.amount).ToString();
                        break;
                    case 2:
                        item2.LoadAsync(@path);
                        item2Lbl.Text = (cur.amount).ToString();
                        break;
                    case 3:
                        item4.LoadAsync(@path);
                        item4Lbl.Text = (cur.amount).ToString();
                        break;
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

        private void freezablelvl2_2_Click(object sender, EventArgs e)
        {

        }

        private void riverlvl2_4_Click(object sender, EventArgs e)
        {

        }

        private void freezablelvl2_6_Click(object sender, EventArgs e)
        {

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            foreach (Control f in Player.Parent.Controls)
            {
                if (f is PictureBox && f.Tag != null && (f.Tag.ToString().StartsWith("fireball") || f.Tag.ToString().StartsWith("iceball") || f.Tag.ToString() == "burning_object"))
                {
                    f.Dispose();
                }
            }
        }

    }
}
