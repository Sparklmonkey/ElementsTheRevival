namespace Battlefield.Abilities
{
    class ScavengerDeathTrigger : DeathTriggerAbility
    {
        public override void Activate(ID owner, Card card)
        {
            card.AtkModify += 1;
            card.DefModify += 1;
            EventBus<UpdateCreatureCardEvent>.Raise(new UpdateCreatureCardEvent(owner, card, true));
        }
    }
}