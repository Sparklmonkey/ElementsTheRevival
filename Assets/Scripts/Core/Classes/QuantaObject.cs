using System;

public class QuantaObject
{
    public int Count;
    public Element Element;

    public event Action<int> OnQuantaChange;

    public QuantaObject(Element element, int count)
    {
        this.Count = count;
        this.Element = element;
    }

    public void UpdateQuanta(int amount, bool isAdd)
    {
        Count = Math.Min(Math.Max(Count + (isAdd ? amount : -amount), 0), 75);
        OnQuantaChange?.Invoke(Count);
    }
}