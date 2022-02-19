using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oracle_FalseGodResult : MonoBehaviour
{
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

    public string GetNextFalseGod() => falseGodNameList[Random.Range(0, falseGodNameList.Count)];
}
