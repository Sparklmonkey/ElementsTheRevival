using System;
using System.Collections.Generic;

namespace Networking
{
    [Serializable]
    public class ArenaResponse
    {
        public string arenaT50Deck;
        public int arenaT50Mark;
        public string username;
        public int playerScore;
        public int arenaWins;
        public int arenaLoses;
        public int arenaRank;
    }
}
