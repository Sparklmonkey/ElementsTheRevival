using System.Collections;
using System.Collections.Generic;
using Battlefield.Abilities;
using UnityEngine;
using Elements.Duel.Manager;
using Sirenix.Serialization;
using Sirenix.OdinInspector;
[CreateAssetMenu(fileName = "TrainerAccount", menuName = "ScriptableObjects/TrainerAccount", order = 4)]
public class TrainerAccount : SerializedScriptableObject
{
    public List<Card> InventoryCards;
}
