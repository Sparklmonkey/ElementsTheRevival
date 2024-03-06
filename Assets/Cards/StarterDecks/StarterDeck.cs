using System.Collections;
using System.Collections.Generic;
using Battlefield.Abilities;
using UnityEngine;
using Elements.Duel.Manager;
using Sirenix.Serialization;
using Sirenix.OdinInspector;
[CreateAssetMenu(fileName = "StarterDeck", menuName = "ScriptableObjects/StarterDeck", order = 3)]
public class StarterDeck : SerializedScriptableObject
{
    [Title("Deck Mark", null, TitleAlignments.Centered), HideLabel]
    public Element MarkElement;

    [Title("Highlight Cards", null, TitleAlignments.Centered)]
    public Card HighlightOne;
    public Card HighlightTwo;

    [Title("Deck List", null, TitleAlignments.Centered), HideLabel]
    public List<Card> DeckList;
}