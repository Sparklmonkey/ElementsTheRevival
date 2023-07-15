using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardTest : MonoBehaviour
{
    [SerializeField]
    private List<Card> cardList;
    public Dictionary<string, int> cardDict = new ();


    // Start is called before the first frame update
    void Start()
    {
        CardDatabase.Instance.SetupNewCardBase();
        foreach (var card in CardDatabase.Instance.fullCardList)
        {
            if(card.cardType == CardType.Spell)
            {
                if(card.skill == "" || card.skill == " ")
                {
                    Debug.Log(card.cardName);
                }
            }
        }
    }

}