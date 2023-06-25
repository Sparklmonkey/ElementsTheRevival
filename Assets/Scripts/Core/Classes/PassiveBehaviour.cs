using System.Collections.Generic;
using UnityEngine;

public class PassiveBehaviour : CardTypeBehaviour
{
    public override void OnCardPlay()
    {
        if (CardPair.card.skill == "" || CardPair.card.skill == "none") { return; }

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
            case "5v2":
            case "7ti":
                CardPair.card.TurnsInPlay = 3;
                break;
            default:
                break;
        }
    }

    public override void OnCardRemove()
    {
        if (CardPair.card.skill == "bones")
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
            case "5v2":
            case "7ti":
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
        if (CardPair.card.skill == "bones")
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
            string skill = CardPair.card.skill;
            int atknow = CardPair.card.AtkNow;

            switch (skill)
            {
                case "fiery":
                    atknow += Mathf.FloorToInt(Owner.GetAllQuantaOfElement(Element.Fire) / 5);
                    break;
                case "hammer":
                    if (Owner.playerPassiveManager.GetMark().card.costElement == Element.Earth || Owner.playerPassiveManager.GetMark().card.costElement == Element.Gravity)
                    {
                        atknow++;
                    }
                    break;
                case "dagger":
                    if (Owner.playerPassiveManager.GetMark().card.costElement == Element.Death || Owner.playerPassiveManager.GetMark().card.costElement == Element.Darkness)
                    {
                        atknow++;
                    }
                    break;
                case "bow":
                    if (Owner.playerPassiveManager.GetMark().card.costElement == Element.Air)
                    {
                        atknow++;
                    }
                case "regenerate":
                    Owner.ModifyHealthLogic(5, false, false);
                    break;
                default:
                    break;
            }

            if (CardPair.card.Freeze > 0)
            {
                atknow = 0;
                CardPair.card.Freeze--;
            }

            if (CardPair.card.IsDelayed)
            {
                atknow = 0;
                CardPair.card.innate.Remove("delay");
            }

            if (!CardPair.card.passive.Contains("momentum"))
            {
                Enemy.ManageShield(ref atknow, ref CardPair);
            }

            //Send Damage
            Enemy.ModifyHealthLogic(atknow, true, false);

            if (atknow > 0)
            {
                if (skill == "vampire")
                {
                    Owner.ModifyHealthLogic(atknow, false, false);
                }
                if (skill == "venom")
                {
                    Enemy.AddPlayerCounter(PlayerCounters.Poison, 1);
                }
                if (skill == "scramble")
                {
                    Enemy.ScrambleQuanta();
                }
            }
        }

        
    }
}