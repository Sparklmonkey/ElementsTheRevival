using System.Collections.Generic;
using UnityEngine;

public class Chaos : AbilityEffect
{
    public override bool NeedsTarget() => true;
    public override TargetPriority GetPriority() => TargetPriority.Any;

    public override void Activate(IDCardPair target)
    {
        ChaosSeed(DuelManager.Instance.GetIDOwner(target.id), target);
    }

    private void ChaosSeed(PlayerManager targetOwner, IDCardPair iDCard)
    {
        var effect = Random.Range(0, 12);

        switch (effect)
        {
            case 0:
                iDCard.card.Poison++;
                break;
            case 1:
                iDCard.card.DefDamage += 5;
                break;
            case 2:
                var waterQ = Owner.GetAllQuantaOfElement(Element.Water);
                var damageToDeal = 2 + Mathf.FloorToInt(waterQ / 10) * 2;
                var willFreeze = Random.Range(0, 100) > 30 + damageToDeal * 5;

                iDCard.card.DefDamage += damageToDeal;
                if (willFreeze)
                {
                    iDCard.card.Freeze = 3;
                }
                break;
            case 3:
                waterQ = Owner.GetAllQuantaOfElement(Element.Fire);
                damageToDeal = 2 + Mathf.FloorToInt(waterQ / 10) * 2;

                iDCard.card.DefDamage += damageToDeal;
                break;
            case 4:
            case 5:
                Owner.PlayCardOnField(new(iDCard.card));
                break;
            case 6:
                iDCard.card.skill = "";
                iDCard.card.desc = "";
                iDCard.card.passiveSkills = new();
                break;
            case 7:
                Card cardToPlay = new(iDCard.card);
                Owner.PlayCardOnField(cardToPlay);
                AnimationManager.Instance.StartAnimation("Steal", iDCard.transform);
                EventBus<OnCardRemovedEvent>.Raise(new OnCardRemovedEvent(iDCard.id));
                return;
            case 8:
                iDCard.card.DefDamage += 3;
                break;
            case 9:
                if (iDCard.card.Freeze > 0)
                {
                    EventBus<OnCardRemovedEvent>.Raise(new OnCardRemovedEvent(iDCard.id));
                    return;
                }
                iDCard.card.DefDamage += 4;
                break;
            case 10:
                if (iDCard.card.innateSkills.Mummy)
                {
                    var pharoah = CardDatabase.Instance.GetCardFromId(iDCard.card.iD.IsUpgraded() ? "7qc" : "5rs");
                    EventBus<OnCardPlayEvent>.Raise(new OnCardPlayEvent(iDCard.id, pharoah));
                }
                else if (iDCard.card.innateSkills.Undead)
                {
                    EventBus<OnCardPlayEvent>.Raise(new OnCardPlayEvent(iDCard.id, CardDatabase.Instance.GetRandomCard(CardType.Creature, iDCard.card.iD.IsUpgraded(), true)));
                }
                else
                {
                    var baseCreature = CardDatabase.Instance.GetCardFromId(iDCard.card.iD);
                    targetOwner.AddCardToDeck(baseCreature);
                    EventBus<OnCardRemovedEvent>.Raise(new OnCardRemovedEvent(iDCard.id));
                }
                break;
            case 11:
                iDCard.card.passiveSkills.GravityPull = true;
                break;
        }
        iDCard.UpdateCard();
    }

    public override List<IDCardPair> GetPossibleTargets(PlayerManager enemy)
    {
        var possibleTargets = enemy.playerCreatureField.GetAllValidCardIds();
        possibleTargets.AddRange(Owner.playerCreatureField.GetAllValidCardIds());
        if (possibleTargets.Count == 0) { return new(); }
        return possibleTargets.FindAll(x => x.IsTargetable());
    }

    public override IDCardPair SelectRandomTarget(List<IDCardPair> possibleTargets)
    {
        if (possibleTargets.Count == 0) { return null; }

        var opCreatures = possibleTargets.FindAll(x => x.id.owner == OwnerEnum.Player && x.HasCard());

        if (opCreatures.Count == 0)
        {
            return null;
        }
        else
        {
            return opCreatures[Random.Range(0, possibleTargets.Count)];
        }
    }
}