using UnityEngine;

public class Pvp_PlayerController
{
    PlayerManager enemyManager;
    public Pvp_PlayerController(PlayerManager enemyManager)
    {
        this.enemyManager = enemyManager;
    }

    public void GetPvpAction(PvP_Action pvP_Action)
    {
        Debug.Log(pvP_Action.ActionType);
        //switch (pvP_Action.ActionType)
        //{
        //    //Play card that is not a spell from hand
        //    case ActionType.PlayCardToField:
        //        //enemyManager.PlayCardFromHandLogicWithLocation(pvP_Action.OriginId, pvP_Action.TargetId);
        //        break;
        //    //Play a spell from hand
        //    case ActionType.PlaySpell:
        //        BattleVars.shared.originId = pvP_Action.OriginId;

        //        enemyManager.PlayCardFromHandLogic(pvP_Action.OriginId);
        //        enemyManager.GetCard(pvP_Action.OriginId).spellAbility.ActivateAbility(pvP_Action.TargetId);
        //        BattleVars.shared.originId = null;
        //        break;
        //    case ActionType.CardOnFieldAbility:
        //        Card card = enemyManager.GetCard(pvP_Action.OriginId);
        //        BattleVars.shared.originId = pvP_Action.OriginId;

        //        enemyManager.SpendQuantaLogic(card.skill.AbilityElement, card.skill.AbilityCost);
        //        card.skill.ActivateAbility(pvP_Action.TargetId);
        //        BattleVars.shared.originId = null;
        //        break;
        //    case ActionType.EndTurn:
        //        DuelManager.EndTurn();
        //        break;
        //    case ActionType.DiscardCard:
        //        enemyManager.DiscardCard(pvP_Action.OriginId);
        //        break;
        //    default:
        //        break;
        //}
    }
}