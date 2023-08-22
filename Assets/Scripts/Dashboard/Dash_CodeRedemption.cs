using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dash_CodeRedemption : MonoBehaviour
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

        codeRedemptionRequest.CodeValue = input.text;
        codeRedemptionRequest.PlayerSavedData = PlayerData.shared;

        await ApiManager.shared.CheckCodeRedemption(input.text, CodeRedepmtionHandler);
    }

    private async void CodeRedepmtionHandler(CodeRedemptionResponse response)
    {
        if (!response.canRedeem)
        {
            errorMessage.text = response.errorMessage;
            return;
        }

        if(response.electrumReward > 0)
        {
            electrumRewardDisplay.SetActive(true);
            PlayerData.shared.electrum += response.electrumReward;
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
                    PlayerData.shared.cardInventory.AddRange(response.cardRewards);
                }
                SetupCardRewardView(cards);
            }
        }
        await ApiManager.shared.SaveDataToUnity();
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

    public void ChooseCard()
    {
        if (ApiManager.isTrainer)
        {
            return;
        }
        PlayerData.shared.cardInventory.Add(cardDisplayDetail.card.iD);

        StartCoroutine(ApiManager.shared.SaveToApi(AccountSuccess, AccountFailure));
        electrumRewardDisplay.SetActive(false);
        cardRewardObject.SetActive(false);
        cardDisplayDetail.gameObject.SetActive(false);
        redeemObject.SetActive(false);
        chooseCardButton.SetActive(false);
    }


    private static void AccountSuccess(AccountResponse accountResponse)
    {
        //Maybe Do Something??
    }
    private static void AccountFailure(AccountResponse accountResponse)
    {
        // Maybe Do Something??
    }

    public void HideRedeem()
    {
        electrumRewardDisplay.SetActive(false);
        cardRewardObject.SetActive(false);
        cardDisplayDetail.gameObject.SetActive(false);
        chooseCardButton.SetActive(false);
    }
}
