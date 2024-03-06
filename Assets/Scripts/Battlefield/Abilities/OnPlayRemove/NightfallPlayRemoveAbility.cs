namespace Battlefield.Abilities
{
    class NightfallPlayRemoveAbility : OnPlayRemoveAbility
    {
        public override void OnPlayActivate(ID owner, Card card)
        {
            DuelManager.Instance.UpdateNightFallEclipse(true, true);
        }

        public override void OnRemoveActivate(ID owner, Card card)
        {
            DuelManager.Instance.UpdateNightFallEclipse(false, true);
        }
    }
}