using System;
using System.Collections.Generic;
using UnityEngine;

public class CardTest : MonoBehaviour
{
    [SerializeField]
    private List<Card> cardList;
    public Dictionary<string, int> cardDict = new ();
    private string oetgDeckCode = "0a0va081da061c4061up022530624t02252018pu";

    // Start is called before the first frame update
    void Start()
    {
        CardDatabase.Instance.SetupNewCardBase();

        var legacyList = oetgDeckCode.ConvertOetgToLegacy();

        foreach (var item in legacyList)
        {
            Debug.Log(item);
        }
    }


    
}