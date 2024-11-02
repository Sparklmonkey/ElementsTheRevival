using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "HasPillarInHand", story: "Check if [Ai] has more [CardType] in hand", category: "Conditions", id: "1f8ea3cc52b1009fc26d4f82b4daa58a")]
public partial class HasPillarInHandCondition : Condition
{

    public BlackboardVariable<PlayerManager> Ai;
    public BlackboardVariable<CardType> CardType;
    public override bool IsTrue()
    {
        return Ai.Value.playerHand.GetPlayableCardsOfType(Ai.Value.HasSufficientQuanta, CardType).Count == 0;
    }

    public override void OnStart()
    {
        Debug.Log($"Check if Ai has {CardType.Value.FastCardTypeString()} in Hand");
    }

    public override void OnEnd()
    {
    }
}
