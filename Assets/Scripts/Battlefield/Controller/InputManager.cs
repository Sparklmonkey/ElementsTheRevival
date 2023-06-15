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
    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;
    private float touchStart;
    // Start is called before the first frame update
    void Start()
    {

        //Fetch the Raycaster from the GameObject (the Canvas)
        m_Raycaster = GetComponent<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        m_EventSystem = GetComponent<EventSystem>();
    }

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

        if (Input.GetMouseButtonDown(0))
        {
            isFingerOnScreen = true;
            touchStart = Time.time;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isFingerOnScreen = false;
            //Set up the new Pointer Event
            m_PointerEventData = new PointerEventData(m_EventSystem);
            //Set the Pointer Event Position to that of the mouse position
            m_PointerEventData.position = Input.mousePosition;

            //Create a list of Raycast Results
            List<RaycastResult> results = new List<RaycastResult>();

            //Raycast using the Graphics Raycaster and mouse click position
            m_Raycaster.Raycast(m_PointerEventData, results);
            //Check if anything was hit by raycast
            if (results.Count < 1)
            {
                GameObject gObject = GameObject.Find("CardDetailView");
                if (gObject != null)
                {
                    gObject.GetComponent<CardDetailView>().CancelButtonAction();
                }
                return;
            }
            //Check if fieldObject was hit
            var cardPairObject = GetIDCardPair(results);
            if (cardPairObject == null)
            {
                return;
            }
            if (BattleVars.shared.isSelectingTarget && DuelManager.GetAllValidTargets().Contains(cardPairObject))
            {
                DuelManager.Instance.player.ActivateAbility(cardPairObject);
                return;
            }
            DuelManager.Instance.player.StartCoroutine(DuelManager.Instance.player.ManageID(cardPairObject));
        }
    }

    private IDCardPair GetIDCardPair(List<RaycastResult> raycastResults)
    {
        foreach (RaycastResult item in raycastResults)
        {
            var returnResult = item.gameObject.GetComponentInChildren<IDCardPair>();
            if (returnResult != null)
            {
                return returnResult;
            }
        }
        return null;
    }
}

