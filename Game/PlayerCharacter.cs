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

    public class PlayerCharacter
    {
        public Control Player;
<<<<<<< Updated upstream
=======
        public Control PlayerSpells;
>>>>>>> Stashed changes
        public bool goRight { get; set; }
        public bool goLeft { get; set; }
        public bool goUp { get; set; }
        public bool goDown { get; set; }
        public bool action { get; set; }
<<<<<<< Updated upstream
        public bool fire { get; set; }
        public Directions playerRotation { get; set; }
=======
        public bool castSpell { get; set; }
        public Directions playerRotation { get; set; }
        public Spells currentSpell { get; set; }
>>>>>>> Stashed changes

        static public int playerSpeed = 6;

        public PlayerItemInventory Items = new PlayerItemInventory();
<<<<<<< Updated upstream

        public enum Directions { Left, Right, Up, Down }

        public PlayerCharacter(Control player)
        {
            Player = player;
=======
        public enum Directions { Left, Right, Up, Down }
        public enum Spells { None, Earth, Fire, Ice }

        public PlayerCharacter(Control player, Control playerspells)
        {
            Player = player;
            PlayerSpells = playerspells;
>>>>>>> Stashed changes
        }

        public void MovePlayer()
        {

            if (goRight)
            {
                Player.Parent.Left -= playerSpeed;
                Player.Left += playerSpeed;
<<<<<<< Updated upstream
=======
                PlayerSpells.Left += playerSpeed;
>>>>>>> Stashed changes
                playerRotation = Directions.Right;

            }
            if (goLeft)
            {
                Player.Parent.Left += playerSpeed;
                Player.Left -= playerSpeed;
<<<<<<< Updated upstream
=======
                PlayerSpells.Left -= playerSpeed;
>>>>>>> Stashed changes
                playerRotation = Directions.Left;
            }
            if (goUp)
            {
                Player.Parent.Top += playerSpeed;
                Player.Top -= playerSpeed;
<<<<<<< Updated upstream
=======
                PlayerSpells.Top -= playerSpeed;
>>>>>>> Stashed changes
                playerRotation = Directions.Up;
            }
            if (goDown)
            {
                Player.Parent.Top -= playerSpeed;
                Player.Top += playerSpeed;
<<<<<<< Updated upstream
=======
                PlayerSpells.Top += playerSpeed;
>>>>>>> Stashed changes
                playerRotation = Directions.Down;
            }

        }


        public void playerCollision(string tag)
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

        public void pushMovableObjects()
        {
<<<<<<< Updated upstream
=======
            MovableObjectsCollision();

>>>>>>> Stashed changes
            foreach (Control x in Player.Parent.Controls)
            {
                if (x is PictureBox && x.Tag == "movable_object")
                {
                    if (action) //gdy gracz ma wcisnieta spacje i podejdzie do movable_object, to moze go przesuwac na wszystkie strony, rowniez ciagnac do siebie (potem mozna dodac warunek znajomosci jakiegos zaklecia)
                    {
                        if (Player.Bounds.IntersectsWith(x.Bounds))
                        {
                            playerSpeed = 3;
                            if (goRight)
                            {
                                x.Left += playerSpeed;
                            }
                            if (goLeft)
                            {
                                x.Left -= playerSpeed;
                            }
                            if (goDown)
                            {
                                x.Top += playerSpeed;
                            }
                            if (goUp)
                            {
                                x.Top -= playerSpeed;
                            }
                        }
                    }
                    else
                    {
                        playerSpeed = 5;
                        playerCollision("movable_object");
                    }
                }
            }
        }

        //wykrywa kolizje ruchomych obiektow z innymi obiektami
        public void MovableObjectsCollision()
        {
            //poniewaz ruch ruchomych obiektow jest zalezny tylko od ruchu gracza, podczas kolizji blokujemy ruch gracza w danym kierunku 
            foreach (Control mv in Player.Parent.Controls) //mv = movable, objekt ktory sie rusza i ma sie zatrzymac 
            {
                if (mv is PictureBox && mv.Tag == "movable_object")
                {
                    foreach (Control st in Player.Parent.Controls) //st = static, po kolizji mv z tym obiektem zatrzymywany jest ruch
                    {
                        if (st is PictureBox)
                        {
                            if (st.Tag == "wall" || st.Tag == "door_closed" || st.Tag == "movable_object")
                            {
                                if (mv.Bounds.IntersectsWith(st.Bounds) && Player.Bounds.IntersectsWith(mv.Bounds))
                                {
                                    if (mv.Right > st.Left && mv.Left < st.Left)
                                    {
                                        goRight = false;
                                    }
                                    if (mv.Left < st.Right && mv.Right > st.Right)
                                    {
                                        goLeft = false;
                                    }
                                    if (mv.Bottom >= st.Top && mv.Top < st.Top)
                                    {
                                        goDown = false;
                                    }
                                    if (mv.Top <= st.Bottom && mv.Bottom > st.Bottom)
                                    {
                                        goUp = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void DoorsInteraction()
        {
            foreach (PictureBox x in Player.Parent.Controls)
            {
                if (x is PictureBox && x.Tag == "door_closed")
                {
                    //jesli gracz dotyka drzwi i nacisnie spacje, to drzwi zmieniaja tag i kolor
                    if (Player.Bounds.IntersectsWith(x.Bounds) && action)
                    {
                        x.Tag = "door_open";
                        x.BackColor = Color.Green;
                    }
                }
            }
        }

        public void Fire()
        {
<<<<<<< Updated upstream
            if (fire)
=======
            if (castSpell)
>>>>>>> Stashed changes
            {
                var directionString = playerRotation.ToString();
                var fireball = new PictureBox
                {
                    Tag = $"fireball|{directionString}",
<<<<<<< Updated upstream
                    Size = new Size(7, 7),
                    Location = new Point(this.Player.Location.X + 25, this.Player.Location.Y + 25),
=======
                    Size = new Size(6, 6),
                    Location = new Point(this.Player.Location.X + 15, this.Player.Location.Y + 15),
>>>>>>> Stashed changes
                    BackColor = Color.Red,
                };
                this.Player.Parent.Controls.Add(fireball);
                fireball.BringToFront();
            }
            foreach (Control f in Player.Parent.Controls)
            {
                if (f is PictureBox && f.Tag != null && f.Tag.ToString().StartsWith("fireball"))
                {
                    var rotation = f.Tag.ToString().Substring("fireball".Length + 1);
                    switch (rotation)
                    {
                        case "Right":
<<<<<<< Updated upstream
                            f.Left += 20;
                            break;
                        case "Left":
                            f.Left -= 20;
                            break;
                        case "Up":
                            f.Top -= 20;
                            break;
                        case "Down":
                            f.Top += 20;
=======
                            f.Left += 15;
                            break;
                        case "Left":
                            f.Left -= 15;
                            break;
                        case "Up":
                            f.Top -= 15;
                            break;
                        case "Down":
                            f.Top += 15;
>>>>>>> Stashed changes
                            break;
                        default: break;
                    }
                    foreach (Control x in Player.Parent.Controls)
                    {
                        if (f.Bounds.IntersectsWith(x.Bounds) && (x.Tag == "wall" || x.Tag == "door_closed"))
                        {
                            f.Dispose();
                        }
                        if (f.Bounds.IntersectsWith(x.Bounds) && x.Tag == "flammable_object")
                        {
                            x.BackColor = Color.Red;
                            x.Tag = "burning_object";
                        }
                    }
<<<<<<< Updated upstream
=======
                }
            }
        }

        public void Ice()
        {
            if (castSpell)
            {
                var directionString = playerRotation.ToString();
                var iceball = new PictureBox
                {
                    Tag = $"iceball|{directionString}",
                    Size = new Size(6, 6),
                    Location = new Point(this.Player.Location.X + 15, this.Player.Location.Y + 15),
                    BackColor = Color.White,
                };
                this.Player.Parent.Controls.Add(iceball);
                iceball.BringToFront();
            }
            foreach (Control f in Player.Parent.Controls)
            {
                if (f is PictureBox && f.Tag != null && f.Tag.ToString().StartsWith("iceball"))
                {
                    var rotation = f.Tag.ToString().Substring("iceball".Length + 1);
                    switch (rotation)
                    {
                        case "Right":
                            f.Left += 15;
                            break;
                        case "Left":
                            f.Left -= 15;
                            break;
                        case "Up":
                            f.Top -= 15;
                            break;
                        case "Down":
                            f.Top += 15;
                            break;
                        default: break;
                    }
                    foreach (Control x in Player.Parent.Controls)
                    {
                        if (f.Bounds.IntersectsWith(x.Bounds) && (x.Tag == "wall" || x.Tag == "door_closed"))
                        {
                            f.Dispose();
                        }
                        if (f.Bounds.IntersectsWith(x.Bounds) && (x.Tag == "freezable_object"))
                        {
                            x.BackColor = Color.White;
                            x.Tag = "ice";
                        }
                    }
>>>>>>> Stashed changes

                }
            }
        }

<<<<<<< Updated upstream
=======

        public void SpellCasting()
        {
            if (currentSpell == Spells.Earth)
            {
                PlayerSpells.BackColor = Color.GreenYellow;
                PlayerSpells.Visible = true;
                pushMovableObjects();
            }
            if (currentSpell == Spells.Fire)
            {
                PlayerSpells.BackColor = Color.Red;
                PlayerSpells.Visible = true;
                Fire();
            }
            if (currentSpell == Spells.Ice)
            {
                PlayerSpells.BackColor = Color.White;
                PlayerSpells.Visible = true;
                Ice();
            }
            if (currentSpell == Spells.None)
            {
                PlayerSpells.Visible = false;
            }


        }

>>>>>>> Stashed changes
        public void HandlePlayerCharacter()
        {
            playerCollision("wall");
            playerCollision("door_closed");
            playerCollision("flammable_object");
<<<<<<< Updated upstream
            MovableObjectsCollision();
            pushMovableObjects();
            DoorsInteraction();
            Fire();
=======
            playerCollision("water");
            playerCollision("river");
            playerCollision("movable_object");
            playerCollision("freezable_object");
            SpellCasting();
            DoorsInteraction();
>>>>>>> Stashed changes
        }


    }


    public class PlayerItemInventory
    {

        private List<Item> _items = new List<Item>();

        /// <summary>
        /// Inserts an item into the players inventory, but if the item already exists - it will update the existing item with the new amount
        /// </summary>
        /// <param name="itemName">The item name to give the player</param>
        /// <param name="amount">The amount of the item to give the player</param>
        public void InsertItem(string itemName, int amount = 1)
        {
            var foundItem = FindItem(itemName);
            if (foundItem == null)
            {
                // insert a new item into the _items list, as it does not exist already
                _items.Add(new Item(itemName, amount));
            }
            else
            {
                // Update the existing item
                foundItem.amount += amount;
            }
        }

        public void RemoveItem(string itemName, int amount = 1)
        {
            //todo: Do stuff here plox
            var foundItem = FindItem(itemName);
            if (foundItem != null)
            {
                _items.Remove(foundItem);
            }
        }

        /// <summary>
        /// Returns the item if found by item name, otherwise returns null
        /// </summary>
        /// <param name="itemName">The item name to find</param>
        /// <returns></returns>
        public Item FindItem(string itemName)
        {

            /*
            // The code below is doing this, but in a single line using Microsoft LINQ - Read up about it, it's extremely powerful.
            foreach ( var item in _items )
            {
                if ( item.name.Equals(itemName))
                {
                    return item;
                }
            }
<<<<<<< Updated upstream

=======
>>>>>>> Stashed changes
            return null;
            */

            return _items.DefaultIfEmpty(null).FirstOrDefault(x => x != null && x.name.Equals(itemName));
        }

<<<<<<< Updated upstream
    }
=======
        public int ListSize()
        {
            return _items.Count;
        }

        public Item ReturnItem(int i)
        {
            return _items[i];
        }

    }


>>>>>>> Stashed changes
}