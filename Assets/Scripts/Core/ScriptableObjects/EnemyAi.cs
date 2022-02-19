using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAi", menuName = "ScriptableObjects/EnemyAi", order = 2)]
public class EnemyAi : ScriptableObject
{
    [Header("Basic Information")]
    public string opponentName;
    public int maxHP;
    public Element mark;
    public List<Card> deck;
    public int spins;
    public int coinAvg;
    public int costToPlay;

    public int scoreWin;


    //Ai Componenents
    public string drawComponent;
    public string discardComponent;
    public string turnComponent;
}


