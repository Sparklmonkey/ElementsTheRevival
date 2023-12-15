using UnityEngine;

public struct QuantaPool
{
    private int Aether { get; set; }
    private int Darkness { get; set; }
    private int Death { get; set; }
    private int Light { get; set; }
    private int Life { get; set; }
    private int Gravity { get; set; }
    private int Entropy { get; set; }
    private int Water { get; set; }
    private int Fire { get; set; }
    private int Air { get; set; }
    private int Earth { get; set; }
    private int Time { get; set; }

    public int AddQuanta(Element element, int amount)
    {
        var quantaAmount = GetQuantaAmount(element);
        quantaAmount += amount;
        if (quantaAmount < 0) { quantaAmount = 0; }
        if (quantaAmount > 75) { quantaAmount = 75; }
        SetQuantaAmount(element, quantaAmount);

        return quantaAmount;
    }
    public int GetQuantaAmount(Element element)
    {
        return element switch
        {
            Element.Aether => Aether,
            Element.Air => Air,
            Element.Darkness => Darkness,
            Element.Light => Light,
            Element.Death => Death,
            Element.Earth => Earth,
            Element.Entropy => Entropy,
            Element.Time => Time,
            Element.Fire => Fire,
            Element.Gravity => Gravity,
            Element.Life => Life,
            Element.Water => Water,
            Element.Other => GetFullQuantaCount(),
            _ => 0
        };
    }

    private void SetQuantaAmount(Element element, int amount)
    {
        switch (element)
        {
            case Element.Aether:
                Aether = amount;
                break;
            case Element.Air:
                Air = amount;
                break;
            case Element.Darkness:
                Darkness = amount;
                break;
            case Element.Light:
                Light = amount;
                break;
            case Element.Death:
                Death = amount;
                break;
            case Element.Earth:
                Earth = amount;
                break;
            case Element.Entropy:
                Entropy = amount;
                break;
            case Element.Time:
                Time = amount;
                break;
            case Element.Fire:
                Fire = amount;
                break;
            case Element.Gravity:
                Gravity = amount;
                break;
            case Element.Life:
                Life = amount;
                break;
            case Element.Water:
                Water = amount;
                break;
            case Element.Other:
                break;
            default:
                break;
        }
    }
    public int GetFullQuantaCount()
    {
        var quantaAmount = 0;

        for (var i = 0; i < 12; i++)
        {
            quantaAmount += GetQuantaAmount((Element)i);
        }

        return quantaAmount;
    }
}