using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeckSelector : MonoBehaviour
{
    private bool canSelect;

    private Element playerDeck;

    [SerializeField]
    private Image playerDeckImage;

    [SerializeField]
    private Transform startPosition, deckGridView;

    private Vector3 finalPosition;

    [SerializeField]
    private GameObject selectorWheel, selectedObject, descPanel, cardObject;
    [SerializeField]
    private TextMeshProUGUI elementDescription;

    [SerializeField]
    private CardDisplayDetail cardDisplayDetail;

    private void Start()
    {
        finalPosition = selectorWheel.transform.position;
        selectorWheel.transform.position = startPosition.position;
        StartCoroutine(MoveSelector());
    }

    private IEnumerator MoveSelector()
    {
        while (selectorWheel.transform.position != finalPosition)
        {
            selectorWheel.transform.position = Vector3.MoveTowards(selectorWheel.transform.position, finalPosition, 1000f * Time.deltaTime);
            yield return null;
        }
        canSelect = true;
        selectedObject.SetActive(true);
        descPanel.SetActive(true);
        ElementSelection(0);
    }

    public void ElementSelection(int element)
    {
        if (!canSelect)
        {
            return;
        }
        playerDeck = (Element)element;
        elementDescription.text = ElementStrings.GetElementDescription(playerDeck);
        playerDeckImage.sprite = ImageHelper.GetElementImage(playerDeck.ToString());
        playerDeckImage.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
        ClearGridView();
        StarterDeck starterDeck = Resources.Load<StarterDeck>("StarterDecks/" + playerDeck.ToString());

        foreach (Card card in starterDeck.deck)
        {
            GameObject cardObject = Instantiate(this.cardObject, deckGridView);
            cardObject.GetComponent<CardDisplay>().SetupCardView(card, cardDisplayDetail);
        }
    }

    private void ClearGridView()
    {
        foreach (Transform child in deckGridView.transform)
        {
            Destroy(child.gameObject);
        }
    }


    public async void StartGame()
    {
        if (playerDeckImage.sprite != null)
        {
            StarterDeck newDeck = Resources.Load<StarterDeck>($"StarterDecks/{playerDeck.FastElementString()}");
            PlayerData.shared.markElement = playerDeck;
            PlayerData.shared.currentDeck = newDeck.deck.SerializeCard();
            PlayerData.shared.cardInventory = new List<CardObject>();
            SceneManager.LoadScene("Dashboard");
        }
    }
}

