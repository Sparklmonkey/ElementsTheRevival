using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

    private int _electrumAmount;
    private List<string> _cardRewardList;
    private string _codeName;
    public async void RedeemCode(TMP_InputField input)
    {
        _codeName = input.text;
        var response = await ApiManager.Instance.GetCodeDetails(_codeName);
        CodeRedepmtionHandler(new(response));
    }

    private async Task AcceptCodeRewards()
    {
        PlayerData.Shared.Electrum += _electrumAmount;
        var invent = PlayerData.Shared.GetInventory();
        invent.AddRange(_cardRewardList);
        PlayerData.Shared.SetInventory(invent);
        await ApiManager.Instance.RedeemCode(_codeName);
        GetComponent<DashboardPlayerData>().UpdateDashboard();
        errorMessage.text = "Code Successfully Redeemed!";
    }

    private async void CodeRedepmtionHandler(CodeDetails response)
    {
        if (response.CodeName == "Invalid Code")
        {
            errorMessage.text = "Code is invalid! Please try again.";
            return;
        }
        if (response.CodeName == "Already Redeemed")
        {
            errorMessage.text = "Code has already been redeemed!";
            return;
        }

        if (response.ElectrumRewards > 0)
        {
            _electrumAmount = response.ElectrumRewards;
            electrumRewardDisplay.SetActive(true);
            electrumRewardAmount.text = response.ElectrumRewards.ToString();
        }

        if (response.CardRewards != null)
        {
            _cardRewardList = response.CardRewards.Split(" ").ToList();
            if (_cardRewardList.Count > 0 && _cardRewardList.First() != "")
            {
                var cards = CardDatabase.Instance.GetCardListWithID(_cardRewardList);
                SetupCardRewardView(cards);
                if (response.IsCardSelect)
                {
                    chooseCardButton.gameObject.SetActive(true);
                    cardRewardLabel.text = "Choose A Card:";
                    return;
                }
                else
                {
                    cardRewardLabel.text = "Cards Gained:";
                    
                }
            }
        }

        await AcceptCodeRewards();
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
        _cardRewardList = new() { cardDisplayDetail.card.Id };

        await AcceptCodeRewards();
        await ApiManager.Instance.SaveGameData();
        electrumRewardDisplay.SetActive(false);
        cardRewardObject.SetActive(false);
        cardDisplayDetail.gameObject.SetActive(false);
        redeemObject.SetActive(false);
        chooseCardButton.SetActive(false);
    }

    public void HideRedeem()
    {
        errorMessage.text = "";
        electrumRewardDisplay.SetActive(false);
        cardRewardObject.SetActive(false);
        cardDisplayDetail.gameObject.SetActive(false);
        chooseCardButton.SetActive(false);
    }
}
