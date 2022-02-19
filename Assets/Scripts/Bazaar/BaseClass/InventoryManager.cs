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

    private bool isInventory;
    public void SetupContentView(List<Card> cardList, bool isInventory)
    {
        this.isInventory = isInventory;
        ClearContentView();
        foreach (Card card in cardList)
        {
            GameObject cardHeadObject = Instantiate(cardHeadPrefab, contentView);
            cardHeadObject.GetComponent<DMCardPrefabNoTT>().SetupCardHead(card, isInventory, this);
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
            RemoveCardFromView(cardObject.transform);
            GetComponent<Bazaar_TransactionManager>().ChangeCoinCount(card.sellPrice, true);
            GetComponent<Bazaar_PlayerDataManager>().ModifyPlayerCardInventory(card, false);
            return;
        }
        if(GetComponent<Bazaar_PlayerDataManager>().CanBuyCard(card.buyPrice))
        {
            GetComponent<Bazaar_TransactionManager>().ChangeCoinCount(card.buyPrice, false);
            GetComponent<Bazaar_PlayerInventoryManager>().AddCardToView(card, !isInventory);
            GetComponent<Bazaar_PlayerDataManager>().ModifyPlayerCardInventory(card, true);
        }
    }

    public void ClearContentView()
    {
        List<DMCardPrefabNoTT> children = new List<DMCardPrefabNoTT>(contentView.GetComponentsInChildren<DMCardPrefabNoTT>());
        foreach (DMCardPrefabNoTT child in children)
        {
            Destroy(child.gameObject);
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
