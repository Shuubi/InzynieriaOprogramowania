using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.IO; //czytnie plikow
using System.Drawing.Text; //zmiana na wybrany font
using WMPLib; //muzyka w tle

namespace Game
{
    public partial class Game : Form
    {
        //na potrzeby dialogow
        bool cont = false;
        bool reading = false;
        public string path = String.Empty;
        PrivateFontCollection pfc = new PrivateFontCollection();

        //na potrzeby inventory
        int curLoc;

        //na potrzeby zapisu stanu gry
        private List<string> depositedItems = new List<string>();

        PlayerCharacter protagonist;

        WindowsMediaPlayer backgroundMusic = new WindowsMediaPlayer();

        public Game()
        {
            InitializeComponent();
            pnlStart.BringToFront();
            pfc.AddFontFile("VCR.ttf");
            protagonist = new PlayerCharacter(Player, PlayerSpells, Map);
            PlayerSpells.Visible = false;
            HideLevels();
            Player.BackColor = Color.Transparent;
            backgroundMusic.URL = "music.wav";
            backgroundMusic.settings.setMode("Loop", true);
         
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
                    if(protagonist.EarthLearned)
                    PlayerSpells.Image = Image.FromFile(@"Images/aura_green.gif");
                }
                if (e.KeyCode == Keys.D2)
                {
                    protagonist.currentSpell = PlayerCharacter.Spells.Fire;
                    if (protagonist.FireLearned)
                        PlayerSpells.Image = Image.FromFile(@"Images/aura_red.gif");
                }
                if (e.KeyCode == Keys.D3)
                {
                    protagonist.currentSpell = PlayerCharacter.Spells.Ice;
                    if (protagonist.IceLearned)
                        PlayerSpells.Image = Image.FromFile(@"Images/aura_white.gif");
                }
                if (e.KeyCode == Keys.Q)
                {
                    protagonist.currentSpell = PlayerCharacter.Spells.None;
                }
            }
            else if (pnlText.Visible != true)
            {
                if (e.KeyCode == Keys.A || e.KeyCode == Keys.Left)
                {
                    if (curLoc > 0)
                    {
                        invCursor.Location = new Point(invCursor.Location.X - 95, invCursor.Location.Y);
                        curLoc--;
                    }
                }
                if (e.KeyCode == Keys.D || e.KeyCode == Keys.Right)
                {
                    if (curLoc < protagonist.Items.ListSize() - 1)
                    {
                        invCursor.Location = new Point(invCursor.Location.X + 95, invCursor.Location.Y);
                        curLoc++;
                    }
                }
            }

            if (e.KeyCode == Keys.Space)
            {
                protagonist.action = true;
                if (reading)
                    cont = true;
                if (pnlInv.Visible && !pnlText.Visible && invCursor.Visible)
                {
                    string name = "item";
                    Item cur = protagonist.Items.ReturnItem(curLoc);
                    path = "../Resources/Dialogs/" + cur.name + ".txt";
                    StreamReader sr = new StreamReader(path);
                    LoadDialog(sr, name);
                    sr.Close();
                }
            }

            if (e.KeyCode == Keys.I)
            {
                if (pnlInv.Visible)
                    pnlInv.Visible = false;
                else
                {
                    invCursor.Location = new Point(142, invCursor.Location.Y);
                    updateInv();
                    pnlInv.BringToFront();
                    pnlInv.Visible = true;
                }
            }

            if (e.KeyCode == Keys.Escape)
            {
                if (!pnlPause.Visible)
                {
                    pnlPause.BringToFront();
                    pnlPause.Visible = true;
                }
                else
                {
                    pnlPause.SendToBack();
                    pnlPause.Visible = false;
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
            Player.Image = Image.FromFile(@"Images\1.png");
            Image img = Player.Image;
            switch (protagonist.playerRotation)
            {
                case PlayerCharacter.Directions.Right:
                    Player.Image = Image.FromFile(@"Images\2.png");
                    break;
                case PlayerCharacter.Directions.Left:
                    Player.Image = Image.FromFile(@"Images\3.png");
                    break;
                case PlayerCharacter.Directions.Up:
                    Player.Image = Image.FromFile(@"Images\4.png");
                    break;
                case PlayerCharacter.Directions.Down:
                    Player.Image = Image.FromFile(@"Images\1.png");
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

                //podnoszenie itemow
                if (Player.Bounds.IntersectsWith(thisPictureBox.Bounds))
                {
                    if (protagonist.action)
                    {
                        if (thisPictureBoxTag.Equals("pickable_item"))
                        {
                            string pickedItem = thisPictureBox.Name;
                            depositedItems.Add(pickedItem); //przed zmianą nazwy dodaj do listy usuniętych obiektów
                            pickedItem = pickedItem.Remove(pickedItem.Length - 1); //ostatni znak nazwy okresla numer itemu lub jego wlasciwosc dlatego trzeba go usunac na potrzeby inventory
                            protagonist.Items.InsertItem(pickedItem);
                            thisPictureBox.Dispose();
                            System.Media.SoundPlayer sound = new System.Media.SoundPlayer("../Resources/Sounds/item.wav");
                            sound.Play();
                            if (protagonist.FirstItem)
                            {
                                protagonist.FirstItem = false;
                                StreamReader sr = new StreamReader(@"../Resources/Dialogs/item.txt");
                                LoadDialog(sr, "item");
                                sr.Close();
                            }
                        }
                        if (reading != true)
                        {
                            if (thisPictureBoxTag.Equals("NPC") || thisPictureBoxTag.Equals("DialogItem"))
                            {
                                //deklaracja na potrzeby spriteów
                                string SprName = "item";

                                if (thisPictureBoxTag.Equals("NPC")) //wymagaja wczytania sprite'a
                                {
                                    //dobranie path zaleznej od nazwy npc
                                    //ewentualnie przerzucic na switch jesli bedzie duzo npc
                                    if (thisPictureBox.Name == "George")
                                    {
                                        if (protagonist.EarthLearned == false)
                                        {
                                            protagonist.EarthLearned = true;
                                            path = "../Resources/Dialogs/George.txt";
                                        }
                                        else
                                        {
                                            path = "../Resources/Dialogs/GeorgeAfter.txt";
                                        }
                                    }
                                    else if (thisPictureBox.Name == "Jackalope")
                                    {
                                        if (protagonist.Jasper == true)
                                        {
                                            path = "../Resources/Dialogs/JackalopeAfter.txt";
                                        }
                                        else if (protagonist.Items.FindItem("carrot") == null || protagonist.Items.FindItem("carrot").amount <= 2)
                                            path = "../Resources/Dialogs/JackalopeIncompleteQuest.txt";
                                        else
                                        {
                                            path = "../Resources/Dialogs/JackalopeFinishedQuest.txt";
                                            protagonist.Items.RemoveItem("carrot");
                                            protagonist.Items.InsertItem("key");
                                            protagonist.Jasper = true;
                                        }
                                    }
                                    else if (thisPictureBox.Name == "Altie")
                                    {
                                        if (protagonist.IceLearned == false)
                                        {
                                            protagonist.IceLearned = true;
                                            path = "../Resources/Dialogs/Altie.txt";
                                        }
                                        else
                                            path = "../Resources/Dialogs/AltieAfter.txt";

                                    }
                                    else if (thisPictureBox.Name == "Dragon")
                                    {
                                        if (protagonist.FireLearned == true)
                                            path = "../Resources/Dialogs/DragonAfter.txt";
                                        else if (protagonist.Items.FindItem("coin") == null)
                                        {
                                            path = "../Resources/Dialogs/DragonIncomplete.txt";
                                        }
                                        else
                                        {
                                            protagonist.FireLearned = true;
                                            path = "../Resources/Dialogs/Dragon.txt";
                                            protagonist.Items.RemoveItem("coin");
                                        }
                                    }
                                    else if (thisPictureBox.Name == "Cthulhu")
                                    {
                                        if (protagonist.Cthulhu == true)
                                            path = "../Resources/Dialogs/CthulhuAfter.txt";
                                        else if (protagonist.Items.FindItem("candy") == null)
                                        {
                                            path = "../Resources/Dialogs/CthulhuIncomplete.txt";
                                        }
                                        else
                                        {
                                            path = "../Resources/Dialogs/Cthulhu.txt";
                                            PBCthulhu.Image = Image.FromFile(@"Images\DACthulhu2.png");
                                            Cthulhu.Image = Image.FromFile(@"Images\Cthulhu2.png");
                                            protagonist.Cthulhu = true;
                                            protagonist.Items.RemoveItem("candy");
                                        }
                                    }
                                    else if (thisPictureBox.Name == "Teodor")
                                    {
                                        if (protagonist.Teodor == false)
                                        {
                                            path = "../Resources/Dialogs/Teodor.txt";
                                            protagonist.Teodor = true;
                                        }

                                        else
                                            path = "../Resources/Dialogs/TeodorAfter.txt";
                                    }
                                    SprName = thisPictureBox.Name;
                                }
                                if (thisPictureBoxTag.Equals("DialogItem")) //nie wczytuja sprite'a
                                {
                                    if (thisPictureBox.Name == "craft_area")
                                    {
                                        if (protagonist.Items.FindItem("stick") == null || protagonist.Items.FindItem("stick").amount < 2 || protagonist.Items.FindItem("pot") == null || protagonist.Items.FindItem("sugar") == null)
                                        {
                                            path = "../Resources/Dialogs/CraftIncomplete.txt";
                                        }
                                        else
                                        {
                                            path = "../Resources/Dialogs/CraftComplete.txt";
                                            protagonist.Items.RemoveItem("stick");
                                            protagonist.Items.RemoveItem("pot");
                                            protagonist.Items.RemoveItem("sugar");
                                            protagonist.Items.InsertItem("candy");
                                        }

                                    }
                                    else if (thisPictureBox.Name == "Locked_door1")
                                    {
                                        if (protagonist.Items.FindItem("key") == null)
                                        {
                                            path = "../Resources/Dialogs/lockedDoor.txt";
                                        }
                                        else
                                        {
                                            path = "../Resources/Dialogs/openDoor.txt";
                                            System.Media.SoundPlayer sound = new System.Media.SoundPlayer("../Resources/Sounds/lock.wav");
                                            sound.Play();
                                            thisPictureBox.BackgroundImage = null;
                                            thisPictureBox.Tag = "door_open";
                                            protagonist.Items.RemoveItem("key");
                                        }
                                    }
                                    else if (thisPictureBox.Name == "locked_door2")
                                    {
                                        if (protagonist.Items.FindItem("crystal") == null || protagonist.Items.FindItem("crystal").amount <= 2)
                                        {
                                            path = "../Resources/Dialogs/lockedDoor2.txt";
                                        }
                                        else
                                        {
                                            path = "../Resources/Dialogs/openDoor2.txt";
                                            thisPictureBox.Tag = "door_open";
                                            protagonist.Items.RemoveItem("crystal");
                                            thisPictureBox.BackgroundImage = null;
                                        }
                                    }
                                    else if (thisPictureBox.Name == "doorlvl1_3" || thisPictureBox.Name == "doorlvl2_4" || thisPictureBox.Name == "doorlvl3_2")
                                    {
                                        path = "../Resources/Dialogs/lockedDoor3.txt";
                                    }
                                }
                                //przekazanie path do funckji wczytujacej dialogi
                                StreamReader sr = new StreamReader(path);
                                LoadDialog(sr, SprName);
                                sr.Close();

                                if(path == "../Resources/Dialogs/Teodor.txt")
                                {
                                    lblEnd.Font = new Font(pfc.Families[0], 28);
                                    pnlEnd.Visible = true;
                                    pnlEnd.BringToFront();
                                    System.Threading.Thread.Sleep(160);
                                    string ln = "The End";
                                    string ln2 = String.Empty;
                                    string sound = String.Empty;
                                    for (int i = 0; i < ln.Length; i++)
                                    {
                                        ln2 = ln2.Insert(ln2.Length, ln[i].ToString());
                                        lblEnd.Text = ln2;

                                        Random number = new Random();
                                        int nr = number.Next(20);
                                        sound = "../Resources/Sounds/" + (nr).ToString() + ".wav";
                                        System.Media.SoundPlayer player = new System.Media.SoundPlayer(sound);
                                        player.Play();

                                        Application.DoEvents();
                                        System.Threading.Thread.Sleep(100);
                                    }
                                }
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
            string sound = String.Empty;

            for (int i = 0; i < ln.Length; i++)
            {


                ln2 = ln2.Insert(ln2.Length, ln[i].ToString());
                lblDialog.Text = ln2;

                Random number = new Random();
                int nr = number.Next(20);
                sound = "../Resources/Sounds/" + (nr).ToString() + ".wav";
                System.Media.SoundPlayer player = new System.Media.SoundPlayer(sound);
                player.Play();

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
        private void LoadDialog(StreamReader sr, string name)
        {
            pnlText.Visible = true;
            lblDialog.Font = new Font(pfc.Families[0], 16);
            string ln = String.Empty;

            name = "PB" + name;
            var pictureBox = this.Controls.Find(name, true).FirstOrDefault() as PictureBox;
            if (name != "PBitem")
            {
                pictureBox.Visible = true;
                pictureBox.BringToFront();
            }
            pnlText.BringToFront();

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

            if (name != "PBitem")
            {
                pictureBox.Visible = false;
                pictureBox.SendToBack();
            }
            lblDialog.Text = "";
            pnlText.Visible = false;
            reading = false;
            System.Threading.Thread.Sleep(100); //zbyt powolne wciśnięcie spacji sprawiało że dialog jednocześnie kończył się i zaczynał od nowa, sleep ma temu przeciwdziałać
        }

        private void updateInv()
        {
            curLoc = 0;
            if (protagonist.Items.ListSize() != 0)
                invCursor.Visible = true;
            Item cur;
            string path;
            for (int i = 0; i < protagonist.Items.ListSize(); i++)
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
                        item3.LoadAsync(@path);
                        item3Lbl.Text = (cur.amount).ToString();
                        break;
                    case 3:
                        item4.LoadAsync(@path);
                        item4Lbl.Text = (cur.amount).ToString();
                        break;
                    case 4:
                        item5.LoadAsync(@path);
                        item5Lbl.Text = (cur.amount).ToString();
                        break;
                    case 5:
                        item6.LoadAsync(@path);
                        item6Lbl.Text = (cur.amount).ToString();
                        break;
                    case 6:
                        item7.LoadAsync(@path);
                        item7Lbl.Text = (cur.amount).ToString();
                        break;
                    case 7:
                        item8.LoadAsync(@path);
                        item8Lbl.Text = (cur.amount).ToString();
                        break;
                }
            }
            if (protagonist.Items.ListSize() < 7)
            {
                for (int i = protagonist.Items.ListSize(); i < 7; i++)
                {
                    path = "../Resources/Items/black.jpg";
                    switch (i) //dodac jesli zwiekszy sie liczba dostepnych itemow w grze
                    {
                        case 0:
                            item1.LoadAsync(@path);
                            item1Lbl.Text = "";
                            break;
                        case 1:
                            item2.LoadAsync(@path);
                            item2Lbl.Text = "";
                            break;
                        case 2:
                            item3.LoadAsync(@path);
                            item3Lbl.Text = "";
                            break;
                        case 3:
                            item4.LoadAsync(@path);
                            item4Lbl.Text = "";
                            break;
                        case 4:
                            item5.LoadAsync(@path);
                            item5Lbl.Text = "";
                            break;
                        case 5:
                            item6.LoadAsync(@path);
                            item6Lbl.Text = "";
                            break;
                        case 6:
                            item7.LoadAsync(@path);
                            item7Lbl.Text = "";
                            break;
                        case 7:
                            item8.LoadAsync(@path);
                            item8Lbl.Text = "";
                            break;
                    }
                }

            }

        }

        //zapisywanie do pliku
        private void saveGame()
        {
            path = @"SaveFiles/1.txt";
            FileStream fs = File.Create(path);
            fs.Close();
            string dane = String.Empty;
            Item cur;

            //lokalizacja
            /*using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine((Map.Location.X).ToString());
                sw.WriteLine((Map.Location.Y).ToString());
                sw.WriteLine((Player.Location.X).ToString());
                sw.WriteLine((Player.Location.Y).ToString());
            }*/

            //inventory
            for (int i = 0; i < protagonist.Items.ListSize(); i++)
            {
                cur = protagonist.Items.ReturnItem(i);

                using (StreamWriter sw = File.AppendText(path))
                {
                    dane = cur.name + ';' + cur.amount;
                    sw.WriteLine(dane);
                }
            }

            //obiekty podniesione (do późniejszego usunięcia)
            using (StreamWriter sw = File.AppendText(path))
                sw.WriteLine("-");
            for (int i = 0; i < depositedItems.Count; i++)
            {
                using (StreamWriter sw = File.AppendText(path))
                    sw.WriteLine(depositedItems[i]);
            }

            //stan drzwi
            using (StreamWriter sw = File.AppendText(path))
                sw.WriteLine("-");
            for (int i = 0; i < protagonist.ObjListSize(); i++)
            {
                using (StreamWriter sw = File.AppendText(path))
                    sw.WriteLine(protagonist.ReturnObj(i));
            }

            //rozmowy z npc
            using (StreamWriter sw = File.AppendText(path))
                sw.WriteLine("-");
            if (protagonist.Cthulhu) using (StreamWriter sw = File.AppendText(path)) sw.WriteLine("C");
            if (protagonist.Jasper) using (StreamWriter sw = File.AppendText(path)) sw.WriteLine("J");
            if (protagonist.Teodor) using (StreamWriter sw = File.AppendText(path)) sw.WriteLine("T");

            //ilosc czarow
            using (StreamWriter sw = File.AppendText(path))
                sw.WriteLine("-");
            if (protagonist.EarthLearned == true) using (StreamWriter sw = File.AppendText(path)) sw.WriteLine("E");
            if (protagonist.FireLearned == true) using (StreamWriter sw = File.AppendText(path)) sw.WriteLine("F");
            if (protagonist.IceLearned == true) using (StreamWriter sw = File.AppendText(path)) sw.WriteLine("I");
        }

        private void loadGame(StreamReader sr)
        {
            string dane = String.Empty;
            /*while ((dane = sr.ReadLine()) != null)
            {
                //lokalizacja
                if (i>0 && i<=4)
                {
                    int loc = Int32.Parse(dane);
                    switch(i)
                    {
                        case 1:
                            Map.Location = new Point(loc, Map.Location.Y);
                            break;
                        case 2:
                            Map.Location = new Point(Map.Location.X, loc);
                            break;
                        case 3:
                            Map.Location = new Point(loc, Player.Location.Y);
                            break;
                        case 4:
                            Map.Location = new Point(Player.Location.X, loc);
                            break;
                    }
                }
            }*/

            //inv
            while ((dane = sr.ReadLine()) != null)
            {
                if (dane.Contains("-"))
                    break;

                string[] daneSplit = dane.Split(';');
                string name = daneSplit[0];
                string strAmount = daneSplit[1];
                int amount = Int32.Parse(strAmount);

                protagonist.Items.InsertItem(name, amount);
            }

            //usuwanie podniesionych obiektów
            while ((dane = sr.ReadLine()) != null)
            {
                if (dane.Contains("-"))
                    break;

                depositedItems.Add(dane);
                var pictureBox = this.Controls.Find(dane, true).FirstOrDefault() as PictureBox;
                pictureBox.Dispose();
               
            }

            //edycja tagow obiektow
            while ((dane = sr.ReadLine()) != null)
            {
                if (dane.Contains("-"))
                    break;

                protagonist.addObj(dane);
                string name = dane.Remove(dane.Length - 1);
                int typ = int.Parse(dane.Substring(dane.Length - 1));
                var pictureBox = this.Controls.Find(name, true).FirstOrDefault() as PictureBox;
                switch (typ)
                {
                    case 1:
                        pictureBox.Tag = "door_open";
                        pictureBox.BackgroundImage = null;
                        break;
                    case 2:
                        pictureBox.BackColor = Color.Red;
                        pictureBox.Tag = "burning_object";
                        break;
                    case 3:
                        pictureBox.BackgroundImage = Image.FromFile(@"Images\ice.png");
                        pictureBox.Tag = "ice";
                        break;
                }
            }

            //stan dialogow npc
            while ((dane = sr.ReadLine()) != null)
            {
                if (dane.Contains("-"))
                    break;
                switch (dane)
                {
                    case "C":
                        protagonist.Cthulhu = true;
                        PBCthulhu.Image = Image.FromFile(@"Images\DACthulhu2.png");
                        Cthulhu.Image = Image.FromFile(@"Images\Cthulhu2.png");
                        break;
                    case "J":
                        protagonist.Jasper = true;
                        break;
                    case "T":
                        protagonist.Teodor = true;
                        break;
                }
            }

            //odblokowywanie czarow
            while ((dane = sr.ReadLine()) != null)
            {
                switch (dane)
                {
                    case "E":
                        protagonist.EarthLearned = true;
                        break;
                    case "F":
                        protagonist.FireLearned = true;
                        break;
                    case "I":
                        protagonist.IceLearned = true;
                        break;
                }
            }

        }

        public void HideLevels()
        {
            foreach (PictureBox x in Player.Parent.Controls)
            {
                if (x.Tag == "cover")
                    x.BringToFront();
            }
        }
  
        public void ShowLevels()
        {
            foreach (PictureBox x in Player.Parent.Controls)
            {
                if (x.Tag == "door_open")
                {
                    string name = x.Name + "cover";

                    foreach (PictureBox c in Player.Parent.Controls)
                    {
                        if (c.Tag == "cover" && c.Name.ToString().StartsWith(name))
                            c.Dispose();
                    }
                }
            }
        }

        public void Transparency()
        {
            foreach (Control back in Map.Controls)
            {
                foreach (Control front in Map.Controls)
                {
                    if (back is PictureBox && back.Bounds.Contains(front.Bounds) && !back.Name.ToString().Contains("cover"))
                    {
                        if (front is PictureBox && !front.Name.ToString().Contains("cover") && front.Tag != null && front.Tag != "wall" && front.Tag != "river" &&
                              front.Tag != "ice" && !front.Tag.ToString().Contains("ball"))
                        {
                            front.BackColor = back.BackColor;
                        }

                    }
                }
            }
        }

        public void OpenDoorWithButton()
        {
            System.Media.SoundPlayer sound = new System.Media.SoundPlayer("../Resources/Sounds/lock.wav");

            foreach (Control rock in Map.Controls)
            {
                if (rock is PictureBox && rock.Name.ToString().Contains("rock"))
                {
                    if (rock.Bounds.IntersectsWith(buttonlvl1.Bounds))
                    {
                        sound.Play();
                        doorlvl1_3.BackgroundImage = null;
                        doorlvl1_3.Tag = "door_open";
                        rock.Name = null;
                        protagonist.addObj("doorlvl1_31");
                    }
                    if (rock.Bounds.IntersectsWith(buttonlvl2.Bounds))
                    {
                        sound.Play();
                        doorlvl2_4.BackgroundImage = null;
                        doorlvl2_4.Tag = "door_open";
                        rock.Name = null;
                        protagonist.addObj("doorlvl2_41");
                    }
                    if (rock.Bounds.IntersectsWith(buttonlvl3.Bounds))
                    {
                        sound.Play();
                        doorlvl3_2.BackgroundImage = null;
                        doorlvl3_2.Tag = "door_open";
                        rock.Name = null;
                        protagonist.addObj("doorlvl3_21");
                    }
                }
            }
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            protagonist.HandlePlayerCharacter();
            ItemInteraction();
            Rotation();
            ShowLevels();
            Transparency();
            OpenDoorWithButton();
           
            protagonist.MovePlayer();

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            pnlStart.Dispose();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            StreamReader sr = new StreamReader(@"SaveFiles/1.txt");
            loadGame(sr);
            sr.Close();

            pnlStart.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            saveGame();
            Application.Exit();
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

        private void btnExit2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            pnlEnd.Dispose();
        }

        private void btnControls_Click_1(object sender, EventArgs e)
        {
            pnlControls.BringToFront();
            pnlControls.Visible = true;
            Game.ActiveForm.KeyPreview = true;
        }

        private void exitControls_Click(object sender, EventArgs e)
        {
            pnlControls.SendToBack();
            pnlControls.Visible = false;
        }

        private void unpause_Click(object sender, EventArgs e)
        {
            pnlPause.SendToBack();
            pnlPause.Visible = false;
        }

        private void Game_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(1);
            //calkowicie wylacza aplikacje nawet w trakcie wczytywania dialogu z pliku
        }
    }


}
