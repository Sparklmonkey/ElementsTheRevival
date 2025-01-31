using System.Collections.Generic;
using UnityEngine;

public class GameStartManager : MonoBehaviour
{
    [SerializeField]
    private ErrorAnimated errorMessageManager;

    [SerializeField] private GameObject popUpModal, popUpObject;
    [SerializeField] private Transform mainPanel;
    private readonly List<string> _elderPrefix = new() { "Aeth", "Ari", "Shad", "Lum", "Mor", "Ter", "Dis", "Chr", "Pyr", "Mas", "Vit", "Aqua" };
    private readonly List<string> _elderSuffix = new() { "eric", "es", "ow", "iel", "tis", "ra", "cord", "onos", "ofuze", "sa", "al", "rius" };

    public void StartGameOnDificulty(int level)
    {

        BattleVars.Shared.ResetBattleVars();
        EnemyAi ai;
        switch (level)
        {
            case 0:
                ai = Resources.Load<EnemyAi>("EnemyAi/Level0/Random");
                ai.mark = (Element)Random.Range(0, 12);
                ai.deck = string.Join(" ", CardDatabase.Instance.GetRandomDeck());
                ai.opponentName = ai.mark.FastElementString();
                break;
            case 1:
                ai = Resources.Load<EnemyAi>("EnemyAi/Level1/Random");
                ai.mark = (Element)Random.Range(0, 12);
                ai.deck = string.Join(" ", CardDatabase.Instance.GetRandomDeck());
                ai.opponentName = ai.mark.FastElementString();
                break;
            case 2:
                var elementDeck = (Element)Random.Range(0, 12);
                ai = Resources.Load<EnemyAi>($"EnemyAi/Level2/{elementDeck.FastElementString()}");
                break;
            case 3:
                elementDeck = (Element)Random.Range(0, 12);
                if (elementDeck == Element.Entropy)
                {
                    elementDeck = Element.Aether;
                }

                ai = Resources.Load<EnemyAi>($"EnemyAi/Level3/{elementDeck.FastElementString()}");
                break;
            case 4:
                ai = Instantiate(Resources.Load<EnemyAi>("EnemyAi/Level4/Random"));
                BattleVars.Shared.PrimaryElement = (Element)Random.Range(0, 12);
                BattleVars.Shared.SecondaryElement = (Element)Random.Range(0, 12);
                var aiName = $"{_elderPrefix[(int)BattleVars.Shared.PrimaryElement]}{_elderSuffix[(int)BattleVars.Shared.SecondaryElement]}";
                ai.mark = BattleVars.Shared.SecondaryElement;
                ai.opponentName = aiName;
                ai.deck = string.Join(" ", CardDatabase.Instance.GetHalfBloodDeck(BattleVars.Shared.PrimaryElement, BattleVars.Shared.SecondaryElement).SerializeCard());
                break;
            case 5:
                var falseGod = PlayerData.Shared.nextFalseGod;
                if (falseGod == "")
                {
                    falseGod = _falseGodNameList[Random.Range(0, _falseGodNameList.Count)];
                }
                ai = Resources.Load<EnemyAi>($@"EnemyAi/Level5/{falseGod}");
                PlayerData.Shared.nextFalseGod = "";
                break;
            case 6:
                if (!RemoteConfigHelper.Instance.IsFeatureEnabled(FeatureType.PvpOne))
                {
                    popUpObject = Instantiate(popUpModal, mainPanel);
                    popUpObject.GetComponent<PopUpModal>().SetupModal("Dashboard", "RemoteConfigFeatureNotEnabledTitle", 
                        "RemoteConfigFeatureNotEnabledButtonTitle", DismissPopUp);
                    return;
                }
                ai = Resources.Load<EnemyAi>($@"EnemyAi/Level5/Serket");
                PlayerData.Shared.nextFalseGod = "";
                break;
            case 7:
                if (!RemoteConfigHelper.Instance.IsFeatureEnabled(FeatureType.PvpTwo))
                {
                    popUpObject = Instantiate(popUpModal, mainPanel);
                    popUpObject.GetComponent<PopUpModal>().SetupModal("Dashboard", "RemoteConfigFeatureNotEnabledTitle", 
                        "RemoteConfigFeatureNotEnabledButtonTitle", DismissPopUp);
                    return;
                }
                ai = Resources.Load<EnemyAi>($@"EnemyAi/Level5/Serket");
                PlayerData.Shared.nextFalseGod = "";
                break;
            case 8:
                if (!RemoteConfigHelper.Instance.IsFeatureEnabled(FeatureType.PvpDuel))
                {
                    popUpObject = Instantiate(popUpModal, mainPanel);
                    popUpObject.GetComponent<PopUpModal>().SetupModal("Dashboard", "RemoteConfigFeatureNotEnabledTitle", 
                        "RemoteConfigFeatureNotEnabledButtonTitle", DismissPopUp);
                    return;
                }
                ai = Resources.Load<EnemyAi>($@"EnemyAi/Level5/Serket");
                PlayerData.Shared.nextFalseGod = "";
                break;
            case 9:
                if (!RemoteConfigHelper.Instance.IsFeatureEnabled(FeatureType.Arena))
                {
                    popUpObject = Instantiate(popUpModal, mainPanel);
                    popUpObject.GetComponent<PopUpModal>().SetupModal("Dashboard", "RemoteConfigFeatureNotEnabledTitle", 
                        "RemoteConfigFeatureNotEnabledButtonTitle", DismissPopUp);
                    return;
                }
                ai = Resources.Load<EnemyAi>($@"EnemyAi/Level5/Serket");
                PlayerData.Shared.nextFalseGod = "";
                break;
            case 10:
                if (!RemoteConfigHelper.Instance.IsFeatureEnabled(FeatureType.T50Arena))
                {
                    popUpObject = Instantiate(popUpModal, mainPanel);
                    popUpObject.GetComponent<PopUpModal>().SetupModal("Dashboard", "RemoteConfigFeatureNotEnabledTitle", 
                        "RemoteConfigFeatureNotEnabledButtonTitle", DismissPopUp);
                    return;
                }
                SceneTransitionManager.Instance.LoadScene("Top50");
                return;
            default:
                elementDeck = (Element)Random.Range(0, 12);
                if (elementDeck == Element.Entropy)
                {
                    elementDeck = Element.Aether;
                }
                ai = Resources.Load<EnemyAi>($"EnemyAi/Level3/{elementDeck.FastElementString()}");
                break;
        }

        if (PlayerData.Shared.electrum < ai.costToPlay)
        {
            errorMessageManager.DisplayAnimatedError("Insufficient Electrum");
        }
        else
        {
            PlayerData.Shared.electrum -= ai.costToPlay;
            BattleVars.Shared.EnemyAiData = ai;
            SceneTransitionManager.Instance.LoadScene("Battlefield");
        }
    }

    private void DismissPopUp()
    {
        Destroy(popUpObject);
    }
    
    private readonly List<string> _falseGodNameList = new()
    {
        "Divine Glory",
        "Serket",
        "Morte",
        "Lionheart",
        "Incarnate",
        "Fire Queen",
        "Seism",
        "Miracle",
        "Graviton",
        "Paradox",
        "Akebono",
        "Neptune",
        "Scorpio",
        "Osiris",
        "Octane",
        "Rainbow",
        "Obliterator",
        "Gemini",
        "Chaos Lord",
        "Dark Matter",
        "Decay",
        "Destiny",
        "Dream Catcher",
        "Elidnis",
        "Eternal Phoenix",
        "Ferox",
        "Gemini",
        "Hecate",
        "Hermes",
        "Jezebel"
    };

}
