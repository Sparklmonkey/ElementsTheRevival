using System.Collections.Generic;
using UnityEngine;

public class Chaos : ActivatedAbility
{
    public override bool NeedsTarget() => true;

    public override void Activate(ID targetId, Card targetCard)
    {
        if (!IsCardValid(targetId, targetCard)) return;
        ChaosSeed(DuelManager.Instance.GetIDOwner(targetId), targetId, targetCard);
    }

    private void ChaosSeed(PlayerManager targetOwner, ID targetId, Card targetCard)
    {
        var effect = Random.Range(0, 12);

        switch (effect)
        {
            case 0:
                targetCard.Poison++;
                break;
            case 1:
                targetCard.DefDamage += 5;
                break;
            case 2:
                var waterQ = DuelManager.Instance.GetIDOwner(BattleVars.Shared.AbilityIDOrigin).GetAllQuantaOfElement(Element.Water);
                var damageToDeal = 2 + Mathf.FloorToInt(waterQ / 10) * 2;
                var willFreeze = Random.Range(0, 100) > 30 + damageToDeal * 5;

                targetCard.DefDamage += damageToDeal;
                if (willFreeze)
                {
                    targetCard.Freeze = 3;
                }
                break;
            case 3:
                waterQ = DuelManager.Instance.GetIDOwner(BattleVars.Shared.AbilityIDOrigin).GetAllQuantaOfElement(Element.Fire);
                damageToDeal = 2 + Mathf.FloorToInt(waterQ / 10) * 2;

                targetCard.DefDamage += damageToDeal;
                break;
            case 4:
            case 5:
                EventBus<AddCardPlayedOnFieldActionEvent>.Raise(new AddCardPlayedOnFieldActionEvent(new(targetCard), targetId.owner.Equals(OwnerEnum.Player)));
                EventBus<PlayCreatureOnFieldEvent>.Raise(new PlayCreatureOnFieldEvent(targetId.owner, new(targetCard)));
                break;
            case 6:
                targetCard.skill = "";
                targetCard.desc = "";
                targetCard.passiveSkills = new();
                break;
            case 7:
                Card cardToPlay = new(targetCard);
                EventBus<AddCardPlayedOnFieldActionEvent>.Raise(new AddCardPlayedOnFieldActionEvent(cardToPlay, targetId.owner.Equals(OwnerEnum.Player)));
                EventBus<PlayCreatureOnFieldEvent>.Raise(new PlayCreatureOnFieldEvent(targetId.owner, cardToPlay));
                EventBus<PlayAnimationEvent>.Raise(new PlayAnimationEvent(targetId, "Steal", Element.Other));
                EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(targetId));
                return;
            case 8:
                targetCard.DefDamage += 3;
                break;
            case 9:
                if (targetCard.Freeze > 0)
                {
                    EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(targetId));
                    return;
                }
                targetCard.DefDamage += 4;
                break;
            case 10:
                if (targetCard.innateSkills.Mummy)
                {
                    var pharoah = CardDatabase.Instance.GetCardFromId(targetCard.iD.IsUpgraded() ? "7qc" : "5rs");
                    EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, pharoah, false));
                }
                else if (targetCard.innateSkills.Undead)
                {
                    EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, CardDatabase.Instance.GetRandomCard(CardType.Creature, targetCard.iD.IsUpgraded(), true), false));
                }
                else
                {
                    var baseCreature = CardDatabase.Instance.GetCardFromId(targetCard.iD);
                    targetOwner.AddCardToDeck(baseCreature);
                    EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(targetId));
                }
                break;
            case 11:
                targetCard.passiveSkills.GravityPull = true;
                break;
        }
        EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(targetId, targetCard, true));
    }

    public override bool IsCardValid(ID id, Card card)
    {
        if (card is null) return false;
        return card.cardType.Equals(CardType.Creature) && card.IsTargetable();
    }
}