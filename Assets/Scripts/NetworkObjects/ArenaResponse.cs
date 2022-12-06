using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ArenaResponse
{
    public List<string> opponentDeck;
    public int deckMark;
    public string username;
    public int playerScore;
    public int arenaWins;
    public int arenaLoses;
    public int arenaRank;
}
