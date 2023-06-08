using System;

public class QuantaObject
{
    public int count;
    public Element element;

    public event Action<int> OnQuantaChange;

    public QuantaObject(Element element, int count)
    {
        this.count = count;
        this.element = element;
    }

    public void UpdateQuanta(int amount, bool isAdd)
    {
        count = Math.Min(Math.Max(count + (isAdd ? amount : -amount), 0), 75);
        OnQuantaChange?.Invoke(count);
    }
}