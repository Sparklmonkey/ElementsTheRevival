using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dash_QuestManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI questTitle, questDescription, questObjective, questReward, selectionDesc;
    [SerializeField]
    private Button claimRewardButton;
    [SerializeField]
    private DashboardPlayerData dashboardPlayerData;
    [SerializeField]
    private GameObject selectionPanel, rareCardPrefab;
    [SerializeField]
    private Transform panelGrid;
    [SerializeField]
    private CardDisplay cardDisplay;
    [SerializeField]
    private List<Card> rareCardsList;
    private string questCompleteDescription = "Great Job! \n Click the reward button to get your reward and move to your next quest.";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetupQuestPanel()
    {
        gameObject.SetActive(true);
        claimRewardButton.onClick.RemoveAllListeners();
        claimRewardButton.gameObject.SetActive(false);
        selectionDesc.gameObject.SetActive(false);
        selectionPanel.SetActive(false);
        if (PlayerData.shared.currentQuestIndex >= quests.Count && PlayerPrefs.GetFloat("ShouldShowRareCard") != 1)
        {
            questTitle.text = "Card Upgrades";
            questDescription.text = "You completed all the available quests, but there are ways to become even stronger, if you are rich enough ... or brave enough. You can create <b>upgraded cards</b>; upgrading a card is going to cost you 1,500 electrum coins. It is very expensive, but if you have plenty of money it does not matter, right? \n There is something else you should know about; something else beside elementals dwells in the universe of elements. There are entities that claim to be <b>invincible, gods</b>, but maybe you can prove them wrong. Do not even think this will be anywhere close to easy; the normal laws of elements do not apply to these guys.";
            claimRewardButton.GetComponentInChildren<TextMeshProUGUI>().text = "Upgrade Cards";
            claimRewardButton.onClick.AddListener(delegate { MoveToCardUpgradeScene(); });
            claimRewardButton.gameObject.SetActive(true);
            questObjective.text = "";
            questReward.text = "";
            return;
        }

        if(PlayerPrefs.GetFloat("ShouldShowRareCard") == 1)
        {
            selectionPanel.SetActive(true);
            questTitle.text = "Quest 7 : Rare cards";
            selectionDesc.gameObject.SetActive(true);
            questDescription.text = "";
            questObjective.text = "";
            questReward.text = "";
            claimRewardButton.gameObject.SetActive(false);
            foreach (Card item in rareCardsList)
            {
                GameObject rareCardObject = Instantiate(rareCardPrefab, panelGrid);
                rareCardObject.GetComponent<RareCardObject>().SetupSelection(item, this, cardDisplay);
            }
            return;
        }

        IQuest questToDisplay = quests[PlayerData.shared.currentQuestIndex];

        if (questToDisplay.IsComplete)
        {
            questTitle.text = questToDisplay.QuestTitle;
            questDescription.text = questCompleteDescription;
            questObjective.text = "";
            questReward.text = questToDisplay.QuestReward;
            claimRewardButton.onClick.AddListener(delegate { questToDisplay.RewardPlayer(); });
            claimRewardButton.onClick.AddListener(delegate { AddNewQuest(); });
            claimRewardButton.gameObject.SetActive(true);
        }
        else
        {
            questTitle.text = questToDisplay.QuestTitle;
            questDescription.text = questToDisplay.QuestDescription;
            questObjective.text = questToDisplay.QuestObjective;
            questReward.text = questToDisplay.QuestReward;
            claimRewardButton.onClick.RemoveAllListeners();
            claimRewardButton.gameObject.SetActive(false);
        }
    }

    public void AddNewQuest()
    {
        PlayerData.shared.currentQuestIndex++;
        dashboardPlayerData.UpdateDashboard();
        SetupQuestPanel();
        PlayerData.SaveData();
    }

    public void MoveToCardUpgradeScene()
    {
        gameObject.SetActive(false);
        dashboardPlayerData.gameObject.GetComponent<DashboardSceneManager>().LoadNewScene("CardUpgrade");
    }

    static private List<IQuest> quests = new List<IQuest> { new WelcomeQuest(), new ImproveDeckQuest(), new BazaarQuest(), new ElementalOneQuest(), new ElementalTwoQuest(), new ScoreOneQuest(), new ScoreTwoQuest() };
}
