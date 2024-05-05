using System;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayerTest : MonoBehaviour
{
    public CardDatabase CardDatabase;
    private void Start()
    {
        foreach (var card in CardDatabase.FullCardList)
        {
            if (CardDatabase.FullCardList.FindAll(c => c.Equals(card)).Count > 1)
            {
                Debug.Log(card.CardName);
            }
        }
    }
}
