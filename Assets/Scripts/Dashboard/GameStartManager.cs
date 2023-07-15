using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartManager : MonoBehaviour
{
    [SerializeField]
    private Error_Animated errorMessageManager;

    private List<string> elderPrefix = new List<string> { "Aeth", "Air", "Shad", "Lum", "Mor", "Ter", "Dis", "Chr", "Pyr", "Mas", "Vit", "Aqua" };
    private List<string> elderSuffix = new List<string> { "eric", "es", "ow", "iel", "tis", "ra", "cord", "onos", "ofuze", "sa", "al", "rius" };

    public void StartGameOnDificulty(int level)
    {

        BattleVars.shared.ResetBattleVars();
        EnemyAi ai;
        switch (level)
        {
            case 0:
                ai = Resources.Load<EnemyAi>("EnemyAi/Level0/Random");
                ai.mark = (Element)Random.Range(0, 12);
                ai.deck = string.Join(" ", CardDatabase.Instance.GetRandomDeck());
                break;
            case 1:
                ai = Resources.Load<EnemyAi>("EnemyAi/Level1/Random");
                ai.mark = (Element)Random.Range(0, 12);
                ai.deck = string.Join(" ", CardDatabase.Instance.GetRandomDeck());
                break;
            case 2:
                Element elementDeck = (Element)Random.Range(0, 12);
                if (elementDeck == Element.Entropy)
                {
                    elementDeck = Element.Aether;
                }
                ai = Resources.Load<EnemyAi>($"EnemyAi/Level2/{elementDeck.FastElementString()}");
                break;
            case 3:
                elementDeck = (Element)Random.Range(0, 12);
                if(elementDeck == Element.Entropy)
                {
                    elementDeck = Element.Aether;
                }
                ai = Resources.Load<EnemyAi>($"EnemyAi/Level3/{elementDeck.FastElementString()}");
                break;
            case 4:
                ai = Instantiate(Resources.Load<EnemyAi>("EnemyAi/Level4/Random"));
                BattleVars.shared.primaryElement = (Element)Random.Range(0, 12);
                BattleVars.shared.secondaryElement = (Element)Random.Range(0, 12);
                string aiName = $"{elderPrefix[(int)BattleVars.shared.primaryElement]}{elderSuffix[(int)BattleVars.shared.secondaryElement]}";
                ai.mark = BattleVars.shared.secondaryElement;
                ai.opponentName = aiName;
                ai.deck = string.Join(" ", CardDatabase.Instance.GetHalfBloodDeck(BattleVars.shared.primaryElement, BattleVars.shared.secondaryElement).SerializeCard());
                break;
            case 5:
                string falseGod = PlayerData.shared.nextFalseGod;
                if (falseGod == "")
                {
                    falseGod = falseGodNameList[Random.Range(0, falseGodNameList.Count)];
                }
                //ai = Resources.Load<EnemyAi>($@"EnemyAi/Level5/Serket");
                ai = Resources.Load<EnemyAi>($@"EnemyAi/Level5/{falseGod}");
                PlayerData.shared.nextFalseGod = "";
                break;
            default:
                elementDeck = (Element)Random.Range(0, 12);
                if (elementDeck == Element.Entropy)
                {
                    elementDeck = Element.Aether;
                }
                ai = Resources.Load<EnemyAi>($"EnemyAi/Level3/{elementDeck.FastElementString()}");
                break;
        }

        if(PlayerData.shared.electrum < ai.costToPlay)
        {
            errorMessageManager.DisplayAnimatedError("Insufficient Electrum");
        }
        else
        {
            PlayerData.shared.electrum -= ai.costToPlay;
            BattleVars.shared.enemyAiData = ai;
            SceneManager.LoadScene("Battlefield");
        }
    }
    private List<string> falseGodNameList = new List<string> { "Divine Glory",
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
                                                                "Jezebel"};

}
