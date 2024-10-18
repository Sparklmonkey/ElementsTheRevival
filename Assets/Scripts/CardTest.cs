using System;
using System.Collections.Generic;
using System.Linq;
using Networking;
using Newtonsoft.Json;
using UnityEngine;

public class CardTest : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        var elderAi = Resources.LoadAll<EnemyAi>($"EnemyAi/Level3/");
        foreach (var elder in elderAi)
        {
            Debug.Log($"--------{elder.opponentName}---------");
            var deck = new List<string>(elder.deck.Split(" ")).DeserializeCard();
            foreach (var card in deck)
            {
                if (card.Type is not CardType.Spell) continue;
                Debug.Log($"--------{card.CardName}---------");
                Debug.Log(card.Id);
            }
        }
    }

    private void ViewedNewsTest()
    {
        var idList = "";
        foreach (var card in CardDatabase.Instance.FullCardList)
        {
            idList += $" \"{card.Id}\",";
        }
        
        Debug.Log(idList);
        // for (int i = 0; i < 12; i++)
        // {
        //     var regularNymph = CardDatabase.Instance.GetRandomRegularNymph((Element)i);
        //     var uppedNymph = CardDatabase.Instance.GetRandomEliteNymph((Element)i);
        //     Debug.Log($"Regular ID: {regularNymph.Id}");
        //     Debug.Log($"Upped ID: {uppedNymph.Id}");
        //     Debug.Log($"Element: {(Element)i}");
        // }
    } 
}

[Serializable]
public class CardDBLegacy
{
    public List<CardDefinition> cardDb;
} 