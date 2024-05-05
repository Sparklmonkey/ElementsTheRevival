using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

public class CardTest : MonoBehaviour
{
    [SerializeField]
    private List<Card> cardList;
    [SerializeField]
    private Transform card;
    public Dictionary<string, int> CardDict = new ();
    private string _oetgDeckCode = "0a0va081da061c4061up022530624t02252018pu";

    // Start is called before the first frame update
    private void Start()
    {
        var json = Resources.Load<TextAsset>("Cards/CardDatabase");

        var cardBodyStringArray = JsonConvert.DeserializeObject<CardDBLegacy>(json.text);
        Debug.Log("Card Check");
        foreach (var VARIABLE in CardDatabase.Instance.FullCardList)
        {
            if (cardBodyStringArray.cardDb.Find(x => x.iD == VARIABLE.Id) is null)
            {
                Debug.Log(VARIABLE.CardName);
            }
        }
    }
}

[Serializable]
public class CardDBLegacy
{
    public List<CardDefinition> cardDb;
} 