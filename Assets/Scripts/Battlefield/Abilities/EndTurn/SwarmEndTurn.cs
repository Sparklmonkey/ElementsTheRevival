namespace Battlefield.Abilities
{
    public class SwarmEndTurn : OnEndTurnAbility
    {
        public override void Activate(ID owner, Card card)
        {
            var player = DuelManager.Instance.GetIDOwner(owner);
            card.Def += player.playerCounters.scarabOld;
            EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(owner, card, true));
        }
    }
}