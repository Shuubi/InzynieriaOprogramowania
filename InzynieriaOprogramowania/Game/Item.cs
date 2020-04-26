using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
   public class Item
    {
        public int amount { set; get; }
        public string name { set; get; }

        public Item(string name, int amount = 1)
        {
            this.name = name;
            this.amount = amount;
        }
    }
}
