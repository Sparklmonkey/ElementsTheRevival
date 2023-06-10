
using System;
using System.Collections.Generic;
using Elements.Duel.Visual;

namespace Elements.Duel.Manager
{

    public class PassiveManager : FieldManager
    {
        private List<PassiveInPlay> _passiveDisplayers;
        public PassiveManager(List<ID> idList, List<PassiveInPlay> passiveDisplayers)
        {
            _passiveDisplayers = passiveDisplayers;
            pairList = new List<IDCardPair>();
            for (int i = 0; i < idList.Count; i++)
            {
                pairList.Add(new IDCardPair(idList[i], null));
                pairList[i].OnCardChanged += _passiveDisplayers[i].DisplayCard;
            }
        }

        public ID PlayPassive(Card card)
        {

            switch (card.cardType)
            {
                case CardType.Weapon:
                    pairList[1].card = card;
                    return pairList[1].id;
                case CardType.Shield:
                    pairList[2].card = card;
                    return pairList[2].id;
                case CardType.Mark:
                    pairList[0].card = card;
                    return pairList[0].id;
                default:
                    break;
            }
            return null;
        }

        public Card GetShield()
        {
            return pairList[2].card;
        }
        public Card GetWeapon()
        {
            return pairList[1].card;
        }

        public Card GetMark()
        {
            return pairList[0].card;
        }

        internal void FreezeWeapon(int amount)
        {
            throw new NotImplementedException();
        }

        public ID GetMarkID()
        {
            return pairList[0].id;
        }

        internal ID GetShieldID()
        {
            return pairList[2].id;
        }
        internal ID GetWeaponID()
        {
            return pairList[1].id;
        }

        private readonly List<string> passiveWithCountdown = new() { "7n8", "5oo", "61t", "80d" };

        public void PassiveTurnDown()
        {
            foreach (var idCard in pairList)
            {
                if(!idCard.HasCard()) { continue; }
                if (passiveWithCountdown.Contains(idCard.card.iD))
                {
                    idCard.card.AbilityUsed = false;
                    idCard.card.TurnsInPlay--;
                    if(idCard.card.TurnsInPlay == 0)
                    {
                        idCard.RemoveCard();
                        return;
                    }
                }
                idCard.UpdateCard();
            }
        }

        internal void RemoveWeapon()
        {
            pairList[1].PlayCard(CardDatabase.Instance.GetPlaceholderCard(1));
        }
    }
}