using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;

public class CardTest : MonoBehaviour
{
    [SerializeField]
    private List<Card> cardList;
    // Start is called before the first frame update
    void Start()
    {
        _ = StartCoroutine(DisplayCards());
    }

    private IEnumerator DisplayCards()
    {
        TextAsset jsonString = Resources.Load<TextAsset>("Cards/CardDatabase");
        CardDB cardDBNew = JsonUtility.FromJson<CardDB>(jsonString.text);

        foreach (var item in cardDBNew.cardDb)
        {
            Card cardy = cardList.Find(x => x.iD == item.iD);
            if (cardy == null) { continue; }
            item.imageID = cardy.imageID;
        }

        Debug.Log(JsonUtility.ToJson(cardDBNew));
        yield break;
    }
    public List<string> innateSkills = new List<string> { "airborne", "chimera", "devourer", "mummy", "obsession", "poisonous", "ranged", "salvaging", "swarm", "undead", 
                                                        "voodoo", "singularity", "immaterial", "integrity", "mutant", "burrow" };

    public List<string> passiveSkills = new List<string> { "vampire", "neurotoxin", "psion", "gravity pull", "phoenix", "infest", "momentum", "fire", 
                                                        "light", "air", "earth", "overdrive", "acceleration", "deadly venom", "venom", "scavenger" };

    public string activeSkills = "ablaze, black hole, luciferin, dead / alive, lycanthropy, deja vu, hatch, evolve, unstable gas, dive, infect, gravity pull, growth, plague, poison, stone form" +
        "precognition, photosynthesis, steam, queen, divineshield, burrow, rebirth, silence, sacrifice, patience, serendipity, bravery, nova, pandemonium, gravity pull, miracle, thunderstorm" +
        "flying, stoneskin, healp, mitosis, rain of fire, blitz, supernova, deadly poison, shard";

    public List<string> skillsNoTarget = new List<string> { "scarab", "ablaze", "black hole", "luciferin", "dead / alive", "lycanthropy", "deja vu", "hatch", "evolve", "unstable gas", "dive", "infect",
        "gravity pull", "growth", "plague", "poison", "stone form", "precognition", "photosynthesis", "steam", "queen", "divineshield", "burrow", "rebirth", "silence", "sacrifice", "patience",
        "serendipity", "bravery", "nova", "pandemonium", "gravity pull", "miracle", "thunderstorm", "flying", "stoneskin", "healp", "mitosis", "rain of fire", "blitz", "supernova", "deadly poison",
        "shard"};
 
    public List<string> skillsWithTarget = new List<string> {  "mutation", "improve", "antimatter", "web", "endow", "liquid shadow", "accretion", "devour", "immortality", "congeal", 
        "berserk", "lobotomize", "rage", "paradox", "freeze", "infection", "nymph", "heal", "petrify", "aflatoxin", "guard", "fractal", "chaos power", "overdrive", "readiness", "wisdom", "chaos",
        "butterfly", "momentum", "acceleration", "armor", "reverse time", "enchant", "earthquake", "adrenaline", "fire bolt", "destroy", "immolate", "icebolt", "purify", "holy light", "blessing", 
        "shockwave", "steal", "drain life", "nightmare", "lightning", "parallel universe","heavy armor", "cremation"}; 
}

//Artifact
//boneyard, soul catch, catapult, empathy, flood, sanctuary, ignite, hasten, nightfall, cloak, duality, eclipse, 

//Weapon
//none, dagger, sword, hammer, bow, scramble, venom, momentum, destroy, regenerate, fiery, tsunami, immaterial, sniper, reverse time, vampire, lobotomize, 

//Shield
//none, shield, edissipation, unholy, bones, weight, spines, reflect, firewall, ice, solar, hope, fog, wings, delay, dusk, phaseshift, 



//Skill



