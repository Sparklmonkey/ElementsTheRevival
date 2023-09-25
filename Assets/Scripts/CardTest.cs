using System.Collections.Generic;
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
    void Start()
    {
        var debugString = "";
        CardDatabase.Instance.FullCardList = CardDatabase.Instance.SetupNewCardBase();
        for (int i = 0; i < 13; i++)
        {
            var element = (Element)i;
            debugString += $"**{element.FastElementString()}: ";

            for (int j = 0; j < 6; j++)
            {
                var cardType = (CardType)j;
                int cardCount = CardDatabase.Instance.FullCardList
                    .FindAll(x => x.costElement == element && x.cardType == cardType).Count;
                debugString += $"\n * {cardType.FastCardTypeString()} - {cardCount}";
            }
            debugString += $"\n ";
        }
        
        Debug.Log(debugString);
    }
}