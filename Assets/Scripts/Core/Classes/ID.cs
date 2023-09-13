using System;

[Serializable]
public class PseudoID
{
    public int owner { get; set; }
    public int field { get; set; }
    public int index { get; set; }
}


[Serializable]
public class ID
{
    public OwnerEnum Owner;
    public FieldEnum Field;
    public int Index;
    public ID(ID id)
    {
        Owner = id.Owner;
        Field = id.Field;
        Index = id.Index;
    }

    public ID(int owner, int field, int index)
    {
        Owner = (OwnerEnum)owner;
        Field = (FieldEnum)field;
        Index = index;
    }

    public ID(OwnerEnum owner, FieldEnum field, int index)
    {
        Owner = owner;
        Field = field;
        Index = index;
    }
}