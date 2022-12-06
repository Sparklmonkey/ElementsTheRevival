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
        hpFull.text = cardOnDisplay.def.ToString();
        atkFull.text = cardOnDisplay.atk.ToString();
        hpCurrent.text = cardOnDisplay.def.ToString();
        frozenCount.text = cardOnDisplay.Freeze.ToString();
        delayCount.text = cardOnDisplay.IsDelayed ? "1" : "0";
        this.fieldIndex.text = fieldIndex.ToString();
        atkCurrent.text = cardOnDisplay.atk.ToString();

        if (cardOnDisplay.innate?.Count > 0)
        {
            passiveTwo.text = cardOnDisplay.innate[0];
        }

        passiveOne.text = "";
        //Top Line Passive
        if(cardOnDisplay.passive?.Count > 0)
        {
            passiveOne.text = cardOnDisplay.passive[0];
        }
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
