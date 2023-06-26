using Elements.Duel.Visual;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public static bool isFingerOnScreen;


    [SerializeField]
    Material dissolveMaterial;
    

    // Update is called once per frame
    void Update()
    {
        if (!DuelManager.allPlayersSetup)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space) && DuelManager.Instance.endTurnButton.interactable)
        {
            BattleVars.shared.spaceTapped = true;
            DuelManager.Instance.PlayerEndTurn();
            return;
        }

    }
}

