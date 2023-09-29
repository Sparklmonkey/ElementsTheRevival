using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeckSelector : MonoBehaviour
{
    private Element _playerDeck;

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
        _playerDeck = element;
        elementDescription.text = ElementStrings.GetElementDescription(_playerDeck);
        ClearGridView();
        List<Card> starterDeck = StarterDecks.Instance.GetStarterDeck(_playerDeck).DeserializeCard();

        for (int i = 0; i < cardHeads.Count; i++)
        {
            cardHeads[i].SetupCardHead(starterDeck[i], cardDisplayDetail);
        }
        cardDisplayDetail.gameObject.SetActive(false);
        (string, string) displayCards = StarterDecks.Instance.GetDisplayCards(_playerDeck);
        cardOne.SetupCardView(CardDatabase.Instance.GetCardFromId(displayCards.Item1));
        cardTwo.SetupCardView(CardDatabase.Instance.GetCardFromId(displayCards.Item2));
    }

    private void ClearGridView()
    {

    }


    public void StartGame()
    {
        PlayerData.Shared.markElement = _playerDeck;
        PlayerData.Shared.currentDeck = StarterDecks.Instance.GetStarterDeck(_playerDeck);
        PlayerData.Shared.inventoryCards = new List<string>();
        SceneTransitionManager.Instance.LoadScene("Dashboard");
    }
}

