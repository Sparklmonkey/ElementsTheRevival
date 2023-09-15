using UnityEngine;
using UnityEngine.Serialization;

public abstract class CardTypeBehaviour : MonoBehaviour
{
    [FormerlySerializedAs("CardPair")] public IDCardPair cardPair;
    public PlayerManager Owner { get; set; }
    public PlayerManager Enemy { get; set; }
    public int StackCount { get; set; }
    public abstract void OnTurnEnd();
    public abstract void OnTurnStart();
    public abstract void OnCardPlay();
    public abstract void OnCardRemove();
    public abstract void DeathTrigger();
}
