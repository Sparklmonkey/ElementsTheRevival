using System;

namespace Elements.Duel.Manager
{
    [Serializable]
    public class PassiveManager : FieldManager
    {

        public IDCardPair PlayPassive(Card card)
        {

            switch (card.cardType)
            {
                case CardType.Weapon:
                    pairList[1].PlayCard(card);
                    return pairList[1];
                case CardType.Shield:
                    pairList[2].PlayCard(card);
                    return pairList[2];
                case CardType.Mark:
                    pairList[0].PlayCard(card);
                    return pairList[0];
                default:
                    break;
            }
            return null;
        }

        public IDCardPair GetShield()
        {
            return pairList[2];
        }
        public IDCardPair GetWeapon()
        {
            return pairList[1];
        }

        public IDCardPair GetMark()
        {
            return pairList[0];
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

        public void PassiveTurnDown()
        {
            foreach (var idCard in pairList)
            {
                if(!idCard.HasCard()) { continue; }
                idCard.cardBehaviour.OnTurnStart();
                idCard.UpdateCard();
            }
        }

        internal void RemoveWeapon()
        {
            pairList[1].PlayCard(CardDatabase.Instance.GetPlaceholderCard(1));
        }

        internal void RemoveShield()
        {
            pairList[2].PlayCard(CardDatabase.Instance.GetPlaceholderCard(0));
        }
    }
}