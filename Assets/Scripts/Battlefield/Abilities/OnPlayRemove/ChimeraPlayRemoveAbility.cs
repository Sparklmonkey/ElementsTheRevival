using System.Linq;

namespace Battlefield.Abilities
{
    class ChimeraPlayRemoveAbility : OnPlayRemoveAbility
    {
        public override void OnPlayActivate(ID owner, Card card)
        {
            var creatureList = DuelManager.Instance.GetIDOwner(owner).playerCreatureField.GetAllValidCardIds();
            var chimeraPwrHp = (0, 0);

            if (creatureList.Count > 0)
            {
                foreach (var creature in creatureList.Where(creature => !creature.Item1.Equals(owner)))
                {
                    chimeraPwrHp.Item1 += creature.Item2.AtkNow;
                    chimeraPwrHp.Item2 += creature.Item2.DefNow;
                    EventBus<ClearCardDisplayEvent>.Raise(new ClearCardDisplayEvent(creature.Item1));
                }
            }

            card.Atk = chimeraPwrHp.Item1;
            card.Def = chimeraPwrHp.Item2;
        }
    }
}