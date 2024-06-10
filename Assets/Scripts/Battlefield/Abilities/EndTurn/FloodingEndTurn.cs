using System.Collections.Generic;
using System.Linq;

namespace Battlefield.Abilities
{
    public class FloodingEndTurn : OnEndTurnAbility
    {
        public override void Activate(ID owner, Card card)
        {
            var saveZones = new List<int> { 10, 11, 12, 13, 9 };
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
            var checkList = idList.FindAll(x => !saveZones.Contains(x.id.index));
            if (checkList.Count == 0) return;
            checkList = checkList.FindAll(x => x.card.CardElement is not Element.Other and Element.Water);
            if (checkList.Count == 0) return;
            checkList = checkList.FindAll(x => x.IsTargetable());
            if (checkList.Count == 0) return;
            foreach (var idCard in checkList)
            {
                EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(idCard.Item1));
            }
        }
    }
}