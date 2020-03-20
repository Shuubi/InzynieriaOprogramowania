using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
   public class PlayerCharacter
    {
        public PlayerItemInventory Items = new PlayerItemInventory();
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
            if ( foundItem == null )
            {
                // insert a new item into the _items list, as it does not exist already
                _items.Add(new Item(itemName, amount));
            } else
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

            return null;
            */

            return _items.DefaultIfEmpty(null).FirstOrDefault(x =>  x != null && x.name.Equals(itemName));
        }

    }
}
