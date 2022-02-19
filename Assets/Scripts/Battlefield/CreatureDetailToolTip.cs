using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CreatureDetailToolTip : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI atkFull, atkCurrent, hpFull, hpCurrent, frozenCount, delayCount, passiveOne, passiveTwo, fieldIndex;

    public void SetupDetailView(Card cardOnDisplay, int fieldIndex)
    {
        hpFull.text = cardOnDisplay.maxHP.ToString();
        atkFull.text = cardOnDisplay.basePower.ToString();
        hpCurrent.text = cardOnDisplay.hp.ToString();
        frozenCount.text = cardOnDisplay.cardCounters.freeze.ToString();
        delayCount.text = cardOnDisplay.cardCounters.delay.ToString();
        this.fieldIndex.text = fieldIndex.ToString();
        atkCurrent.text = cardOnDisplay.power.ToString();

        passiveTwo.text = cardOnDisplay.cardPassives.isAirborne ? "airborne" : "";

        passiveOne.text = "";
        //Top Line Passive
        passiveOne.text = cardOnDisplay.cardPassives.isPoisonous ? "poisonous" : passiveOne.text;
        passiveOne.text = cardOnDisplay.cardPassives.isVoodoo ? "voodoo" : passiveOne.text;
        passiveOne.text = cardOnDisplay.cardPassives.isMummy ? "mummy" : passiveOne.text;
        passiveOne.text = cardOnDisplay.cardPassives.isUndead ? "undead" : passiveOne.text;
        passiveOne.text = cardOnDisplay.cardPassives.isBurrowed ? "burrow" : passiveOne.text;
        passiveOne.text = cardOnDisplay.cardAbilities.onPlayAbilityScript.Contains("Chimera") ? "chimera" : passiveOne.text;
        passiveOne.text = cardOnDisplay.name.Contains("Singularity") ? "singularity" : passiveOne.text;
        passiveOne.text = cardOnDisplay.name.Contains("Scarab") ? "swarm" : passiveOne.text;
        passiveOne.text = cardOnDisplay.cardPassives.isVenemous ? "venemous" : passiveOne.text;
        passiveOne.text = cardOnDisplay.cardAbilities.endTurnAbilityScript.Contains("Devour") ? "devourer" : passiveOne.text;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
