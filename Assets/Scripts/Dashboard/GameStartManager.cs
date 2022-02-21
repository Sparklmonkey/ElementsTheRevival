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
        EnemyAi ai = new EnemyAi();
        switch (level)
        {
            case 0:
                ai = Resources.Load<EnemyAi>("EnemyAi/Level0/Random");
                ai.mark = (Element)Random.Range(0, 12);
                ai.deck = CardDatabase.GetRandomDeck();
                break;
            case 1:
                ai = Resources.Load<EnemyAi>("EnemyAi/Level1/Random");
                ai.mark = (Element)Random.Range(0, 12);
                ai.deck = CardDatabase.GetRandomDeck();
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
                ai = Resources.Load<EnemyAi>("EnemyAi/Level4/Random");
                Element primaryElement = (Element)Random.Range(0, 12);
                Element secondaryElement = (Element)Random.Range(0, 12);
                string aiName = $"{elderPrefix[(int)primaryElement]}{elderSuffix[(int)secondaryElement]}";
                ai.mark = secondaryElement;
                ai.opponentName = aiName;
                ai.deck = CardDatabase.GetHalfBloodDeck(primaryElement, secondaryElement);
                break;
            case 5:
                string falseGod = PlayerData.shared.nextFalseGod;
                if(falseGod == "")
                {
                    falseGod = falseGodNameList[Random.Range(0, falseGodNameList.Count)];
                }
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
            errorMessageManager.DisplayAnimatedError("Insufficient Electrum Message");
        }
        else
        {
            PlayerData.shared.electrum -= ai.costToPlay;
            BattleVars.shared.enemyAiData = ai;
            SceneManager.LoadScene("Battlefield");
        }
    }
    private List<string> falseGodNameList = new List<string> { "Akebono",
                                                                "Chaos Lord",
                                                                "Dark Matter",
                                                                "Decay",
                                                                "Destiny",
                                                                "Divine Glory",
                                                                "Dream Catcher",
                                                                "Elidnis",
                                                                "Eternal Phoenix",
                                                                "Ferox",
                                                                "Fire Queen",
                                                                "Gemini",
                                                                "Graviton",
                                                                "Hecate",
                                                                "Hermes",
                                                                "Incarnate",
                                                                "Jezebel",
                                                                "Lionheart",
                                                                "Miracle",
                                                                "Morte",
                                                                "Neptune",
                                                                "Obliterator",
                                                                "Octane",
                                                                "Osiris",
                                                                "Paradox",
                                                                "Rainbow",
                                                                "Scorpio",
                                                                "Seism",
                                                                "Serket"};
}
