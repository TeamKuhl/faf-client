using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KuhlEngine;

namespace FAFClient
{
    class Friend
    {
        public int Direction { get; set; }
        public int Speed { get; set; }
        public Item Item { get; set; }
        public int MoveLock { get; set; }

        public Friend(Item aItem)
        {
            Item = aItem;
            Direction = 0;
            Speed = 0;
        }
    }
}
