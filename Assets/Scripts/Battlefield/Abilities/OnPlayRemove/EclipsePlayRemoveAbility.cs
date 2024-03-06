namespace Battlefield.Abilities
{
    class EclipsePlayRemoveAbility : OnPlayRemoveAbility
    {
        public override void OnPlayActivate(ID owner, Card card)
        {
            DuelManager.Instance.UpdateNightFallEclipse(true, false);
        }

        public override void OnRemoveActivate(ID owner, Card card)
        {
            DuelManager.Instance.UpdateNightFallEclipse(false, false);
        }
    }
}