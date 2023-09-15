using System;
[Serializable]

public class GameStatRequest
{
    public int aiLevel;
    public string aiName;
    public bool isWin;

    public GameStatRequest(EnemyAi enemy, bool didWin, bool isArena)
    {
        isWin = didWin;
        aiName = enemy.opponentName;

        if (isArena)
        {
            aiLevel = 6;
        }
        else
        {
            switch (enemy.spins)
            {
                case 0:
                    aiLevel = 0;
                    break;
                case 1:
                    aiLevel = 1;
                    break;
                case 2:
                    aiLevel = 2;
                    break;
                case 3 when enemy.maxHp == 100:
                    aiLevel = 3;
                    break;
                case 3 when enemy.maxHp == 150:
                    aiLevel = 4;
                    break;
                case 3 when enemy.maxHp == 200:
                    aiLevel = 5;
                    break;
                default:
                    break;
            }
        }        
    }
}
