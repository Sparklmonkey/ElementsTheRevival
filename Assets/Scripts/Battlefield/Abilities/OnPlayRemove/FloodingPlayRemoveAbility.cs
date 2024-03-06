namespace Battlefield.Abilities
{
    class FloodingPlayRemoveAbility : OnPlayRemoveAbility
    {
        public override void OnPlayActivate(ID owner, Card card)
        {
            DuelManager.Instance.AddFloodCount(1);
        }

        public override void OnRemoveActivate(ID owner, Card card)
        {
            DuelManager.Instance.AddFloodCount(-1);
        }
    }
}