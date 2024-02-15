namespace Battlefield.Abilities
{
    public class DivineShieldEndTurn : OnEndTurnAbility
    {
        public override void Activate(ID owner, Card card)
        {
            if (!card.passiveSkills.DivineShield) return;
            card.passiveSkills.DivineShield.Toggle();
            card.innateSkills.Immaterial = false;
            EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(owner, card, true));
        }
    }
    
    public class SteamEndTurn : OnEndTurnAbility
    {
        public override void Activate(ID owner, Card card)
        {
            if (card.Charge <= 0) return;
            card.Charge--;
            card.AtkModify--;
            EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(owner, card, true));
        }
    }
}