using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Identifiable : MonoBehaviour
{
    private ID objectId;

    public ID GetObjectID() => objectId;

    public void SetID(OwnerEnum owner, FieldEnum field, int index, Transform transform)
    {
        objectId = new ID(owner, field, index);
        Battlefield_ObjectIDManager.shared.AddIdTransformPair(objectId, transform);
    }
}
