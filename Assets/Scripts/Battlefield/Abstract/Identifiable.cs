using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class Identifiable : MonoBehaviour
{
    private ID objectId;

    public ID GetObjectID() => objectId;
}
