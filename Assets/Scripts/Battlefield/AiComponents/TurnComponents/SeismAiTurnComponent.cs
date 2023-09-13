using System.Collections;

public class SeismAiTurnComponent : AiBaseFunctions, IAiTurnComponent
{


    public IEnumerator RestOfTurn(PlayerManager aiManager)
    {

        //Play Arsenic if none in play
        yield return aiManager.StartCoroutine(PlayWeapon(aiManager, "Elite Pulverizer"));
        //Play Arsenic if none in play
        yield return aiManager.StartCoroutine(PlayShield(aiManager, "Diamond Shield"));

        //Play Creatures
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Elite Antlion"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Steel Golem"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Basalt Dragon"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Gnome Gemfinder"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Elite Graboid"));
        yield return aiManager.StartCoroutine(PlayPermanent(aiManager, "Silurian Dragon"));


        //Activate Graboids
        yield return aiManager.StartCoroutine(ActivateRepeatAbilityNoTarget(aiManager, CardType.Creature, "Graboid", "Elite Graboid"));
        //Activate Stone Skin
        yield return aiManager.StartCoroutine(ActivateRepeatSpellNoTarget(aiManager, "Stone Skin", "Granite Skin"));

        yield return aiManager.StartCoroutine(ActivateRepeatSpellWithTarget(aiManager, "Reverse Time", "Rewind"));
        yield return aiManager.StartCoroutine(ActivateRepeatSpellWithTarget(aiManager, "Earthquake", "Quicksand"));
    }
}
