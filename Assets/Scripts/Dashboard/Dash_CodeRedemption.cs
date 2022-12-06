using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Dash_CodeRedemption : MonoBehaviour
{
    private List<string> validCodes = new List<string> { "ElementsDiscordOEtG", "Legacy_ETG" };
    [SerializeField]
    private TextMeshProUGUI errorMessage, electrumRewardAmount, cardRewardLabel;
    [SerializeField]
    private GameObject redeemObject, electrumRewardDisplay, cardRewardObject, cardContentView, redeemCardPrefab, chooseCardButton;
    [SerializeField]
    private CardDisplayDetail cardDisplayDetail;

    public void RedeemCode(TMP_InputField input)
    {

        CodeRedemptionRequest codeRedemptionRequest = new CodeRedemptionRequest();

        codeRedemptionRequest.CodeValue = input.text;
        codeRedemptionRequest.PlayerSavedData = PlayerData.shared;

        StartCoroutine(ApiManager.shared.CheckRedeemCode(codeRedemptionRequest, CodeRedeemSuccessHandler, CodeRedeemFailureHandler));
    }

    private void CodeRedeemSuccessHandler(CodeRedemptionResponse codeRedemptionResponse)
    {
        if (codeRedemptionResponse.electrumReward > 0)
        {
            electrumRewardDisplay.SetActive(true);
            codeRedemptionResponse.playerSavedData.electrum += codeRedemptionResponse.electrumReward;
            electrumRewardAmount.text = codeRedemptionResponse.electrumReward.ToString();
        }

        if (codeRedemptionResponse.cardRewards.Count > 0)
        {
            if(codeRedemptionResponse.cardRewards[0] != "")
            {
                List<Card> cards = CardDatabase.GetCardListWithID(codeRedemptionResponse.cardRewards);
                cardRewardLabel.text = "Cards Gained:";

                if (codeRedemptionResponse.isCardSelection)
                {
                    chooseCardButton.gameObject.SetActive(true);
                    cardRewardLabel.text = "Choose A Card:";
                }
                else
                {
                    codeRedemptionResponse.playerSavedData.cardInventory.AddRange(codeRedemptionResponse.cardRewards);
                }
                SetupCardRewardView(cards);
            }
        }
        else
        {
            StartCoroutine(ApiManager.shared.SaveToApi(AccountSuccess, AccountFailure));
        }

        errorMessage.text = "Code Successfully Redeemed!";
        PlayerData.LoadFromApi(codeRedemptionResponse.playerSavedData);
        GetComponent<DashboardPlayerData>().UpdateDashboard();
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

    private void CodeRedeemFailureHandler(CodeRedemptionResponse codeRedemptionResponse)
    {
        errorMessage.text = "Code is either invalid or has already been redeemed.";
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
