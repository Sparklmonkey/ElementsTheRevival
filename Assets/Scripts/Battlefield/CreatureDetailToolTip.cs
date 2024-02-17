using TMPro;
using UnityEngine;

public class CreatureDetailToolTip : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI atkFull, atkCurrent, hpFull, hpCurrent, frozenCount, delayCount, passiveOne, passiveTwo, fieldIndex;

    public void SetupDetailView(Card cardOnDisplay, int fieldIndex)
    {
        hpFull.text = cardOnDisplay.Def.ToString();
        atkFull.text = cardOnDisplay.Atk.ToString();
        hpCurrent.text = cardOnDisplay.Def.ToString();
        frozenCount.text = cardOnDisplay.Counters.Freeze.ToString();
        delayCount.text = $"{cardOnDisplay.Counters.Delay}";
        this.fieldIndex.text = fieldIndex.ToString();
        atkCurrent.text = cardOnDisplay.Atk.ToString();

        //if (cardOnDisplay.innate?.Count > 0)
        //{
        //    passiveTwo.text = cardOnDisplay.innate[0];
        //}

        passiveOne.text = "";
        //Top Line Passive
        //if(cardOnDisplay.passive?.Count > 0)
        //{
        //    passiveOne.text = cardOnDisplay.passive[0];
        //}
    }
    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {

    }
}
