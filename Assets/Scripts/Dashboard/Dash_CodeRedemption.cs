using System.Collections.Generic;
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

        CodeRedemptionRequest codeRedemptionRequest = new CodeRedemptionRequest();

        codeRedemptionRequest.codeValue = input.text;
        codeRedemptionRequest.playerSavedData = PlayerData.Shared;

        await ApiManager.Instance.CheckCodeRedemption(input.text, CodeRedepmtionHandler);
    }

    private async void CodeRedepmtionHandler(CodeRedemptionResponse response)
    {
        if (!response.canRedeem)
        {
            errorMessage.text = response.errorMessage;
            return;
        }

        if (response.electrumReward > 0)
        {
            electrumRewardDisplay.SetActive(true);
            PlayerData.Shared.electrum += response.electrumReward;
            electrumRewardAmount.text = response.electrumReward.ToString();
        }

        if (response.cardRewards.Count > 0)
        {
            if (response.cardRewards[0] != "")
            {
                List<Card> cards = CardDatabase.Instance.GetCardListWithID(response.cardRewards);
                cardRewardLabel.text = "Cards Gained:";

                if (response.isCardSelection)
                {
                    chooseCardButton.gameObject.SetActive(true);
                    cardRewardLabel.text = "Choose A Card:";
                }
                else
                {
                    PlayerData.Shared.cardInventory.AddRange(response.cardRewards);
                }
                SetupCardRewardView(cards);
            }
        }
        await ApiManager.Instance.SaveDataToUnity();
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
        PlayerData.Shared.cardInventory.Add(cardDisplayDetail.card.iD);

        await ApiManager.Instance.SaveDataToUnity();
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
