using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StarterDeck", menuName = "ScriptableObjects/StarterDeck", order = 3)]
public class StarterDeck  : ScriptableObject
{
    [Header("Basic Information")]
    public Element mark;
    public List<Card> deck;
}


