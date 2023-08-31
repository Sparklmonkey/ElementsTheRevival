using System;
using System.Collections.Generic;

public class PvpUserInfo
{
    public int Score { get; set; }
    public int Win { get; set; }
    public int Lost { get; set; }
    public string Username { get; set; }
    public Element ElementMark { get; set; }
    public PvpUserInfo(string username, int wins, int loses, int score, int opponentMark)
    {
        Username = username;
        Score = score;
        Win = wins;
        Lost = loses;
        ElementMark = (Element)opponentMark;
    }

}
