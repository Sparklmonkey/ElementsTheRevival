using System;

[Serializable]
public class PseudoID
{
    public int Owner { get; set; }
    public int Field { get; set; }
    public int Index { get; set; }
}


[Serializable]
public class ID : IEquatable<ID>
{
    public OwnerEnum owner;
    public FieldEnum field;
    public int index;
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

    public bool Equals(ID other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return owner == other.owner && field == other.field && index == other.index;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((ID)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine((int)owner, (int)field, index);
    }
}