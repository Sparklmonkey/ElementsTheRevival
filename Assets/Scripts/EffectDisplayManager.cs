using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EffectDisplayManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _poisonImage, _momentumImage, _psionImage, _immaterialImage, _gravityImage, _delayedImage, _frozenImage, _burrowedImage, _adrenalineImage;
    [SerializeField]
    private TextMeshProUGUI _poisonCount;

    public void UpdateEffectDisplay(Card card, int stack)
    {
        _momentumImage.SetActive(card.passiveSkills.Momentum);
        _psionImage.SetActive(card.passiveSkills.Psion);
        _immaterialImage.SetActive(card.innateSkills.Immaterial);
        _gravityImage.SetActive(card.passiveSkills.GravityPull);
        _delayedImage.SetActive(card.innateSkills.Delay > 0);
        _frozenImage.SetActive(card.Freeze > 0);
        _burrowedImage.SetActive(card.passiveSkills.Burrow);
        _adrenalineImage.SetActive(card.passiveSkills.Adrenaline);
        _poisonImage.SetActive(card.Poison != 0);

        if (card.Poison != 0)
        {
            _poisonImage.GetComponent<Image>().sprite = ImageHelper.GetPoisonSprite(card.Poison > 0);
            _poisonImage.GetComponent<Image>().color = card.IsAflatoxin ? new(108f, 108f, 108f) : new(byte.MaxValue, byte.MaxValue, byte.MaxValue);
            _poisonCount.text = $"{Mathf.Abs(card.Poison)}";
        }
    }
}

public class EffectBody
{
    public int PoisonValue;
    public bool IsMomentum;
    public bool IsPsion;
    public bool IsImmaterial;
    public bool IsGravity;
    public bool IsDelayed;
    public bool IsFrozen;
    public bool IsBurrowed;
    public bool IsAdrenaline;
    public bool IsAflatoxin;

    public EffectBody(int poisonCount = 0, bool isMomentum = false, bool isPsion = false, 
        bool isImmaterial = false, bool isGravity = false, bool isDelayed = false, 
        bool isFrozen = false, bool isBurrowed = false, bool isAdrenaline = false, bool isAflatoxin = false )
    {
        PoisonValue = poisonCount;
        IsMomentum = isMomentum;
        IsPsion = isPsion;
        IsImmaterial = isImmaterial;
        IsGravity = isGravity;
        IsDelayed = isDelayed;
        IsFrozen = isFrozen;
        IsBurrowed = isBurrowed;
        IsAdrenaline = isAdrenaline;
        IsAflatoxin = isAflatoxin;
    }

    public void UpdateEffect(string effectName, int poisonValue = 0, bool newValue = false)
    {
        // Get the PropertyInfo object for the property with the specified name
        PropertyInfo propertyInfo = this.GetType().GetProperty(effectName);

        // Set the value of the property using the PropertyInfo object
        if(poisonValue != 0)
        {
            propertyInfo.SetValue(this, poisonValue);
        }
        else
        {
            propertyInfo.SetValue(this, newValue);
        }
    }
}
