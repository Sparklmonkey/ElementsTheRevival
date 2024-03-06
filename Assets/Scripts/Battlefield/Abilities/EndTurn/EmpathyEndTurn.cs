namespace Battlefield.Abilities
{
    public class EmpathyEndTurn : OnEndTurnAbility
    {
        public override void Activate(ID owner, Card card)
        {
            var creatureCount = DuelManager.Instance.GetIDOwner(owner).playerCreatureField.GetAllValidCardIds().Count;
            EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(creatureCount, false, false, owner.owner));
        }
    }
}
