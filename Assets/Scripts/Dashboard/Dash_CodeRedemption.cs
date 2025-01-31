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
        var response = await ApiManager.Instance.GetCodeDetails(input.text);
        CodeRedepmtionHandler(new(response));
    }

    private async void CodeRedepmtionHandler(CodeDetails response)
    {
        if (response.CodeName == "NotValid")
        {
            errorMessage.text = "Code has already been used, or is invalid.";
            return;
        }

        if (response.ElectrumRewards > 0)
        {
            electrumRewardDisplay.SetActive(true);
            PlayerData.Shared.electrum += response.ElectrumRewards;
            electrumRewardAmount.text = response.ElectrumRewards.ToString();
        }
        Debug.Log(response.CardRewards);
        var cardList = response.CardRewards.Split(" ").ToList();
        
        
        if (cardList.Count > 0)
        {
            if (cardList[0] != "")
            {
                var cards = CardDatabase.Instance.GetCardListWithID(cardList);
                cardRewardLabel.text = "Cards Gained:";

                if (response.IsCardSelect)
                {
                    chooseCardButton.gameObject.SetActive(true);
                    cardRewardLabel.text = "Choose A Card:";
                }
                else
                {
                    var invent = PlayerData.Shared.GetInventory();
                    invent.AddRange(cardList);
                    PlayerData.Shared.SetInventory(invent);
                }
                SetupCardRewardView(cards);
            }
        }
        else
        {
            await ApiManager.Instance.RedeemCode(response.CodeName);
        }
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
            var redeemCardObject = Instantiate(redeemCardPrefab, cardContentView.transform);
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
        if (ApiManager.IsTrainer) return;
        var invent = PlayerData.Shared.GetInventory();
        invent.Add(cardDisplayDetail.card.Id);
        PlayerData.Shared.SetInventory(invent);
        
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
