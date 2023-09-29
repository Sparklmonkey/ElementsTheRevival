using System.Collections.Generic;
using System.Linq;
using Core.Networking.Response;
using Networking;
using TMPro;
using UnityEngine;

public class DashCodeRedemption : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI errorMessage, electrumRewardAmount, cardRewardLabel;
    [SerializeField]
    private GameObject redeemObject, electrumRewardDisplay, cardRewardObject, cardContentView, redeemCardPrefab, chooseCardButton;
    [SerializeField]
    private CardDisplayDetail cardDisplayDetail;

    public async void RedeemCode(TMP_InputField input)
    {
        var response = await ApiManager.Instance.CheckCodeRedemption(input.text);
        CodeRedepmtionHandler(new(response));
    }

    private async void CodeRedepmtionHandler(CodeDetails response)
    {
        if (response.codeName == "NotValid")
        {
            errorMessage.text = "Code has already been used, or is invalid.";
            return;
        }

        if (response.electrumRewards > 0)
        {
            electrumRewardDisplay.SetActive(true);
            PlayerData.Shared.electrum += response.electrumRewards;
            electrumRewardAmount.text = response.electrumRewards.ToString();
        }

        var cardList = response.cardRewards.Split(" ").ToList();
        
        
        if (cardList.Count > 0)
        {
            if (cardList[0] != "")
            {
                List<Card> cards = CardDatabase.Instance.GetCardListWithID(cardList);
                cardRewardLabel.text = "Cards Gained:";

                if (response.isCardSelect)
                {
                    chooseCardButton.gameObject.SetActive(true);
                    cardRewardLabel.text = "Choose A Card:";
                }
                else
                {
                    PlayerData.Shared.inventoryCards.AddRange(cardList);
                }
                SetupCardRewardView(cards);
            }
        }
        await ApiManager.Instance.SaveGameData();
        GetComponent<DashboardPlayerData>().UpdateDashboard();
        errorMessage.text = "Code Successfully Redeemed!";
    }

    private void SetupCardRewardView(List<Card> cardObjects)
    {
        foreach (Transform item in cardContentView.transform)
        {
            Destroy(item.gameObject);
        }
        cardRewardObject.SetActive(true);
        foreach (var card in cardObjects)
        {
            GameObject redeemCardObject = Instantiate(redeemCardPrefab, cardContentView.transform);
            redeemCardObject.GetComponent<RedeemCardObject>().SetupObject(card, this);
        }
    }

    public void DisplayCardDetail(Card card)
    {
        cardDisplayDetail.gameObject.SetActive(true);
        cardDisplayDetail.SetupCardView(card, true, false);
    }

    public async void ChooseCard()
    {
        if (ApiManager.IsTrainer)
        {
            return;
        }
        PlayerData.Shared.inventoryCards.Add(cardDisplayDetail.card.iD);

        await ApiManager.Instance.SaveGameData();
        electrumRewardDisplay.SetActive(false);
        cardRewardObject.SetActive(false);
        cardDisplayDetail.gameObject.SetActive(false);
        redeemObject.SetActive(false);
        chooseCardButton.SetActive(false);
    }

    public void HideRedeem()
    {
        electrumRewardDisplay.SetActive(false);
        cardRewardObject.SetActive(false);
        cardDisplayDetail.gameObject.SetActive(false);
        chooseCardButton.SetActive(false);
    }
}
