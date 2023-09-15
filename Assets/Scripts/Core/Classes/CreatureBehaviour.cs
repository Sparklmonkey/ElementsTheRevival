using System.Collections.Generic;
using UnityEngine;

public class CreatureBehaviour : CardTypeBehaviour
{
    public override void OnCardPlay()
    {
        if (cardPair.card.innateSkills.Swarm)
        {
            Owner.AddPlayerCounter(PlayerCounters.Scarab, 1);
        }

        if (cardPair.card.innateSkills.Integrity)
        {
            var shardList = Owner.playerHand.GetAllValidCardIds().FindAll(x => x.card.cardName.Contains("Shard of"));
            cardPair.card = CardDatabase.Instance.GetGolemAbility(shardList);
        }

        if (cardPair.card.innateSkills.Chimera)
        {
            var creatureList = Owner.playerCreatureField.GetAllValidCardIds();
            (int, int) chimeraPwrHp = (0, 0);

            if (creatureList.Count > 0)
            {
                foreach (var creature in creatureList)
                {
                    if (creature.id != cardPair.id)
                    {
                        chimeraPwrHp.Item1 += creature.card.AtkNow;
                        chimeraPwrHp.Item2 += creature.card.DefNow;
                        creature.RemoveCard();
                    }
                }
            }

            cardPair.card.atk = chimeraPwrHp.Item1;
            cardPair.card.def = chimeraPwrHp.Item2;
            cardPair.UpdateCard();
        }
    }

    public override void OnCardRemove()
    {
        if (cardPair.card.innateSkills.Swarm)
        {
            Owner.AddPlayerCounter(PlayerCounters.Scarab, -1);
        }

        if (ShouldActivateDeathTriggers())
        {
            DuelManager.Instance.ActivateDeathTriggers();
        }


        if (cardPair.card.IsAflatoxin)
        {
            cardPair.PlayCard(CardDatabase.Instance.GetCardFromId("6ro"));
            return;
        }

        if (cardPair.card.passiveSkills.Phoenix && !ShouldActivateDeathTriggers())
        {
            cardPair.PlayCard(CardDatabase.Instance.GetCardFromId(cardPair.card.iD.IsUpgraded() ? "7dt" : "5fd"));
            return;
        }
        cardPair.card = null;
    }

    private bool ShouldActivateDeathTriggers()
    {
        if (BattleVars.Shared.AbilityOrigin != null)
        {
            return BattleVars.Shared.AbilityOrigin.card.skill != "reversetime";
        }
        return cardPair.card.cardName.Contains("Skeleton");
    }

    public override void OnTurnStart()
    {
        return;
    }

    public override void DeathTrigger()
    {
        if (cardPair.card.passiveSkills.Scavenger)
        {
            cardPair.card.AtkModify += 1;
            cardPair.card.DefModify += 1;
            cardPair.UpdateCard();
        }

    }

    public override void OnTurnEnd()
    {
        int adrenalineIndex = 0;
        bool hasAdrenaline = cardPair.card.passiveSkills.Adrenaline;
        bool isFirstAttack = true;
        int atkNow = cardPair.card.AtkNow;

        bool shouldSkip = DuelManager.Instance.GetCardCount(new() { "5rp", "7q9" }) > 0 || Owner.playerCounters.patience > 0 || cardPair.card.Freeze > 0 || cardPair.card.innateSkills.Delay > 0 || atkNow == 0;
        cardPair.card.AbilityUsed = false;

        while (isFirstAttack || hasAdrenaline)
        {
            isFirstAttack = false;
            if (!shouldSkip)
            {

                if (cardPair.card.innateSkills.Regenerate)
                {
                    Owner.ModifyHealthLogic(5, false, false);
                }
                if (cardPair.card.innateSkills.Fiery)
                {
                    atkNow += Mathf.FloorToInt(Owner.GetAllQuantaOfElement(Element.Fire) / 5);
                }
                if (cardPair.card.innateSkills.Hammer)
                {
                    if (Owner.playerPassiveManager.GetMark().card.costElement == Element.Earth || Owner.playerPassiveManager.GetMark().card.costElement == Element.Gravity)
                    {
                        atkNow++;
                    }
                }
                if (cardPair.card.innateSkills.Dagger)
                {
                    if (Owner.playerPassiveManager.GetMark().card.costElement == Element.Death || Owner.playerPassiveManager.GetMark().card.costElement == Element.Darkness)
                    {
                        atkNow++;
                    }
                }
                if (cardPair.card.innateSkills.Bow)
                {
                    if (Owner.playerPassiveManager.GetMark().card.costElement == Element.Air)
                    {
                        atkNow++;
                    }
                }
                bool isFreedomEffect = Random.Range(0, 100) < (25 * Owner.playerCounters.freedom) && cardPair.card.costElement.Equals(Element.Air);
                atkNow = Mathf.FloorToInt(isFreedomEffect ? atkNow * 1.5f : atkNow);
                SoundManager.Instance.PlayAudioClip("CreatureDamage");
                if (Enemy.HasGravityCreatures())
                {
                    Enemy.ManageGravityCreatures(ref atkNow, ref cardPair);
                    if (cardPair.card.DefNow <= 0)
                    {
                        cardPair.RemoveCard();
                        return;
                    }
                }

                if (!cardPair.card.passiveSkills.Momentum)
                {
                    if (Enemy.ManageShield(ref atkNow, ref cardPair))
                    {
                        if (cardPair.card.DefNow <= 0)
                        {
                            cardPair.RemoveCard();
                            return;
                        }
                    }
                }

                if (atkNow > 0 && adrenalineIndex < 2)
                {
                    if (cardPair.card.passiveSkills.Venom)
                    {
                        Enemy.AddPlayerCounter(PlayerCounters.Poison, 1);
                    }
                    if (cardPair.card.passiveSkills.DeadlyVenom)
                    {
                        Enemy.AddPlayerCounter(PlayerCounters.Poison, 2);
                    }
                    if (cardPair.card.passiveSkills.Neurotoxin)
                    {
                        Enemy.AddPlayerCounter(PlayerCounters.Neurotoxin, 1);
                    }
                }

                if (atkNow != 0 && adrenalineIndex < 2)
                {
                    if (cardPair.card.passiveSkills.Vampire)
                    {
                        Owner.ModifyHealthLogic(atkNow, false, false);
                    }
                    Enemy.ModifyHealthLogic(atkNow, true, cardPair.card.passiveSkills.Psion);
                }

                if (adrenalineIndex < 2)
                {
                    EndTurnPassiveEffect();
                }

                SingularityEffect();
                cardPair.UpdateCard();
                if (!cardPair.HasCard()) { return; }
                if (cardPair.card.AtkNow != 0 && hasAdrenaline)
                {
                    adrenalineIndex++;
                    if (DuelManager.AdrenalineDamageList[Mathf.Abs(cardPair.card.AtkNow) - 1].Count <= adrenalineIndex)
                    {
                        hasAdrenaline = false;
                    }
                    else
                    {
                        atkNow = DuelManager.AdrenalineDamageList[Mathf.Abs(cardPair.card.AtkNow) - 1][adrenalineIndex];
                        if (cardPair.card.passiveSkills.Antimatter)
                        {
                            atkNow = -atkNow;
                        }
                    }
                }
            }
            CreatureTurnDownTick();
            cardPair.UpdateCard();
        }
    }

    private void EndTurnPassiveEffect()
    {
        if (cardPair.card.innateSkills.Delay == 0)
        {
            if (cardPair.card.passiveSkills.Air)
            {
                AnimationManager.Instance.StartAnimation("QuantaGenerate", transform, Element.Air);
                Owner.GenerateQuantaLogic(Element.Air, 1);
            }
            if (cardPair.card.passiveSkills.Earth)
            {
                AnimationManager.Instance.StartAnimation("QuantaGenerate", transform, Element.Earth);
                Owner.GenerateQuantaLogic(Element.Earth, 1);
            }
            if (cardPair.card.passiveSkills.Fire)
            {
                AnimationManager.Instance.StartAnimation("QuantaGenerate", transform, Element.Fire);
                Owner.GenerateQuantaLogic(Element.Fire, 1);
            }
            if (cardPair.card.passiveSkills.Light)
            {
                AnimationManager.Instance.StartAnimation("QuantaGenerate", transform, Element.Light);
                Owner.GenerateQuantaLogic(Element.Light, 1);
            }
            if (cardPair.card.innateSkills.Devourer)
            {
                if (Enemy.GetAllQuantaOfElement(Element.Other) > 0 && Enemy.playerCounters.sanctuary == 0)
                {
                    Enemy.SpendQuantaLogic(Element.Other, 1);
                }
                AnimationManager.Instance.StartAnimation("QuantaGenerate", transform, Element.Darkness);
                Owner.GenerateQuantaLogic(Element.Darkness, 1);
            }
            if (cardPair.card.passiveSkills.Overdrive)
            {
                cardPair.card.AtkModify += 3;
                cardPair.card.DefModify -= 1;
            }
            if (cardPair.card.passiveSkills.Acceleration)
            {
                cardPair.card.AtkModify += 2;
                cardPair.card.DefModify -= 1;
            }
            if (cardPair.card.passiveSkills.Infest)
            {
                Owner.PlayCardOnFieldLogic(CardDatabase.Instance.GetCardFromId("4t8"));
            }

        }

        if (cardPair.card.innateSkills.Swarm)
        {
            cardPair.card.def = Owner.playerCounters.scarab;
            cardPair.UpdateCard();
        }
    }

    private void CreatureTurnDownTick()
    {
        if (cardPair.card.passiveSkills.Dive)
        {
            cardPair.card.passiveSkills.Dive = false;
            cardPair.card.atk /= 2;
            cardPair.card.AtkModify /= 2;
        }

        if (cardPair.card.Freeze > 0)
        {
            cardPair.card.Freeze--;
        }

        if (cardPair.card.Charge > 0)
        {
            cardPair.card.Charge--;
            cardPair.card.AtkModify--;
            cardPair.card.DefModify--;

        }
        if (cardPair.card.innateSkills.Delay > 0)
        {
            cardPair.card.innateSkills.Delay--;
        }

        int healthChange = cardPair.card.Poison;
        cardPair.card.DefDamage += healthChange;
    }

    private void SingularityEffect()
    {
        if (!cardPair.card.innateSkills.Singularity) { return; }
        List<string> singuEffects = new() { "Chaos", "Copy", "Nova" };
        if (cardPair.card.AtkNow > 0)
        {
            cardPair.card.atk = -(Mathf.Abs(cardPair.card.atk));
            cardPair.card.AtkModify = -(Mathf.Abs(cardPair.card.AtkModify));
        }

        if (!cardPair.card.innateSkills.Immaterial)
        {
            singuEffects.Add("Immaterial");
        }
        if (!cardPair.card.passiveSkills.Adrenaline)
        {
            singuEffects.Add("Adrenaline");
        }
        if (!cardPair.card.passiveSkills.Vampire)
        {
            singuEffects.Add("Vampire");
        }

        switch (singuEffects[Random.Range(0, singuEffects.Count)])
        {
            case "Immaterial":
                cardPair.card.innateSkills.Immaterial = true;
                break;
            case "Vampire":
                cardPair.card.passiveSkills.Vampire = true;
                break;
            case "Chaos":
                int chaos = Random.Range(1, 6);
                cardPair.card.AtkModify += chaos;
                cardPair.card.DefModify += chaos;
                break;
            case "Nova":
                for (int y = 0; y < 12; y++)
                {
                    DuelManager.GetNotIDOwner(cardPair.id).GenerateQuantaLogic((Element)y, 1);
                }
                break;
            case "Addrenaline":
                cardPair.card.passiveSkills.Adrenaline = true;
                break;
            default:
                Card duplicate = new(cardPair.card);
                AnimationManager.Instance.StartAnimation("ParallelUniverse", transform);
                DuelManager.GetIDOwner(cardPair.id).PlayCardOnFieldLogic(duplicate);
                break;
        }
    }
}
