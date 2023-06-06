using System;

[Serializable]
public class IDCardPair
{
    public ID id;
    public Card card;

    public IDCardPair(ID id, Card card)
    {
        this.id = id;
        this.card = card;
    }

}
