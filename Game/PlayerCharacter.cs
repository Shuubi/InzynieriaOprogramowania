using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
   public class PlayerCharacter
    {
        public List<Item> inventory;

        public PlayerCharacter()
        {
            List<Item> inventory = new List<Item>();
        }
    }
}
