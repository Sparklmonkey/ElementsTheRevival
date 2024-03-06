using UnityEngine;

namespace Battlefield.Abilities
{
    public abstract class ShieldSkill
    {
        public virtual int ActivateSkill(int atkNow, (ID id, Card card) cardPair)
        {
            return atkNow;
        }
    }
}