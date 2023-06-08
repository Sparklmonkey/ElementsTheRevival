using System;

[Serializable]
public class IDCardPair
{
    public ID id;
    public Card card;
    public event Action<Card> OnCardChanged;

    public IDCardPair(ID id, Card card)
    {
        this.id = id;
        this.card = card;
    }

    public void PlayCard(Card card)
    {
        this.card = card;
        OnCardChanged?.Invoke(card);
    }

    public void RemoveCard()
    {
        card = null;
        OnCardChanged?.Invoke(null);
    }

    public Card GetCard() => card;
}
