using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    private Transform contentView;
    [SerializeField]
    private BazaarCardDisplayManager cardDisplayManager;
    [SerializeField]
    private GameObject cardHeadPrefab;
    private List<DmCardPrefabNoTt> _dMCards = new();

    private bool _isInventory;
    public void SetupContentView(List<Card> cardList, bool isInventory)
    {
        this._isInventory = isInventory;
        ClearContentView();
        if (isInventory)
        {
            UpdateCardView();
        }
        else
        {
            foreach (var card in cardList)
            {
                var cardHeadObject = Instantiate(cardHeadPrefab, contentView);
                cardHeadObject.GetComponent<DmCardPrefabNoTt>().SetupCardHead(card, isInventory, this);
            }
        }
    }

    public void ShowCardDisplay(Card card)
    {
        cardDisplayManager.gameObject.SetActive(true);
        cardDisplayManager.SetupCardDisplay(card, !_isInventory);
    }

    public void HideCardDisplay()
    {
        cardDisplayManager.gameObject.SetActive(false);
    }

    public void ChangeCardOwner(DmCardPrefabNoTt cardObject)
    {
        var card = cardObject.GetCard();
        if (_isInventory)
        {
            GetComponent<BazaarPlayerDataManager>().ModifyPlayerCardInventory(card, false);
        }
        else if (GetComponent<BazaarPlayerDataManager>().CanBuyCard(card.BuyPrice))
        {
            GetComponent<BazaarPlayerDataManager>().ModifyPlayerCardInventory(card, true);
        }

        GetComponent<BazaarPlayerInventoryManager>().UpdateCardView();
    }

    public void ClearContentView()
    {
        var children = new List<DmCardPrefabNoTt>(contentView.GetComponentsInChildren<DmCardPrefabNoTt>());
        foreach (var child in children)
        {
            Destroy(child.gameObject);
        }
    }

    public void UpdateCardView()
    {
        _dMCards = new List<DmCardPrefabNoTt>();
        ClearContentView();
        var cardList = PlayerData.Shared.inventoryCards.DeserializeCard();
        cardList.Sort((x, y) => string.Compare(x.Id, y.Id));
        foreach (var card in cardList)
        {
            var dMCard = _dMCards.Find(x => x.GetCard().Id == card.Id);
            if (dMCard is not null)
            {
                dMCard.AddCard();
            }
            else
            {
                var cardHeadObject = Instantiate(cardHeadPrefab, contentView);
                cardHeadObject.GetComponent<DmCardPrefabNoTt>().SetupCardHead(card, _isInventory, this);
                _dMCards.Add(cardHeadObject.GetComponent<DmCardPrefabNoTt>());
            }
        }
    }

    public void AddCardToView(Card card, bool isInventory)
    {
        var cardHeadObject = Instantiate(cardHeadPrefab, contentView);
        cardHeadObject.GetComponent<DmCardPrefabNoTt>().SetupCardHead(card, isInventory, this);
    }

    public void RemoveCardFromView(Transform cardTransform)
    {
        Destroy(cardTransform.gameObject);
    }
}
