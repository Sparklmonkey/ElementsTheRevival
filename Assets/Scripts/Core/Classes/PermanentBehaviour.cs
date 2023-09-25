using System.Collections.Generic;

public class PermanentBehaviour : CardTypeBehaviour
{
    public override void OnCardPlay()
    {
        switch (cardPair.card.iD)
        {
            case "7q9":
            case "5rp":
                cardPair.card.TurnsInPlay = 2;
                Owner.AddPlayerCounter(PlayerCounters.Delay, 1);
                DuelManager.Instance.GetNotIDOwner(cardPair.id).AddPlayerCounter(PlayerCounters.Delay, 1);
                break;
            case "5v2":
            case "7ti":
                Owner.AddPlayerCounter(PlayerCounters.Invisibility, 3);
                cardPair.card.TurnsInPlay = 3;
                Owner.ActivateCloakEffect(cardPair);
                break;
            case "5j2":
            case "7hi":
                Owner.AddPlayerCounter(PlayerCounters.Patience, 1);
                break;
            case "5uq":
            case "7ta":
                DuelManager.Instance.UpdateNightFallEclipse(true, cardPair.card.iD);
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

        if (cardPair.card.innateSkills.Sanctuary)
        {
            Owner.AddPlayerCounter(PlayerCounters.Sanctuary, 1);
        }
    }

    public override void OnCardRemove()
    {
        switch (cardPair.card.iD)
        {
            case "5v2":
            case "7ti":
                Owner.ResetCloakPermParent(cardPair);
                if (Owner.playerPermanentManager.GetAllValidCardIds().FindAll(x => x.card.iD == "5v2" || x.card.iD == "7ti").Count == 1)
                {
                    Owner.DeactivateCloakEffect();
                    Owner.AddPlayerCounter(PlayerCounters.Invisibility, -3);
                }
                break;
            case "5j2":
            case "7hi":
                Owner.AddPlayerCounter(PlayerCounters.Patience, -1);
                break;
            case "5uq":
            case "7ta":
                DuelManager.Instance.UpdateNightFallEclipse(false, cardPair.card.iD);
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

        if (cardPair.card.innateSkills.Sanctuary)
        {
            Owner.AddPlayerCounter(PlayerCounters.Sanctuary, -1);
        }
        if(cardPair.stackCount == 0)
        {
            cardPair.card = null;
        }
    }

    public override void OnTurnStart()
    {
        List<string> permanentsWithCountdown = new() { "7q9", "5rp", "5v2", "7ti" };
        cardPair.card.AbilityUsed = false;
        if (permanentsWithCountdown.Contains(cardPair.card.iD))
        {
            cardPair.card.TurnsInPlay--;

            if (cardPair.card.TurnsInPlay == 0)
            {
                cardPair.RemoveCard();
                return;
            }
        }
        cardPair.UpdateCard();
    }

    public override void DeathTrigger()
    {
        if (cardPair.card.innateSkills.SoulCatch)
        {
            Owner.GenerateQuantaLogic(Element.Death, cardPair.card.iD.IsUpgraded() ? 3 : 2);
        }
        if (cardPair.card.innateSkills.Boneyard)
        {
            Owner.PlayCardOnFieldLogic(CardDatabase.Instance.GetCardFromId(cardPair.card.iD.IsUpgraded() ? "716" : "52m"));
        }
    }

    public override void OnTurnEnd()
    {
        if (cardPair.card.cardType == CardType.Artifact)
        {
            if (cardPair.card.innateSkills.Empathy)
            {
                int creatureCount = Owner.playerCreatureField.GetAllValidCardIds().Count;
                Owner.ModifyHealthLogic(creatureCount, false, false);
            }
            if (cardPair.card.innateSkills.Gratitude)
            {
                int healthAmount = Owner.playerPassiveManager.GetMark().card.costElement == Element.Life ? 5 : 3;
                Owner.ModifyHealthLogic(healthAmount, false, false);
            }

            List<int> floodList = new() { 11, 13, 9, 10, 12 };
            switch (cardPair.card.iD)
            {
                case "5j2":
                case "7hi":
                    var creatureList = Owner.playerCreatureField.GetAllValidCardIds();
                    foreach (var creature in creatureList)
                    {
                        int statModifier = DuelManager.Instance.GetCardCount(new() { "5ih", "7h1" }) > 0 && floodList.Contains(cardPair.id.index) ? 5 : 2;
                        creature.card.AtkModify += statModifier;
                        creature.card.AtkModify += statModifier;
                        cardPair.UpdateCard();
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

            if (cardPair.card.innateSkills.Sanctuary)
            {
                Owner.ModifyHealthLogic(4, false, false);
            }

            if (cardPair.card.innateSkills.Void)
            {
                int healthChange = Owner.playerPassiveManager.GetMark().card.costElement == Element.Darkness ? 3 : 2;
                DuelManager.Instance.GetNotIDOwner(cardPair.id).ModifyMaxHealthLogic(healthChange, false);
            }
        }

        if (cardPair.card.cardType == CardType.Pillar)
        {
            if (cardPair.card.cardName.Contains("Pendulum"))
            {
                Owner.GenerateQuantaLogic(cardPair.card.skillElement, cardPair.card.costElement == Element.Other ? 3 * StackCount : 1 * StackCount);
                if (Owner.isPlayer || Owner.playerCounters.invisibility <= 0)
                {
                    AnimationManager.Instance.StartAnimation("QuantaGenerate", transform, cardPair.card.costElement);
                }

                cardPair.card.skillElement = cardPair.card.skillElement == cardPair.card.costElement ? DuelManager.Instance.GetIDOwner(cardPair.id).playerPassiveManager.GetMark().card.costElement : cardPair.card.costElement;
            }
            else
            {
                Owner.GenerateQuantaLogic(cardPair.card.costElement, cardPair.card.costElement == Element.Other ? 3 * StackCount : 1 * StackCount);
                if (Owner.isPlayer || Owner.playerCounters.invisibility <= 0)
                {
                    AnimationManager.Instance.StartAnimation("QuantaGenerate", transform, cardPair.card.costElement);
                }
            }
        }

    }
}