using System.Collections.Generic;
using System.Linq;
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
        var starterDeck = CardDatabase.Instance.StarterDecks.First(x => x.MarkElement.Equals(element));

        for (var i = 0; i < cardHeads.Count; i++)
        {
            cardHeads[i].SetupCardHead(starterDeck.DeckList[i], cardDisplayDetail);
        }
        cardDisplayDetail.gameObject.SetActive(false);
        
        cardOne.SetupCardView(starterDeck.HighlightOne);
        cardTwo.SetupCardView(starterDeck.HighlightTwo);
    }

    private void ClearGridView()
    {

    }


    public void StartGame()
    {
        PlayerData.Shared.MarkElement = _playerDeck;
        PlayerData.Shared.SetDeck(CardDatabase.Instance.StarterDecks.First(x => x.MarkElement.Equals(_playerDeck)).DeckList.SerializeCard());
        PlayerData.Shared.InventoryCards = "X";
        SceneTransitionManager.Instance.LoadScene("Dashboard");
    }
}

