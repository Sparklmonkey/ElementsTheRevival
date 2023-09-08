using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeckSelector : MonoBehaviour
{
    private Element playerDeck;

    [SerializeField]
    private List<StarterDeckCardHead> cardHeads;

    [SerializeField]
    private GameObject selectorWheel, selectedObject, descPanel, cardObject;
    [SerializeField]
    private TextMeshProUGUI elementDescription;

    [SerializeField]
    private CardDisplay cardDisplayDetail, cardOne, cardTwo;

    public void Awake()
    {
        ElementSelection(Element.Aether);
    }
    public void ElementSelection(Element element)
    {
        playerDeck = element;
        elementDescription.text = ElementStrings.GetElementDescription(playerDeck);
        ClearGridView();
        List<Card> starterDeck = StarterDecks.Instance.GetStarterDeck(playerDeck).DeserializeCard();

        for (int i = 0; i < cardHeads.Count; i++)
        {
            cardHeads[i].SetupCardHead(starterDeck[i], cardDisplayDetail);
        }
        cardDisplayDetail.gameObject.SetActive(false);
        (string, string) displayCards = StarterDecks.Instance.GetDisplayCards(playerDeck);
        cardOne.SetupCardView(CardDatabase.Instance.GetCardFromId(displayCards.Item1));
        cardTwo.SetupCardView(CardDatabase.Instance.GetCardFromId(displayCards.Item2));
    }

    private void ClearGridView()
    {

    }


    public void StartGame()
    {
        PlayerData.shared.markElement = playerDeck;
        PlayerData.shared.currentDeck = StarterDecks.Instance.GetStarterDeck(playerDeck);
        PlayerData.shared.cardInventory = new List<string>();
        SceneTransitionManager.Instance.LoadScene("Dashboard");
    }
}

