using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DummyClient
{
    class Room
    {
        public Dictionary<int, S_PLAYERLIST.Player> players;

        public Room()
        {
            players = new Dictionary<int, S_PLAYERLIST.Player>();
        }
    }
}
