namespace Battlefield.Abilities
{
    public class DivineShieldEndTurn : OnEndTurnAbility
    {
        public override void Activate(ID owner, Card card)
        {
            if (card.passiveSkills.DivineShield <= 0) return;
            card.passiveSkills.DivineShield--;
            if (card.passiveSkills.DivineShield <= 0)
            {
                card.innateSkills.Immaterial = false;
                EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(owner, card, true));
            }
        }
    }
    
    public class SteamEndTurn : OnEndTurnAbility
    {
        public override void Activate(ID owner, Card card)
        {
            if (card.Counters.Charge <= 0) return;
            card.Counters.Charge--;
            card.AtkModify--;
            EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(owner, card, true));
        }
    }
}