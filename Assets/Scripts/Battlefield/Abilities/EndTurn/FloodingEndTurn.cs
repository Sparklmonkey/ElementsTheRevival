using System.Collections.Generic;
using System.Linq;

namespace Battlefield.Abilities
{
    public class FloodingEndTurn : OnEndTurnAbility
    {
        public override void Activate(ID owner, Card card)
        {
            var player = DuelManager.Instance.GetIDOwner(owner);
            var enemy = DuelManager.Instance.GetNotIDOwner(owner);
            if (player.GetAllQuantaOfElement(Element.Water) == 0)
            {
                EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(owner));
                return;
            }
            EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(1, Element.Water, owner.owner, false));
            var idList = player.playerCreatureField.GetAllValidCardIds();
            idList.AddRange(enemy.playerCreatureField.GetAllValidCardIds());
            if (idList.Count == 0)
            {
                return;
            }
            foreach (var idCardPair in idList)
            {
                if(idCardPair.id.index is 9 or 10 or 11 or 12 or 13) continue;
                if(idCardPair.card.CostElement is Element.Other or Element.Water) continue;
                if(idCardPair.card.innateSkills.Immaterial || idCardPair.card.passiveSkills.Burrow) continue;
                EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(idCardPair.id));
            }
        }
    }
}