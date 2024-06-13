using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Settings
{
    public class OptionToggle : MonoBehaviour
    {
        [SerializeField]
        private List<Image> toggleOptions;

        public int currentToggle = 0;

        public void SetUpToggle(int index)
        {
            for (var i = 0; i < toggleOptions.Count; i++)
            {
                toggleOptions[i].color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue / 2);
                if (i == index)
                {
                    toggleOptions[index].color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
                }
            }
            currentToggle = index;
        }

        public void ToggleOptions()
        {
            toggleOptions[currentToggle].color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue / 2);
            currentToggle++;
            if (currentToggle >= toggleOptions.Count) { currentToggle = 0; }
            toggleOptions[currentToggle].color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
        }

    }
}
