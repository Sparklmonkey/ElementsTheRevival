using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]

public class GameStatRequest
{
    public string fgName;
    public int hbPrimary;
    public int hbSecondary;
    public bool isL1;
    public bool isL2;
    public bool isL3;
    public bool isArena;
    public bool didWin;
    public int gameStatId;

    public GameStatRequest(EnemyAi enemy, bool didWin, bool isArena)
    {
        this.didWin = didWin;
        this.isArena = isArena;
        isL1 = enemy.spins == 0;
        isL2 = enemy.spins == 1;
        isL3 = enemy.spins == 2;
        hbPrimary = enemy.spins == 3 && enemy.maxHP == 150 ? (int)BattleVars.shared.primaryElement : 14;
        hbSecondary = enemy.spins == 3 && enemy.maxHP == 150 ? (int)BattleVars.shared.secondaryElement : 14;
        fgName = enemy.spins == 3 && enemy.maxHP == 200 ? enemy.opponentName : "";
        gameStatId = PlayerData.shared.gameStatsId;
    }
}
