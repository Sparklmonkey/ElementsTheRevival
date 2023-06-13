using System;
using UnityEngine;
[Serializable]
public class IDCardPair : MonoBehaviour
{
    public ID id;
    public Card card;
    public int stackCount;
    public event Action<Card, int> OnCardChanged;
    public event Action<Card, int> OnCardRemoved;

    public IDCardPair(ID id, Card card)
    {
        this.id = id;
        this.card = card;
    }

    public void PlayCard(Card card)
    {
        this.card = card;
        if(card.cardType == CardType.Pillar)
        {
            stackCount++;
        }
        else
        {
            stackCount = 1;
        }
        OnCardChanged?.Invoke(card, stackCount);
    }

    public void UpdateCard()
    {
        if (card.cardType.Equals(CardType.Creature))
        {
            if (card.DefDamage < 0) { card.DefDamage = 0; }
            if (card.DefNow <= 0)
            {
                RemoveCard();
                return;
            }
        }
        OnCardChanged?.Invoke(card, stackCount);
    }

    public void RemoveCard()
    {
        stackCount--;
        OnCardRemoved?.Invoke(card, stackCount);
        if (stackCount == 0)
        {
            card = null;
        }
    }

    public Card GetCard() => card;

    internal bool HasCard()
    {
        return card != null && card.iD != "4t2" && card.iD != "4t1";
    }
}
