using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    private Transform contentView;
    [SerializeField]
    private Bazaar_CardDisplayManager cardDisplayManager;
    [SerializeField]
    private GameObject cardHeadPrefab;
    private List<DMCardPrefabNoTT> dMCards = new List<DMCardPrefabNoTT>();

    private bool isInventory;
    public void SetupContentView(List<Card> cardList, bool isInventory)
    {
        this.isInventory = isInventory;
        ClearContentView();
        if (isInventory)
        {
            UpdateCardView();
        }
        else
        {
            foreach (Card card in cardList)
            {
                GameObject cardHeadObject = Instantiate(cardHeadPrefab, contentView);
                cardHeadObject.GetComponent<DMCardPrefabNoTT>().SetupCardHead(card, isInventory, this);
            }
        }
    }

    public void ShowCardDisplay(Card card)
    {
        cardDisplayManager.gameObject.SetActive(true);
        cardDisplayManager.SetupCardDisplay(card, !isInventory);
    }

    public void HideCardDisplay()
    {
        cardDisplayManager.gameObject.SetActive(false);
    }

    public void ChangeCardOwner(DMCardPrefabNoTT cardObject)
    {
        Card card = cardObject.GetCard();
        if (isInventory)
        {
            GetComponent<Bazaar_PlayerDataManager>().ModifyPlayerCardInventory(card, false);
        }
        else if(GetComponent<Bazaar_PlayerDataManager>().CanBuyCard(card.BuyPrice))
        {
            GetComponent<Bazaar_PlayerDataManager>().ModifyPlayerCardInventory(card, true);
        }

        GetComponent<Bazaar_PlayerInventoryManager>().UpdateCardView();
    }

    public void ClearContentView()
    {
        List<DMCardPrefabNoTT> children = new List<DMCardPrefabNoTT>(contentView.GetComponentsInChildren<DMCardPrefabNoTT>());
        foreach (DMCardPrefabNoTT child in children)
        {
            Destroy(child.gameObject);
        }
    }

    public void UpdateCardView()
    {
        dMCards = new List<DMCardPrefabNoTT>();
        ClearContentView();
        List<Card> cardList = PlayerData.shared.cardInventory.DeserializeCard();
        cardList.Sort((x, y) => string.Compare(x.iD, y.iD));
        foreach (var card in cardList)
        {
            DMCardPrefabNoTT dMCard = dMCards.Find(x => x.GetCard().cardName == card.cardName);
            if(dMCard != null)
            {
                dMCard.AddCard();
            }
            else
            {
                GameObject cardHeadObject = Instantiate(cardHeadPrefab, contentView);
                cardHeadObject.GetComponent<DMCardPrefabNoTT>().SetupCardHead(card, isInventory, this);
                dMCards.Add(cardHeadObject.GetComponent<DMCardPrefabNoTT>());
            }
        }
    }

    public void AddCardToView(Card card, bool isInventory)
    {
        GameObject cardHeadObject = Instantiate(cardHeadPrefab, contentView);
        cardHeadObject.GetComponent<DMCardPrefabNoTT>().SetupCardHead(card, isInventory, this);
    }

    public void RemoveCardFromView(Transform cardTransform)
    {
        Destroy(cardTransform.gameObject);
    }
}
