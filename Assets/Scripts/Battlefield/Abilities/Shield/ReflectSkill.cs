namespace Battlefield.Abilities
{
    public class ReflectSkill : ShieldSkill
    {
        public override int ActivateSkill(int atkNow, (ID id, Card card) cardPair)
        {
            if (!cardPair.card.passiveSkills.Psion) return atkNow;
            EventBus<ModifyPlayerHealthLogicEvent>.Raise(new ModifyPlayerHealthLogicEvent(atkNow, cardPair.id.owner, false));
            return 0;
        }
    }
}