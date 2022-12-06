using System.Collections;

public class AkebonoAiTurnComponent : AiBaseFunctions, IAiTurnComponent
{
    public IEnumerator RestOfTurn(PlayerManager aiManager)
    {
        yield return aiManager.StartCoroutine(PlayWeapon(aiManager, "Elite Titan"));
        yield return aiManager.StartCoroutine(PlayShield(aiManager, "Tower Shield"));

        //Play Creatures
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Massive Dragon"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Elite Chimera"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Steel Armagio"));

        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Shard of Focus"));

        yield return aiManager.StartCoroutine(ActivateRepeatAbilityWithTarget(aiManager, CardType.Creature, "Shard of Focus", "Shard of Focus", x => x.Owner.Equals(OwnerEnum.Player)));

        yield return aiManager.StartCoroutine(ActivateRepeatAbilityNoTarget(aiManager, CardType.Creature, "Armagio", "Elite Armagio"));
        
        yield return aiManager.StartCoroutine(ActivateRepeatSpellWithTarget(aiManager, "Acceleration", "Overdrive", x => x.Owner.Equals(OwnerEnum.Opponent)));
        yield return aiManager.StartCoroutine(ActivateRepeatSpellWithTarget(aiManager, "Momentum", "Unstoppable", x => x.Owner.Equals(OwnerEnum.Opponent)));
    }
}
