using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DashQuestManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI questTitle, questDescription, questObjective, questReward, questCompleteTitle, questCompleteDesc, questCompleteObjective, questCompleteReward, selectionDesc;
    [SerializeField]
    private Button claimRewardButton;
    [SerializeField]
    private DashboardPlayerData dashboardPlayerData;
    [SerializeField]
    private GameObject selectionPanel, rareCardPrefab, questStartObject, questCompleteObject, cardUpgradeObject;
    [SerializeField]
    private Transform panelGrid;
    [SerializeField]
    private CardDisplay cardDisplay;
    [SerializeField]
    private List<RareCardObject> weaponObjects;
    private string _questCompleteDescription = "Great Job! \n Click the reward button to get your reward and move to your next quest.";
    // Start is called before the first frame update
    private void Start()
    {

    }

    public void SetupQuestPanel()
    {
        gameObject.SetActive(true);
        claimRewardButton.onClick.RemoveAllListeners();
        selectionDesc.gameObject.SetActive(false);
        selectionPanel.SetActive(false);
        questStartObject.SetActive(false);
        questCompleteObject.SetActive(false);
        if (PlayerData.Shared.CurrentQuestIndex >= _quests.Count && PlayerPrefs.GetFloat("ShouldShowRareCard") != 1)
        {
            cardUpgradeObject.SetActive(true);
            return;
        }

        if (PlayerPrefs.GetFloat("ShouldShowRareCard") == 1)
        {
            selectionPanel.SetActive(true);
            questStartObject.SetActive(true);
            questTitle.text = "Quest 7 : Rare cards";
            selectionDesc.gameObject.SetActive(true);
            questDescription.text = "";
            questObjective.text = "";
            questReward.text = "";
            claimRewardButton.gameObject.SetActive(false);
            for (var i = 0; i < CardDatabase.Instance.RareWeaponRewards.Count; i++)
            {
                weaponObjects[i].SetupSelection(CardDatabase.Instance.GetCardFromId(CardDatabase.Instance.RareWeaponRewards[i]), this, cardDisplay);
            }
            return;
        }

        var questToDisplay = _quests[PlayerData.Shared.CurrentQuestIndex];

        if (questToDisplay.IsComplete)
        {
            questCompleteObject.SetActive(true);
            questCompleteTitle.text = questToDisplay.QuestTitle;
            questCompleteDesc.text = _questCompleteDescription;
            questCompleteObjective.text = "";
            questCompleteReward.text = questToDisplay.QuestReward;
            claimRewardButton.onClick.RemoveAllListeners();
            claimRewardButton.onClick.AddListener(delegate { questToDisplay.RewardPlayer(); });
            claimRewardButton.onClick.AddListener(delegate { AddNewQuest(); });
        }
        else
        {
            questStartObject.SetActive(true);
            questTitle.text = questToDisplay.QuestTitle;
            questDescription.text = questToDisplay.QuestDescription;
            questObjective.text = questToDisplay.QuestObjective;
            questReward.text = questToDisplay.QuestReward;
        }
    }

    public void AddNewQuest()
    {
        PlayerData.Shared.CurrentQuestIndex++;
        dashboardPlayerData.UpdateDashboard();
        SetupQuestPanel();
        PlayerData.SaveData();
    }

    public void MoveToCardUpgradeScene()
    {
        gameObject.SetActive(false);
        SceneTransitionManager.Instance.LoadScene("CardUpgrade");
    }

    private static List<IQuest> _quests = new() { new WelcomeQuest(), new ImproveDeckQuest(), new BazaarQuest(), new ElementalOneQuest(), new ElementalTwoQuest(), new ScoreOneQuest(), new ScoreTwoQuest() };
}
