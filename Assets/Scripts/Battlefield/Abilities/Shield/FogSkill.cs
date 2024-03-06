namespace Battlefield.Abilities
{
    public class FogSkill : ShieldSkill
    {
        public override int ActivateSkill(int atkNow, (ID id, Card card) cardPair)
        {
            return UnityEngine.Random.Range(0f, 1f) <= 0.4f ? 0 : atkNow;
        }
    }
}