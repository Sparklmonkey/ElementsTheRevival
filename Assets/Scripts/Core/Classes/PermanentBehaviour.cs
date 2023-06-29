using System.Collections.Generic;
using UnityEngine;

public class PermanentBehaviour : CardTypeBehaviour
{
    public override void OnCardPlay()
    {
        if (CardPair.card.skill == "" || CardPair.card.skill == "none") { return; }

        switch (CardPair.card.iD)
        {
            case "7q9":
            case "5rp":
                CardPair.card.TurnsInPlay = 1;
                Owner.AddPlayerCounter(PlayerCounters.Delay, 1);
                DuelManager.GetNotIDOwner(CardPair.id).AddPlayerCounter(PlayerCounters.Delay, 1);
                break;
            case "5v2":
            case "7ti":
                Owner.AddPlayerCounter(PlayerCounters.Invisibility, 3);
                CardPair.card.TurnsInPlay = 3;
                Owner.ActivateCloakEffect(CardPair);
                break;
            case "5j2":
            case "7hi":
                Owner.AddPlayerCounter(PlayerCounters.Patience, 1);
                break;
            case "5uq":
            case "7ta":
                DuelManager.Instance.UpdateNightFallEclipse(true, CardPair.card.iD);
                break;
            case "5pa":
            case "7nq":
                Owner.AddPlayerCounter(PlayerCounters.Freedom, 1);
                break;
            case "5ih":
            case "7h1":
                DuelManager.Instance.AddFloodCount(1);
                break;
            default:
                break;
        }

        if (CardPair.card.passive.Contains("sanctuary"))
        {
            Owner.AddPlayerCounter(PlayerCounters.Sanctuary, 1);
        }
    }

    public override void OnCardRemove()
    {
        switch (CardPair.card.iD)
        {
            case "5v2":
            case "7ti":
                if (Owner.playerPermanentManager.GetAllValidCardIds().FindAll(x => x.card.skill == "cloak").Count == 1)
                {
                    Owner.DeactivateCloakEffect(CardPair);
                    Owner.AddPlayerCounter(PlayerCounters.Invisibility, -3);
                }
                break;
            case "5j2":
            case "7hi":
                Owner.AddPlayerCounter(PlayerCounters.Patience, -1);
                break;
            case "5uq":
            case "7ta":
                DuelManager.Instance.UpdateNightFallEclipse(false, CardPair.card.iD);
                break;
            case "5pa":
            case "7nq":
                Owner.AddPlayerCounter(PlayerCounters.Freedom, -1);
                break;
            case "5ih":
            case "7h1":
                DuelManager.Instance.AddFloodCount(-1);
                break;
            default:
                break;
        }

        if (CardPair.card.passive.Contains("sanctuary"))
        {
            Owner.AddPlayerCounter(PlayerCounters.Sanctuary, -1);
        }
    }

    public override void OnTurnStart()
    {
        List<string> permanentsWithCountdown = new() { "7q9", "5rp", "5v2", "7ti" };
        CardPair.card.AbilityUsed = false;
        if (permanentsWithCountdown.Contains(CardPair.card.iD))
        {
            CardPair.card.TurnsInPlay--;

            if (CardPair.card.TurnsInPlay == 0)
            {
                CardPair.RemoveCard();
                return;
            }
        }
        CardPair.UpdateCard();
    }

    public override void DeathTrigger()
    {
        if (CardPair.card.innate.Contains("soul catch"))
        {
            Owner.GenerateQuantaLogic(Element.Death, CardPair.card.iD.IsUpgraded() ? 3 : 2);
        }
        if (CardPair.card.innate.Contains("boneyard"))
        {
            Owner.PlayCardOnFieldLogic(CardDatabase.Instance.GetCardFromId(CardPair.card.iD.IsUpgraded() ? "716" : "52m"));
        }
    }

    public override void OnTurnEnd()
    {
        if (CardPair.card.cardType == CardType.Artifact)
        {
            if (CardPair.card.innate.Contains("empathy"))
            {
                int creatureCount = Owner.playerCreatureField.GetAllValidCardIds().Count;
                Owner.ModifyHealthLogic(creatureCount, false, false);
            }
            if (CardPair.card.innate.Contains("gratitude"))
            {
                int healthAmount = Owner.playerPassiveManager.GetMark().card.costElement == Element.Life ? 5 : 3;
                Owner.ModifyHealthLogic(healthAmount, false, false);
            }

            List<int> floodList = new() { 11, 13, 9, 10, 12 };
            switch (CardPair.card.iD)
            {
                case "5j2":
                case "7hi":
                    var creatureList = Owner.playerCreatureField.GetAllValidCardIds();
                    foreach (var creature in creatureList)
                    {
                        int statModifier = DuelManager.IsFloodInPlay() && floodList.Contains(CardPair.id.Index) ? 5 : 2;
                        creature.card.AtkModify += statModifier;
                        creature.card.AtkModify += statModifier;
                        CardPair.UpdateCard();
                    }
                    break;
                case "5ih":
                case "7h1":
                    DuelManager.Instance.enemy.ClearFloodedArea(floodList);
                    DuelManager.Instance.player.ClearFloodedArea(floodList);
                    break;
                default:
                    break;
            }

            if (CardPair.card.innate.Contains("sanctuary"))
            {
                Owner.ModifyHealthLogic(4, false, false);
            }

            if (CardPair.card.innate.Contains("void"))
            {
                int healthChange = Owner.playerPassiveManager.GetMark().card.costElement == Element.Darkness ? 3 : 2;
                DuelManager.GetNotIDOwner(CardPair.id).ModifyMaxHealthLogic(healthChange, false);
            }
        }

        if (CardPair.card.cardType == CardType.Pillar)
        {
            if (CardPair.card.cardName.Contains("Pendulum"))
            {
                Owner.GenerateQuantaLogic(CardPair.card.skillElement, CardPair.card.costElement == Element.Other ? 3 * StackCount : 1 * StackCount);
                StartCoroutine(Game_AnimationManager.shared.PlayAnimation("QuantaGenerate", transform, CardPair.card.costElement));

                CardPair.card.skillElement = CardPair.card.skillElement == CardPair.card.costElement ? DuelManager.GetIDOwner(CardPair.id).playerPassiveManager.GetMark().card.costElement : CardPair.card.costElement;
            }
            else
            {
                Owner.GenerateQuantaLogic(CardPair.card.costElement, CardPair.card.costElement == Element.Other ? 3 * StackCount : 1 * StackCount);
                StartCoroutine(Game_AnimationManager.shared.PlayAnimation("QuantaGenerate", transform, CardPair.card.costElement));
            }
        }

    }
}