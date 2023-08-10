using System.Collections.Generic;
using UnityEngine;

public class CreatureBehaviour : CardTypeBehaviour
{
    public override void OnCardPlay()
    {
        if (CardPair.card.innateSkills.Swarm)
        {
            Owner.AddPlayerCounter(PlayerCounters.Scarab, 1);
        }

        if (CardPair.card.innateSkills.Integrity)
        {
            var shardList = Owner.playerHand.GetAllValidCardIds().FindAll(x => x.card.cardName.Contains("Shard of"));
            CardPair.card = CardDatabase.Instance.GetGolemAbility(shardList);
        }

        if (CardPair.card.innateSkills.Chimera)
        {
            var creatureList = Owner.playerCreatureField.GetAllValidCardIds();
            (int, int) chimeraPwrHP = (0, 0);

            if (creatureList.Count > 0)
            {
                foreach (var creature in creatureList)
                {
                    if (creature.id != CardPair.id)
                    {
                        chimeraPwrHP.Item1 += creature.card.AtkNow;
                        chimeraPwrHP.Item2 += creature.card.DefNow;
                        creature.RemoveCard();
                    }
                }
            }

            CardPair.card.atk = chimeraPwrHP.Item1;
            CardPair.card.def = chimeraPwrHP.Item2;
            CardPair.UpdateCard();
        }
    }

    public override void OnCardRemove()
    {
        if (CardPair.card.innateSkills.Swarm)
        {
            Owner.AddPlayerCounter(PlayerCounters.Scarab, -1);
        }
        if (!CardPair.card.cardName.Contains("Skeleton"))
        {
            DuelManager.Instance.ActivateDeathTriggers();
        }

        if (CardPair.card.IsAflatoxin)
        {
            CardPair.PlayCard(CardDatabase.Instance.GetCardFromId("6ro"));
            return;
        }

        if (CardPair.card.passiveSkills.Phoenix)
        {
            if (BattleVars.shared.abilityOrigin != null)
            {
                if (BattleVars.shared.abilityOrigin.card.skill != "reverse time")
                {
                    CardPair.PlayCard(CardDatabase.Instance.GetCardFromId(CardPair.card.iD.IsUpgraded() ? "7dt" : "5fd"));
                }
            }
            else
            {
                CardPair.PlayCard(CardDatabase.Instance.GetCardFromId(CardPair.card.iD.IsUpgraded() ? "7dt" : "5fd"));
            }
        }
    }

    public override void OnTurnStart()
    {
        return;
    }

    public override void DeathTrigger()
    {
        if (CardPair.card.passiveSkills.Scavenger)
        {
            CardPair.card.AtkModify += 1;
            CardPair.card.DefModify += 1;
            CardPair.UpdateCard();
        }

    }

    public override void OnTurnEnd()
    {
        int adrenalineIndex = 0;
        bool hasAdrenaline = CardPair.card.passiveSkills.Adrenaline;
        bool isFirstAttack = true;
        int atkNow = CardPair.card.AtkNow;

        bool shouldSkip = DuelManager.IsSundialInPlay() || Owner.playerCounters.patience > 0 || CardPair.card.Freeze > 0 || CardPair.card.innateSkills.Delay > 0 || atkNow == 0;
        CardPair.card.AbilityUsed = false;

        while (isFirstAttack || hasAdrenaline)
        {
            isFirstAttack = false;
            if (!shouldSkip)
            {
                bool isFreedomEffect = Random.Range(0, 100) < (25 * Owner.playerCounters.freedom) && CardPair.card.costElement.Equals(Element.Air);
                atkNow = Mathf.FloorToInt(isFreedomEffect ? atkNow * 1.5f : atkNow);
                Game_SoundManager.shared.PlayAudioClip("CreatureDamage");
                if (Enemy.HasGravityCreatures())
                {
                    Enemy.ManageGravityCreatures(ref atkNow, ref CardPair);
                    if (CardPair.card.DefNow <= 0)
                    {
                        CardPair.RemoveCard();
                        return;
                    }
                }

                if (!CardPair.card.passiveSkills.Momentum)
                {
                    if(Enemy.ManageShield(ref atkNow, ref CardPair))
                    {
                        if (CardPair.card.DefNow <= 0)
                        {
                            CardPair.RemoveCard();
                            return;
                        }
                    }
                }

                if (atkNow > 0)
                {
                    if (CardPair.card.passiveSkills.Venom)
                    {
                        Enemy.AddPlayerCounter(PlayerCounters.Poison, 1);
                    }
                    if (CardPair.card.passiveSkills.DeadlyVenom)
                    {
                        Enemy.AddPlayerCounter(PlayerCounters.Poison, 2);
                    }
                    if (CardPair.card.passiveSkills.Neurotoxin)
                    {
                        Enemy.AddPlayerCounter(PlayerCounters.Neurotoxin, 1);
                    }
                }
                if (atkNow != 0)
                {
                    if (CardPair.card.passiveSkills.Vampire)
                    {
                        Owner.ModifyHealthLogic(atkNow, false, false);
                    }
                    Enemy.ModifyHealthLogic(atkNow, true, CardPair.card.passiveSkills.Psion);
                }

                EndTurnPassiveEffect();
                SingularityEffect();
                CreatureTurnDownTick();
                CardPair.UpdateCard();
                if (!CardPair.HasCard()) { return; }
                if (CardPair.card.AtkNow != 0 && hasAdrenaline)
                {
                    adrenalineIndex++;
                    if (DuelManager.AdrenalineDamageList[Mathf.Abs(CardPair.card.AtkNow) - 1].Count >= adrenalineIndex)
                    {
                        hasAdrenaline = false;
                    }
                    else
                    {
                        atkNow = DuelManager.AdrenalineDamageList[Mathf.Abs(CardPair.card.AtkNow) - 1][adrenalineIndex];
                        if (CardPair.card.passiveSkills.Antimatter)
                        {
                            atkNow = -atkNow;
                        }
                    }
                }
            }
        }
    }

    private void EndTurnPassiveEffect()
    {
        if (CardPair.card.innateSkills.Delay == 0)
        {
            if (CardPair.card.passiveSkills.Air)
            {
                Game_AnimationManager.shared.StartAnimation("QuantaGenerate", transform, Element.Air);
                Owner.GenerateQuantaLogic(Element.Air, 1);
            }
            if (CardPair.card.passiveSkills.Earth)
            {
                Game_AnimationManager.shared.StartAnimation("QuantaGenerate", transform, Element.Earth);
                Owner.GenerateQuantaLogic(Element.Earth, 1);
            }
            if (CardPair.card.passiveSkills.Fire)
            {
                Game_AnimationManager.shared.StartAnimation("QuantaGenerate", transform, Element.Fire);
                Owner.GenerateQuantaLogic(Element.Fire, 1);
            }
            if (CardPair.card.passiveSkills.Light)
            {
                Game_AnimationManager.shared.StartAnimation("QuantaGenerate", transform, Element.Light);
                Owner.GenerateQuantaLogic(Element.Light, 1);
            }
            if (CardPair.card.innateSkills.Devourer)
            {
                if (Enemy.GetAllQuantaOfElement(Element.Other) > 0 && Enemy.playerCounters.sanctuary == 0)
                {
                    Enemy.SpendQuantaLogic(Element.Other, 1);
                    Game_AnimationManager.shared.StartAnimation("QuantaGenerate", transform, Element.Darkness);
                    Owner.GenerateQuantaLogic(Element.Darkness, 1);
                }
            }
            if (CardPair.card.passiveSkills.Overdrive)
            {
                CardPair.card.AtkModify += 3;
                CardPair.card.DefModify -= 1;
            }
            if (CardPair.card.passiveSkills.Acceleration)
            {
                CardPair.card.AtkModify += 2;
                CardPair.card.DefModify -= 1;
            }
            if (CardPair.card.passiveSkills.Infest)
            {
                Owner.PlayCardOnFieldLogic(CardDatabase.Instance.GetCardFromId("4t8"));
            }
        }

        if (CardPair.card.innateSkills.Swarm)
        {
            CardPair.card.def = Owner.playerCounters.scarab;
            CardPair.UpdateCard();
        }
    }

    private void CreatureTurnDownTick()
    {
        if (CardPair.card.passiveSkills.Dive)
        {
            CardPair.card.passiveSkills.Dive = false;
            CardPair.card.atk /= 2;
            CardPair.card.AtkModify /= 2;
        }

        if (CardPair.card.Freeze > 0)
        {
            CardPair.card.Freeze--;
        }

        if (CardPair.card.Charge > 0)
        {
            CardPair.card.Charge--;
            CardPair.card.AtkModify--;
            CardPair.card.DefModify--;

        }
        if (CardPair.card.innateSkills.Delay > 0)
        {
            CardPair.card.innateSkills.Delay--;
        }

        int healthChange = CardPair.card.Poison;
        CardPair.card.DefDamage += healthChange;
    }

    private void SingularityEffect()
    {
        if (!CardPair.card.innateSkills.Singularity) { return; }
        List<string> singuEffects = new() { "Chaos", "Copy", "Nova" };
        if (CardPair.card.AtkNow > 0)
        {
            CardPair.card.atk = -(Mathf.Abs(CardPair.card.atk));
            CardPair.card.AtkModify = -(Mathf.Abs(CardPair.card.AtkModify));
        }

        if (!CardPair.card.innateSkills.Immaterial)
        {
            singuEffects.Add("Immaterial");
        }
        if (!CardPair.card.passiveSkills.Adrenaline)
        {
            singuEffects.Add("Adrenaline");
        }
        if (!CardPair.card.passiveSkills.Vampire)
        {
            singuEffects.Add("Vampire");
        }

        switch (singuEffects[Random.Range(0, singuEffects.Count)])
        {
            case "Immaterial":
                CardPair.card.innateSkills.Immaterial = true;
                break;
            case "Vampire":
                CardPair.card.passiveSkills.Vampire = true;
                break;
            case "Chaos":
                int chaos = Random.Range(1, 6);
                CardPair.card.AtkModify += chaos;
                CardPair.card.DefModify += chaos;
                break;
            case "Nova":
                for (int y = 0; y < 12; y++)
                {
                    DuelManager.GetNotIDOwner(CardPair.id).GenerateQuantaLogic((Element)y, 1);
                }
                break;
            case "Addrenaline":
                CardPair.card.passiveSkills.Adrenaline = true;
                break;
            default:
                Card duplicate = new(CardPair.card);
                Game_AnimationManager.shared.StartAnimation("ParallelUniverse", transform);
                DuelManager.GetIDOwner(CardPair.id).PlayCardOnFieldLogic(duplicate);
                break;
        }
    }
}