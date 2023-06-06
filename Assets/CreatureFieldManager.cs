using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureFieldManager : MonoBehaviour
{
    private List<CardInPlay> _creaturePositions;
    private void Awake()
    {
        _creaturePositions = new();
        List<CardInPlay> cardInPlayList = new(GetComponentsInChildren<CardInPlay>());

        for (int i = 0; i < cardInPlayList.Count; i++)
        {
            _creaturePositions.Add(cardInPlayList.Find(x => x.gameObject.transform.parent.name.Equals($"Creature_{i + 1}")));
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
