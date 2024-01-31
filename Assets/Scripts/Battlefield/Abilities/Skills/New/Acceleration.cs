public class AccelerationNew : ActivatedAbility
{
    public override bool NeedsTarget() => true;
    
    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        targetCard.desc = "Acceleration: \n Gain +2 /-1 per turn";
        targetCard.skill = "";
        targetCard.passiveSkills.Acceleration = true;
        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, targetCard, true));
    }

    public override AiTargetType GetAiTargetType(string skill)
    {
        return new AiTargetType(false, false, false, TargetType.DefineDef, 1, 3, 0);
    }

    public override bool IsCardValid(ID id, Card card)
    {
        if (card is null) return false;
        return card.cardType.Equals(CardType.Creature) && card.IsTargetable();
    }
    
}