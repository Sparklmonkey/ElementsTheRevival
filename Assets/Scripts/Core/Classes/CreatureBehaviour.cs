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
        Owner.AddPlayerCounter(PlayerCounters.Scarab, cardPair.card.innateSkills.Swarm ? -1 : 0);
        var shouldDeath = ShouldActivateDeathTriggers();
        if (cardPair.card.IsAflatoxin)
        {
            cardPair.PlayCard(CardDatabase.Instance.GetCardFromId("6ro"));
        } 
        else if (cardPair.card.passiveSkills.Phoenix && !shouldDeath)
        {
            cardPair.PlayCard(CardDatabase.Instance.GetCardFromId(cardPair.card.iD.IsUpgraded() ? "7dt" : "5fd"));
        }
        else
        {
            cardPair.card = null;
        }
        
        if (shouldDeath)
        {
            DuelManager.Instance.ActivateDeathTriggers();
        }
    }

    private bool ShouldActivateDeathTriggers()
    {
        if (BattleVars.Shared.AbilityOrigin is not null)
        {
            return BattleVars.Shared.AbilityOrigin.card.skill is not "reversetime";
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

        bool shouldSkip = DuelManager.Instance.GetCardCount(new() { "5rp", "7q9" }) > 0 || 
                          Owner.playerCounters.patience > 0 || 
                          cardPair.card.Freeze > 0 || 
                          cardPair.card.innateSkills.Delay > 0;
        cardPair.card.AbilityUsed = false;

        while (isFirstAttack || hasAdrenaline)
        {
            isFirstAttack = false;
            if (!shouldSkip)
            {
                cardPair.CardInnateEffects(ref atkNow);
                var isFreedomEffect = Random.Range(0, 100) < (25 * Owner.playerCounters.freedom) && cardPair.card.costElement.Equals(Element.Air);
                atkNow = Mathf.FloorToInt(isFreedomEffect ? atkNow * 1.5f : atkNow);
                SoundManager.Instance.PlayAudioClip("CreatureDamage");
                if (atkNow > 0)
                {
                    if (Enemy.HasGravityCreatures())
                    {
                        Enemy.ManageGravityCreatures(ref atkNow);
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
                }

                if (adrenalineIndex < 2)
                {
                    cardPair.EndTurnPassiveEffect();

                    if (atkNow > 0)
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

                    if (atkNow != 0)
                    {
                        if (cardPair.card.passiveSkills.Vampire)
                        {
                            Owner.ModifyHealthLogic(atkNow, false, false);
                        }

                        Enemy.ModifyHealthLogic(atkNow, true, cardPair.card.passiveSkills.Psion);
                    }
                }

                cardPair.SingularityEffect();
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
            cardPair.CreatureTurnDownTick();
            cardPair.UpdateCard();
        }
    }
}
