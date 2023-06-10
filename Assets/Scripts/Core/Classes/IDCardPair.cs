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
        return card != null;
    }
}
