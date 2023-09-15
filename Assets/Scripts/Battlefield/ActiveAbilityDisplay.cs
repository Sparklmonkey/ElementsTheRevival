using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ActiveAbilityDisplay : MonoBehaviour
{
    [FormerlySerializedAs("_abilityName")] [SerializeField]
    private TextMeshProUGUI abilityName;

    [FormerlySerializedAs("_abilityCost")] [SerializeField]
    private TextMeshProUGUI abilityCost;

    [FormerlySerializedAs("_abilityElement")] [SerializeField]
    private Image abilityElement;

    public void ShowAbility(string name, int cost, Element element)
    {
        abilityName.text = name;
        abilityCost.text = $"{cost}";
        abilityElement.sprite = ImageHelper.GetElementImage(element.FastElementString());
    }
}
