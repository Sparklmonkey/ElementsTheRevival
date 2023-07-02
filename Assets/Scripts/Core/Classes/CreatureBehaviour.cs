using System.Collections.Generic;
using UnityEngine;

public class CreatureBehaviour : CardTypeBehaviour
{
    public override void OnCardPlay()
    {
        if (CardPair.card.skill == "") { return; }

        if (CardPair.card.innate.Contains("swarm"))
        {
            Owner.AddPlayerCounter(PlayerCounters.Scarab, 1);
        }

        if (CardPair.card.innate.Contains("integrity"))
        {
            var shardList = Owner.playerHand.GetAllValidCardIds().FindAll(x => x.card.cardName.Contains("Shard of"));
            CardPair.card = CardDatabase.Instance.GetGolemAbility(shardList);
        }

        if (CardPair.card.innate.Contains("chimera"))
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
        if (CardPair.card.innate.Contains("swarm"))
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

        if (CardPair.card.passive.Count == 0) { return; }

        if (CardPair.card.passive.Contains("phoenix"))
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
        if (CardPair.card.passive.Contains("scavenger"))
        {
            CardPair.card.AtkModify += 1;
            CardPair.card.DefModify += 1;
            CardPair.UpdateCard();
        }

    }

    public override void OnTurnEnd()
    {
        int adrenalineIndex = 0;
        bool hasAdrenaline = CardPair.card.passive.Contains("adrenaline");
        bool isFirstAttack = true;
        int atkNow = CardPair.card.AtkNow;

        bool shouldSkip = DuelManager.IsSundialInPlay() || Owner.playerCounters.patience > 0 || CardPair.card.Freeze > 0 || CardPair.card.IsDelayed || atkNow == 0;
        CardPair.card.AbilityUsed = false;

        while (isFirstAttack || hasAdrenaline)
        {
            isFirstAttack = false;
            if (!shouldSkip)
            {
                bool isFreedomEffect = Random.Range(0, 100) < (25 * Owner.playerCounters.freedom) && CardPair.card.costElement.Equals(Element.Air);
                atkNow = Mathf.FloorToInt(isFreedomEffect ? atkNow * 1.5f : atkNow);
                Game_SoundManager.shared.PlayAudioClip("CreatureDamage");
                Enemy.ManageGravityCreatures(ref atkNow, ref CardPair);
                if (CardPair.card.DefNow <= 0)
                {
                    CardPair.RemoveCard();
                    return;
                }

                if (!CardPair.card.passive.Contains("momentum"))
                {
                    Enemy.ManageShield(ref atkNow, ref CardPair);
                    if (CardPair.card.DefNow <= 0)
                    {
                        CardPair.RemoveCard();
                        return;
                    }
                }

                if (atkNow > 0)
                {
                    if (CardPair.card.passive.Contains("venom"))
                    {
                        Enemy.AddPlayerCounter(PlayerCounters.Poison, 1);
                    }
                    if (CardPair.card.passive.Contains("deadly venom"))
                    {
                        Enemy.AddPlayerCounter(PlayerCounters.Poison, 2);
                    }
                    if (CardPair.card.passive.Contains("neurotoxin"))
                    {
                        Enemy.AddPlayerCounter(PlayerCounters.Neurotoxin, 1);
                    }
                }
                if (atkNow != 0)
                {
                    if (CardPair.card.passive.Contains("vampire"))
                    {
                        Owner.ModifyHealthLogic(atkNow, false, false);
                    }
                    Enemy.ModifyHealthLogic(atkNow, true, CardPair.card.IsPsion);
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
                        if (CardPair.card.passive.Contains("antimatter"))
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
        if (!CardPair.card.IsDelayed
                    && CardPair.card.passive?.Count > 0)
        {
            if (CardPair.card.passive.Contains("air"))
            {
                Game_AnimationManager.shared.StartAnimation("QuantaGenerate", transform, Element.Air);
                Owner.GenerateQuantaLogic(Element.Air, 1);
            }
            if (CardPair.card.passive.Contains("earth"))
            {
                Game_AnimationManager.shared.StartAnimation("QuantaGenerate", transform, Element.Earth);
                Owner.GenerateQuantaLogic(Element.Earth, 1);
            }
            if (CardPair.card.passive.Contains("fire"))
            {
                Game_AnimationManager.shared.StartAnimation("QuantaGenerate", transform, Element.Fire);
                Owner.GenerateQuantaLogic(Element.Fire, 1);
            }
            if (CardPair.card.passive.Contains("light"))
            {
                Game_AnimationManager.shared.StartAnimation("QuantaGenerate", transform, Element.Light);
                Owner.GenerateQuantaLogic(Element.Light, 1);
            }
            if (CardPair.card.passive.Contains("devourer"))
            {
                if (Enemy.GetAllQuantaOfElement(Element.Other) > 0 && Enemy.playerCounters.sanctuary == 0)
                {
                    Enemy.SpendQuantaLogic(Element.Other, 1);
                    Game_AnimationManager.shared.StartAnimation("QuantaGenerate", transform, Element.Darkness);
                    Owner.GenerateQuantaLogic(Element.Darkness, 1);
                }
            }
            if (CardPair.card.passive.Contains("overdrive"))
            {
                CardPair.card.AtkModify += 3;
                CardPair.card.DefModify -= 1;
            }
            if (CardPair.card.passive.Contains("acceleration"))
            {
                CardPair.card.AtkModify += 2;
                CardPair.card.DefModify -= 1;
            }
            if (CardPair.card.passive.Contains("infest"))
            {
                Owner.PlayCardOnFieldLogic(CardDatabase.Instance.GetCardFromId("4t8"));
            }
        }

        if (CardPair.card.innate.Contains("swarm"))
        {
            CardPair.card.def = Owner.playerCounters.scarab;
        }
    }

    private void CreatureTurnDownTick()
    {
        if (CardPair.card.passive.Contains("dive"))
        {
            CardPair.card.passive.Remove("dive");
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
        if (CardPair.card.IsDelayed)
        {
            CardPair.card.innate.Remove("delay");
        }

        int healthChange = CardPair.card.Poison;
        CardPair.card.DefDamage += healthChange;
    }

    private void SingularityEffect()
    {
        if (!CardPair.card.passive.Contains("singularity")) { return; }
        List<string> singuEffects = new() { "Chaos", "Copy", "Nova" };
        if (CardPair.card.AtkNow > 0)
        {
            CardPair.card.atk = -(Mathf.Abs(CardPair.card.atk));
            CardPair.card.AtkModify = -(Mathf.Abs(CardPair.card.AtkModify));
        }

        if (!CardPair.card.innate.Contains("immaterial"))
        {
            singuEffects.Add("Immaterial");
        }
        if (!CardPair.card.passive.Contains("adrenaline"))
        {
            singuEffects.Add("Addrenaline");
        }
        if (!CardPair.card.passive.Contains("vampire"))
        {
            singuEffects.Add("Vampire");
        }

        switch (singuEffects[Random.Range(0, singuEffects.Count)])
        {
            case "Immaterial":
                CardPair.card.innate.Add("immaterial");
                break;
            case "Vampire":
                CardPair.card.passive.Add("vampire");
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
                CardPair.card.passive.Add("adrenaline");
                break;
            default:
                Card duplicate = new(CardPair.card);
                Game_AnimationManager.shared.StartAnimation("ParallelUniverse", transform);
                DuelManager.GetIDOwner(CardPair.id).PlayCardOnFieldLogic(duplicate);
                break;
        }
    }
}