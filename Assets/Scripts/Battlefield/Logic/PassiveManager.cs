
using System;
using System.Collections.Generic;

namespace Elements.Duel.Manager
{

    public class PassiveManager : FieldManager
    {
        public PassiveManager(List<ID> idList)
        {
            pairList = new List<IDCardPair>();
            for (int i = 0; i < idList.Count; i++)
            {
                pairList.Add(new IDCardPair(idList[i], null));
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
    }
}