using System;
using System.Collections.Generic;

namespace Elements.Duel.Manager
{
    [Serializable]
    public class PassiveManager : FieldManager
    {

        public void PlayPassive(Card card)
        {

            switch (card.cardType)
            {
                case CardType.Weapon:
                    PairList[1].PlayCard(card);
                    break;
                case CardType.Shield:
                    PairList[2].PlayCard(card);
                    break;
                case CardType.Mark:
                    PairList[0].PlayCard(card);
                    break;
            }
        }

        public IDCardPair GetShield()
        {
            return PairList[2];
        }
        public IDCardPair GetWeapon()
        {
            return PairList[1];
        }

        public IDCardPair GetMark()
        {
            return PairList[0];
        }
        public ID GetMarkID()
        {
            return PairList[0].id;
        }

        internal ID GetShieldID()
        {
            return PairList[2].id;
        }
        internal ID GetWeaponID()
        {
            return PairList[1].id;
        }
        private readonly List<string> _turnCount = new() { "7n8", "5oo", "61t", "80d" };
        public void PassiveTurnDown()
        {
            foreach (var idCard in PairList)
            {
                if (!idCard.HasCard()) { continue; }
                idCard.cardBehaviour.OnTurnStart();
                if (idCard.card.TurnsInPlay <= 0 && _turnCount.Contains(idCard.card.iD))
                {
                    if (idCard.id.index == 2)
                    {
                        RemoveShield();
                    }
                }
                idCard.UpdateCard();
            }
        }

        internal void RemoveWeapon()
        {
            PairList[1].PlayCard(CardDatabase.Instance.GetPlaceholderCard(1));
        }

        internal void RemoveShield()
        {
            PairList[2].PlayCard(CardDatabase.Instance.GetPlaceholderCard(0));
        }
    }
}