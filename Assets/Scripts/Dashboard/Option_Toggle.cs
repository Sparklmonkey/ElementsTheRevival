using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option_Toggle : MonoBehaviour
{
    [SerializeField]
    private List<Image> toggleOptions;

    public int currentToggle = 0;

    public void SetUpToggle(int index)
    {
        for (int i = 0; i < toggleOptions.Count; i++)
        {
            toggleOptions[i].color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue / 2);
        }
        currentToggle = index;
        toggleOptions[currentToggle].color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
    }

    public void ToggleOptions()
    {
        toggleOptions[currentToggle].color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue / 2);
        currentToggle++;
        if(currentToggle >= toggleOptions.Count) { currentToggle = 0; }
        toggleOptions[currentToggle].color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
    }

}
