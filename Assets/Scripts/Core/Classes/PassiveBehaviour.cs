using UnityEngine;

public class PassiveBehaviour : CardTypeBehaviour
{
    public override void OnCardPlay()
    {
        switch (cardPair.card.iD)
        {
            case "7n8":
            case "5oo":
                cardPair.card.TurnsInPlay = 5;
                break;
            case "61t":
            case "80d":
                cardPair.card.TurnsInPlay = 3;
                break;
            default:
                break;
        }

        if (cardPair.card.innateSkills.Bones)
        {
            Owner.AddPlayerCounter(PlayerCounters.Bone, 7);
        }
    }

    public override void OnCardRemove()
    {
        if (cardPair.card.innateSkills.Bones)
        {
            Owner.AddPlayerCounter(PlayerCounters.Bone, -9999);
        }

        if (cardPair.card.cardType == CardType.Weapon)
        {
            cardPair.PlayCard(CardDatabase.Instance.GetPlaceholderCard(1));
        }
        else if (cardPair.card.cardType == CardType.Shield)
        {
            cardPair.PlayCard(CardDatabase.Instance.GetPlaceholderCard(2));
        }
    }

    public override void OnTurnStart()
    {
        switch (cardPair.card.iD)
        {
            case "7n8":
            case "5oo":
                cardPair.card.TurnsInPlay--;
                break;
            case "61t":
            case "80d":
                cardPair.card.TurnsInPlay--;
                break;
        }
    }

    public override void DeathTrigger()
    {
        if (cardPair.card.innateSkills.Bones)
        {
            Owner.AddPlayerCounter(PlayerCounters.Bone, 2);
        }
    }

    public override void OnTurnEnd()
    {
        if (cardPair.card.cardType == CardType.Mark)
        {
            AnimationManager.Instance.StartAnimation("QuantaGenerate", transform, cardPair.card.costElement);
            if (BattleVars.Shared.EnemyAiData.maxHp >= 150 && cardPair.id.owner == OwnerEnum.Opponent)
            {
                Owner.GenerateQuantaLogic(cardPair.card.costElement, 3);
            }
            else
            {
                Owner.GenerateQuantaLogic(cardPair.card.costElement, 1);
            }
        }

        if (cardPair.card.cardType == CardType.Weapon)
        {
            int atknow = cardPair.card.AtkNow;

            if (cardPair.card.innateSkills.Regenerate)
            {
                Owner.ModifyHealthLogic(5, false, false);
            }
            if (cardPair.card.innateSkills.Fiery)
            {
                atknow += Mathf.FloorToInt(Owner.GetAllQuantaOfElement(Element.Fire) / 5);
            }
            if (cardPair.card.innateSkills.Hammer)
            {
                if (Owner.playerPassiveManager.GetMark().card.costElement == Element.Earth || Owner.playerPassiveManager.GetMark().card.costElement == Element.Gravity)
                {
                    atknow++;
                }
            }
            if (cardPair.card.innateSkills.Dagger)
            {
                if (Owner.playerPassiveManager.GetMark().card.costElement == Element.Death || Owner.playerPassiveManager.GetMark().card.costElement == Element.Darkness)
                {
                    atknow++;
                }
            }
            if (cardPair.card.innateSkills.Bow)
            {
                if (Owner.playerPassiveManager.GetMark().card.costElement == Element.Air)
                {
                    atknow++;
                }
            }

            if (cardPair.card.Freeze > 0)
            {
                atknow = 0;
                cardPair.card.Freeze--;
            }

            if (cardPair.card.innateSkills.Delay > 0)
            {
                atknow = 0;
                cardPair.card.innateSkills.Delay--;
            }

            if (!cardPair.card.passiveSkills.Momentum)
            {
                Enemy.ManageShield(ref atknow, ref cardPair);
            }

            //Send Damage
            Enemy.ModifyHealthLogic(atknow, true, false);

            if (atknow > 0)
            {
                if (cardPair.card.passiveSkills.Vampire)
                {
                    Owner.ModifyHealthLogic(atknow, false, false);
                }
                if (cardPair.card.passiveSkills.Venom)
                {
                    Enemy.AddPlayerCounter(PlayerCounters.Poison, 1);
                }
                if (cardPair.card.innateSkills.Scramble)
                {
                    Enemy.ScrambleQuanta();
                }
            }
            cardPair.card.AbilityUsed = false;
        }


    }
}