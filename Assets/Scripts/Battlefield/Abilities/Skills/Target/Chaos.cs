using System.Collections.Generic;
using UnityEngine;

public class Chaos : AbilityEffect
{
    public override bool NeedsTarget() => true;

    public override void Activate(IDCardPair target)
    {
        ChaosSeed(DuelManager.GetIDOwner(target.id), target);
    }

    private void ChaosSeed(PlayerManager targetOwner, IDCardPair iDCard)
    {
        int effect = Random.Range(0, 12);

        switch (effect)
        {
            case 0:
                iDCard.card.Poison++;
                break;
            case 1:
                iDCard.card.DefDamage += 5;
                break;
            case 2:
                int waterQ = Owner.GetAllQuantaOfElement(Element.Water);
                int damageToDeal = 2 + (Mathf.FloorToInt(waterQ / 10) * 2);
                bool willFreeze = Random.Range(0, 100) > 30 + (damageToDeal * 5);

                iDCard.card.DefDamage += damageToDeal;
                if (willFreeze)
                {
                    iDCard.card.Freeze = 3;
                }
                break;
            case 3:
                waterQ = Owner.GetAllQuantaOfElement(Element.Fire);
                damageToDeal = 2 + (Mathf.FloorToInt(waterQ / 10) * 2);

                iDCard.card.DefDamage += damageToDeal;
                break;
            case 4:
            case 5:
                Owner.PlayCardOnFieldLogic(new(iDCard.card));
                break;
            case 6:
                iDCard.card.skill = "";
                iDCard.card.desc = "";
                iDCard.card.passiveSkills = new();
                break;
            case 7:
                Card cardToPlay = new(iDCard.card);
                Owner.PlayCardOnFieldLogic(cardToPlay);
                Game_AnimationManager.shared.StartAnimation("Steal", iDCard.transform);
                iDCard.RemoveCard();
                return;
            case 8:
                iDCard.card.DefDamage += 3;
                break;
            case 9:
                if (iDCard.card.Freeze > 0)
                {
                    iDCard.RemoveCard();
                    return;
                }
                else
                {
                    iDCard.card.DefDamage += 4;
                }
                break;
            case 10:
                if (iDCard.card.innateSkills.Mummy)
                {
                    Card pharoah = CardDatabase.Instance.GetCardFromId(iDCard.card.iD.IsUpgraded() ? "7qc" : "5rs");
                    iDCard.PlayCard(pharoah);
                }
                else if (iDCard.card.innateSkills.Undead)
                {
                    Card rndCreature = iDCard.card.iD.IsUpgraded() ? CardDatabase.Instance.GetRandomEliteCreature() : CardDatabase.Instance.GetRandomCreature();
                    iDCard.PlayCard(rndCreature);
                }
                else
                {
                    Card baseCreature = CardDatabase.Instance.GetCardFromId(iDCard.card.iD);
                    targetOwner.AddCardToDeck(baseCreature);
                    iDCard.RemoveCard();
                }
                break;
            case 11:
                iDCard.card.passiveSkills.GravityPull = true;
                break;
            default:
                break;
        }
        iDCard.UpdateCard();
    }

    public override List<IDCardPair> GetPossibleTargets(PlayerManager enemy)
    {
        var possibleTargets = enemy.playerCreatureField.GetAllValidCardIds();
        possibleTargets.AddRange(Owner.playerCreatureField.GetAllValidCardIds());
        if (possibleTargets.Count == 0) { return new(); }
        return possibleTargets.FindAll(x => x.IsTargetable() && x.card.innateSkills.Airborne);
    }

    public override IDCardPair SelectRandomTarget(List<IDCardPair> posibleTargets)
    {
        if (posibleTargets.Count == 0) { return null; }

        var opCreatures = posibleTargets.FindAll(x => x.id.Owner == OwnerEnum.Player && x.HasCard());

        if (opCreatures.Count == 0)
        {
            return null;
        }
        else
        {
            return opCreatures[Random.Range(0, posibleTargets.Count)];
        }
    }
}