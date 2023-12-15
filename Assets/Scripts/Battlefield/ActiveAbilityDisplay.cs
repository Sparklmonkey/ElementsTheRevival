using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActiveAbilityDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI abilityNameLabel;
    [SerializeField]
    private TextMeshProUGUI abilityCost;
    [SerializeField]
    private Image abilityElement;

    public void ShowAbility(string abilityName, int cost, Element element)
    {
        abilityNameLabel.text = abilityName;
        abilityCost.text = $"{cost}";
        abilityElement.sprite = ImageHelper.GetElementImage(element.FastElementString());
    }
}
