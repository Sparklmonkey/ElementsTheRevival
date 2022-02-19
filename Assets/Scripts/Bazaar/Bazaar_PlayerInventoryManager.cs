using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bazaar_PlayerInventoryManager : InventoryManager
{
    [SerializeField]
    private TextMeshProUGUI cardCount;
    [SerializeField]
    private List<Card> testList;

    public void SetupPlayerInvetoryView(List<Card> cardList)
    {
        SetupContentView(cardList, true);
    }

}
