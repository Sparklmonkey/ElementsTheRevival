namespace Battlefield.Abilities
{
    public class GratitudeEndTurn : OnEndTurnAbility
    {
        public override void Activate(ID owner, Card card)
        {
            var player = DuelManager.Instance.GetIDOwner(owner);
            var healthAmount = player.playerPassiveManager.GetMark().Item2.CardElement == Element.Life ? 5 : 3;
            EventBus<ModifyPlayerHealthEvent>.Raise(new ModifyPlayerHealthEvent(healthAmount, false, false, owner.owner));
        }
    }
}