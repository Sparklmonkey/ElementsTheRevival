using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActiveAbilityDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _abilityName, _abilityCost;
    [SerializeField]
    private Image _abilityElement;

    public void ShowAbility(string name, int cost, Element element)
    {
        _abilityName.text = name;
        _abilityCost.text = $"{cost}";
        _abilityElement.sprite = ImageHelper.GetElementImage(element.FastElementString());
    }
}
