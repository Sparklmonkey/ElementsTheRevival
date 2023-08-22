using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battlefield_ObjectIDManager : MonoBehaviour 
{
    public static Battlefield_ObjectIDManager shared;

    private List<ID> idList;
    private List<Transform> transformList; 

    private void Awake()
    {
        if(shared != null)
        {
            return;
        }
        shared = this;
        idList = new List<ID>();
        transformList = new List<Transform>();
    }

    public void AddIdTransformPair(ID iD, Transform transform)
    {
        idList.Add(iD);
        transformList.Add(transform);
    }

    //public Transform GetObjectFromID(ID objectID)
    //{
    //    return transformList[GetIndexOfID(objectID)];
    //}

    private int GetIndexOfID(ID idToFind)
    {
        for (int i = 0; i < idList.Count; i++)
        {
            ID id = idList[i];
            if(id.Index == idToFind.Index && id.Field == idToFind.Field && id.Owner == idToFind.Owner) { return i; }
        }
        return -1;
    }
}
