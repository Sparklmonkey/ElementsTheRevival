using System;
using UnityEngine;

public class ActionDisplayManager : MonoBehaviour
{
    [SerializeField]
    private Transform actionContentView;
    [SerializeField]
    private GameObject actionCellPrefab, actionViewObject;

    private ActionManager _actionManager;

    private void OnEnable()
    {
        _actionManager = new ActionManager();
    }

    public void SetupActionView()
    {
        if (_actionManager.ActionList == null) { return; }
        actionViewObject.SetActive(true);
        if (_actionManager.ActionList.Count == 0) { return; }

        ClearView();
        for (var i = _actionManager.ActionList.Count - 1; i >= 0; i--)
        {
            var actionCellObject = Instantiate(actionCellPrefab, actionContentView);
            actionCellObject.GetComponent<ActionCell>().SetupFromElementAction(_actionManager.ActionList[i]);
        }
    }

    private void ClearView()
    {
        foreach (Transform item in actionContentView)
        {
            Destroy(item.gameObject);
        }
    }

    private void OnDisable()
    {
        ClearView();
    }
}
