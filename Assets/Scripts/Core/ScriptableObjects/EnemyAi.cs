using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "EnemyAi", menuName = "ScriptableObjects/EnemyAi", order = 2)]
public class EnemyAi : ScriptableObject
{
    [Header("Basic Information")]
    public string opponentName;
    [FormerlySerializedAs("maxHP")] public int maxHp;
    public Element mark;
    public string deck;
    public int spins;
    public int coinAvg;
    public int costToPlay;

    public int scoreWin;
    public int hpDivide;


    //Ai Componenents
    public string drawComponent;
    public string discardComponent;
    public string turnComponent;

}


