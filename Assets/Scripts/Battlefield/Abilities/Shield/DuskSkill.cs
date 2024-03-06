namespace Battlefield.Abilities
{
    public class DuskSkill : ShieldSkill
    {
        public override int ActivateSkill(int atkNow, (ID id, Card card) cardPair)
        {
            return UnityEngine.Random.Range(0f, 1f) <= 0.5f ? 0 : atkNow;
        }
    }
}