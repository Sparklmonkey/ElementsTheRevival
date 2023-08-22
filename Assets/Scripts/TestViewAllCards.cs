using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestViewAllCards : MonoBehaviour
{
    public List<Card> cardList;
    public GameObject cardPrefab;
    public Transform contentView;
    // Start is called before the first frame update
    //void Start()
    //{
        //SetupStarterDecks();
        //StarterDeck starterDeck = Resources.Load<StarterDeck>("StarterDecks/Aether");
        //Debug.Log(starterDeck.Test5.Count);
        //foreach (Card card in cardList)
        //{
        //    GameObject cardObject = Instantiate(cardPrefab, contentView);
        //    cardObject.GetComponent<CardDisplay>().SetupCardView(card);
        //}
    //}

    public void ChangeDeckView(int element)
    {
        string elementToFind = ((Element)element).ToString();

        StarterDeck starterDeck = Resources.Load<StarterDeck>("StarterDecks/" + elementToFind);

        starterDeck.deck = starterDeck.deck.SortDeck();
        foreach (Transform child in contentView)
        {
            Destroy(child.gameObject);
        }

        foreach (Card card in starterDeck.deck)
        {
            GameObject cardObject = Instantiate(cardPrefab, contentView);
            cardObject.GetComponent<CardDisplay>().SetupCardView(card);
        }
    }


    private readonly string baseSOpath = @"Cards";

}


