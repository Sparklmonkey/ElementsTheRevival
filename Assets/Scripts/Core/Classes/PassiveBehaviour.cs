using System.Collections.Generic;
using UnityEngine;

public class PassiveBehaviour : CardTypeBehaviour
{
    public override void OnCardPlay()
    {
        switch (CardPair.card.iD)
        {
            case "7q9":
            case "5rp":
                CardPair.card.TurnsInPlay = 1;
                break;
            case "7n8":
            case "5oo":
                CardPair.card.TurnsInPlay = 5;
                break;
            case "61t":
            case "80d":
                CardPair.card.TurnsInPlay = 3;
                break;
            default:
                break;
        }

        if (CardPair.card.innateSkills.Bones)
        {
            Owner.AddPlayerCounter(PlayerCounters.Bone, 7);
        }
    }

    public override void OnCardRemove()
    {
        if (CardPair.card.innateSkills.Bones)
        {
            Owner.AddPlayerCounter(PlayerCounters.Bone, -9999);
        }
    }

    public override void OnTurnStart()
    {
        switch (CardPair.card.iD)
        {
            case "7q9":
            case "5rp":
                CardPair.card.TurnsInPlay--;
                if(CardPair.card.TurnsInPlay <= 0)
                {
                    CardPair.RemoveCard();
                    return;
                }
                break;
            case "7n8":
            case "5oo":
                CardPair.card.TurnsInPlay--;
                if (CardPair.card.TurnsInPlay <= 0)
                {
                    CardPair.RemoveCard();
                    return;
                }
                break;
            case "61t":
            case "80d":
                CardPair.card.TurnsInPlay--;
                if (CardPair.card.TurnsInPlay <= 0)
                {
                    CardPair.RemoveCard();
                    return;
                }
                break;
            default:
                break;
        }
        CardPair.UpdateCard();
    }

    public override void DeathTrigger()
    {
        if (CardPair.card.innateSkills.Bones)
        {
            Owner.AddPlayerCounter(PlayerCounters.Bone, 2);
        }
    }

    public override void OnTurnEnd()
    {
        if(CardPair.card.cardType == CardType.Mark)
        {
            Game_AnimationManager.shared.PlayAnimation("QuantaGenerate", transform, CardPair.card.costElement);

            if (BattleVars.shared.enemyAiData.maxHP >= 150 && CardPair.id.Owner == OwnerEnum.Opponent)
            {
                Owner.GenerateQuantaLogic(CardPair.card.costElement, 3);
            }
            else
            {
                Owner.GenerateQuantaLogic(CardPair.card.costElement, 1);
            }
        }

        if (CardPair.card.cardType == CardType.Weapon)
        {
            int atknow = CardPair.card.AtkNow;

            if (CardPair.card.innateSkills.Regenerate)
            {
                Owner.ModifyHealthLogic(5, false, false);
            }
            if (CardPair.card.innateSkills.Fiery)
            {
                atknow += Mathf.FloorToInt(Owner.GetAllQuantaOfElement(Element.Fire) / 5);
            }
            if (CardPair.card.innateSkills.Hammer)
            {
                if (Owner.playerPassiveManager.GetMark().card.costElement == Element.Earth || Owner.playerPassiveManager.GetMark().card.costElement == Element.Gravity)
                {
                    atknow++;
                }
            }
            if (CardPair.card.innateSkills.Dagger)
            {
                if (Owner.playerPassiveManager.GetMark().card.costElement == Element.Death || Owner.playerPassiveManager.GetMark().card.costElement == Element.Darkness)
                {
                    atknow++;
                }
            }
            if (CardPair.card.innateSkills.Bow)
            {
                if (Owner.playerPassiveManager.GetMark().card.costElement == Element.Air)
                {
                    atknow++;
                }
            }

            if (CardPair.card.Freeze > 0)
            {
                atknow = 0;
                CardPair.card.Freeze--;
            }

            if (CardPair.card.innateSkills.Delay > 0)
            {
                atknow = 0;
                CardPair.card.innateSkills.Delay--;
            }

            if (!CardPair.card.passiveSkills.Momentum)
            {
                Enemy.ManageShield(ref atknow, ref CardPair);
            }

            //Send Damage
            Enemy.ModifyHealthLogic(atknow, true, false);

            if (atknow > 0)
            {
                if (CardPair.card.passiveSkills.Vampire)
                {
                    Owner.ModifyHealthLogic(atknow, false, false);
                }
                if (CardPair.card.passiveSkills.Venom)
                {
                    Enemy.AddPlayerCounter(PlayerCounters.Poison, 1);
                }
                if (CardPair.card.innateSkills.Scramble)
                {
                    Enemy.ScrambleQuanta();
                }
            }
            CardPair.card.AbilityUsed = false;
        }

        
    }
}