using System.Collections;
using UnityEngine;

public abstract class CardTypeBehaviour : MonoBehaviour
{
    public IDCardPair CardPair;
    public PlayerManager Owner { get; set; }
    public PlayerManager Enemy { get; set; }
    public abstract void OnTurnEnd();
    public abstract void OnTurnStart();
    public abstract void OnCardPlay();
    public abstract void OnCardRemove();
    public abstract void DeathTrigger();
}
