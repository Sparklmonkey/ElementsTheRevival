
using System;

[Serializable]
public class CardObject
{
    public string cardName;
    public string cardType;
    public bool isUpgraded;

    public CardObject()
    {
        cardName = "";
        cardType = "";
        isUpgraded = false;
    }

    public CardObject(string cardName, string cardType, bool isUpgraded)
    {
        this.cardName = cardName;
        this.cardType = cardType;
        this.isUpgraded = isUpgraded;
    }
}
