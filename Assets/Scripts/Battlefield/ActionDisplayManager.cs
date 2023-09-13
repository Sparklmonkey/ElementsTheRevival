using UnityEngine;

public class ActionDisplayManager : MonoBehaviour
{
    [SerializeField]
    private Transform actionContentView;
    [SerializeField]
    private GameObject actionCellPrefab, actionViewObject;


    public void SetupActionView()
    {
        if (ActionManager.actionList == null) { return; }
        actionViewObject.SetActive(true);
        if (ActionManager.actionList.Count == 0) { return; }

        ClearView();
        for (int i = ActionManager.actionList.Count - 1; i >= 0; i--)
        {
            GameObject actionCellObject = Instantiate(actionCellPrefab, actionContentView);
            actionCellObject.GetComponent<ActionCell>().SetupFromElementAction(ActionManager.actionList[i]);
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
