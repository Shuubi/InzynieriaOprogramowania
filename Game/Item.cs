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
        string name { set; get; }

        public Item(string n)
        {
            name = n;
            amount = 1;
        }
    }
}
