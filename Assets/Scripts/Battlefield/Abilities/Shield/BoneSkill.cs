namespace Battlefield.Abilities
{
    public class BoneSkill : ShieldSkill
    {
        public override int ActivateSkill(int atkNow, (ID id, Card card) cardPair)
        {
            if (DuelManager.Instance.GetNotIDOwner(cardPair.id).playerCounters.bone <= 0) return atkNow;
            EventBus<ModifyPlayerCounterEvent>.Raise(new ModifyPlayerCounterEvent(PlayerCounters.Bone, cardPair.id.owner, -1));
            return 0;

        }
    }
}