using System.Collections.Generic;

public class Gravitypullc : ActivatedAbility
{
    public override bool NeedsTarget() => false;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        var owner = DuelManager.Instance.GetIDOwner(targetId);
        var gCreature = owner.playerCreatureField.GetCreatureWithGravity();
        if (!gCreature.Equals(default))
        {
            gCreature.card.passiveSkills.GravityPull = false;
            EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(gCreature.id, gCreature.card, true));
        }
        targetCard.passiveSkills.GravityPull = true;
        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, targetCard, true));
    }
}
