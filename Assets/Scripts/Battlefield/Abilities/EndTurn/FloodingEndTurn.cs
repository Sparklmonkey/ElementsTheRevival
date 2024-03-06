using System.Collections.Generic;
using System.Linq;

namespace Battlefield.Abilities
{
    public class FloodingEndTurn : OnEndTurnAbility
    {
        private List<int> _saveZones = new() { 11, 13, 9, 10, 12 };
        public override void Activate(ID owner, Card card)
        {
            var player = DuelManager.Instance.GetIDOwner(owner);
            var idList = player.playerCreatureField.GetAllValidCardIds();
            foreach (var idCard in from idCard in idList where !_saveZones.Contains(idCard.Item1.index) 
                     where idCard.Item2.CardElement is not Element.Other and Element.Water where !idCard.IsTargetable() select idCard)
            {
                EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(idCard.Item1));
            }

            if (player.GetAllQuantaOfElement(Element.Water) == 0)
            {
                EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(owner));
                return;
            }
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(1, Element.Water, owner.owner, false));
        }
    }
}