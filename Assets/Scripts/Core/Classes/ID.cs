using System;
using UnityEngine.Serialization;

[Serializable]
public class PseudoID
{
    public int Owner { get; set; }
    public int Field { get; set; }
    public int Index { get; set; }
}


[Serializable]
public class ID
{
    [FormerlySerializedAs("Owner")] public OwnerEnum owner;
    [FormerlySerializedAs("Field")] public FieldEnum field;
    [FormerlySerializedAs("Index")] public int index;
    public ID(ID id)
    {
        owner = id.owner;
        field = id.field;
        index = id.index;
    }

    public ID(int owner, int field, int index)
    {
        this.owner = (OwnerEnum)owner;
        this.field = (FieldEnum)field;
        this.index = index;
    }

    public ID(OwnerEnum owner, FieldEnum field, int index)
    {
        this.owner = owner;
        this.field = field;
        this.index = index;
    }
}