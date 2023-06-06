using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EffectDisplayManager : MonoBehaviour
{
    [SerializeField]
    private Image _poisonImage, _momentumImage, _psionImage, _immaterialImage, _gravityImage, _delayedImage, _frozenImage, _burrowedImage, _adrenalineImage;
    [SerializeField]
    private TextMeshProUGUI _poisonCount;


    public void UpdateEffectDisplay(EffectBody effectBody)
    {
        _momentumImage.gameObject.SetActive(effectBody.IsMomentum);
        _psionImage.gameObject.SetActive(effectBody.IsPsion);
        _immaterialImage.gameObject.SetActive(effectBody.IsImmaterial);
        _gravityImage.gameObject.SetActive(effectBody.IsGravity);
        _delayedImage.gameObject.SetActive(effectBody.IsDelayed);
        _frozenImage.gameObject.SetActive(effectBody.IsFrozen);
        _burrowedImage.gameObject.SetActive(effectBody.IsBurrowed);
        _adrenalineImage.gameObject.SetActive(effectBody.IsAdrenaline);
        _poisonImage.gameObject.SetActive(effectBody.PoisonValue != 0);

        if (effectBody.PoisonValue != 0)
        {
            _poisonImage.sprite = ImageHelper.GetPoisonSprite(effectBody.PoisonValue > 0);
            _poisonImage.color = effectBody.IsAflatoxin ? new(108f, 108f, 108f) : new(byte.MaxValue, byte.MaxValue, byte.MaxValue);
            _poisonCount.text = $"{Mathf.Abs(effectBody.PoisonValue)}";
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
