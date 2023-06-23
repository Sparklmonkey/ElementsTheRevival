public class Shieldbones : ShieldAbility
{
    public override void ActivateShield(ref int atkNow, ref IDCardPair cardPair)
    {
        atkNow = 0;
        Owner.AddPlayerCounter(PlayerCounters.Bone, -1);
        if (Owner.playerCounters.bone <= 0)
        {
            Owner.playerPassiveManager.RemoveShield();
        }
    }
}
