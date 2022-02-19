using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battlefield_ObjectIDManager : MonoBehaviour
{
    public static Battlefield_ObjectIDManager shared;

    private Dictionary<ID, Transform> idTransformList;

    private void Awake()
    {
        if(shared != null)
        {
            return;
        }
        shared = new Battlefield_ObjectIDManager();
        shared.idTransformList = new Dictionary<ID, Transform>();
    }

    public void AddIdTransformPair(ID iD, Transform transform)
    {
        idTransformList.Add(iD, transform);
    }

    public Transform GetObjectFromID(ID objectID)
    {
        return idTransformList[objectID];
    }
}
