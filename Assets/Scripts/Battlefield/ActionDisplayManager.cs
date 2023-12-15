using UnityEngine;

public class ActionDisplayManager : MonoBehaviour
{
    [SerializeField]
    private Transform actionContentView;
    [SerializeField]
    private GameObject actionCellPrefab, actionViewObject;


    public void SetupActionView()
    {
        if (ActionManager.ActionList == null) { return; }
        actionViewObject.SetActive(true);
        if (ActionManager.ActionList.Count == 0) { return; }

        ClearView();
        for (var i = ActionManager.ActionList.Count - 1; i >= 0; i--)
        {
            var actionCellObject = Instantiate(actionCellPrefab, actionContentView);
            actionCellObject.GetComponent<ActionCell>().SetupFromElementAction(ActionManager.ActionList[i]);
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
