using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActiveCardDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _cardName, _creatureStats, _spellSymbol;
    [SerializeField]
    private ActiveAbilityDisplay _activeAbilityDisplay;
    [SerializeField]
    private AbilityAnimation _usableAbilityAnimation;
    [SerializeField]
    private EffectDisplayManager _cardEffectManager;
    [SerializeField]
    private BaseCardDisplay _baseCardDisplay;


    public void UpdateCardDisplay(Card card, EffectBody effectBody)
    {
        _cardEffectManager.UpdateEffectDisplay(effectBody);
        //_cardEffectManager.UpdateEffectDisplay(new EffectBody(card.Poison, 
        //    card.innate.Contains("momentum"), 
        //    card.innate.Contains("psion"),
        //    card.innate.Contains("immaterial"),
        //    ));
    }


}


