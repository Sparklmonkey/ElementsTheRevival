using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command_PlayActionAnimation : Command
{
    private PlayerManager playerManager;
    ID cardID;
    Element element;
    public Command_PlayActionAnimation(PlayerManager playerManager, ID cardID, Element element)
    {
        this.playerManager = playerManager;
        this.cardID = cardID;
        this.element = element;
    }

    public override void StartCommandExecution()
    {
        void EffectMark() => playerManager.PlayActionAnimationVisual(cardID);
        Game_AnimationManager.PlayAnimation("QuantaGenerate", Battlefield_ObjectIDManager.shared.GetObjectFromID(cardID), EffectMark, element);
    }
}
