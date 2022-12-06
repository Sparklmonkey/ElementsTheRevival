using System;

[Serializable]
public class CardAbilities
{
    //Ability that be used once per turn, has activation cost
    public string skillScript;
    //Ability that is activated at the end of the turn
    public string endTurnAbilityScript;
    //Ability that is activated when card is played
    public string onPlayAbilityScript;
    //Ability that is activated when another card is destroyed
    public string onDeathAbilityScript;

    //Shield ability -> damage reduction + status effects
    public string shieldAbilityScript;
    //Weapon ability -> damage modifiers + status effects
    public string weaponAbilityScript;
    //Spell ability 
    public string spellAbilityScript;

    public bool CreatureHasNoAbility()
    {
        return skillScript == "" && endTurnAbilityScript == "" && onPlayAbilityScript == "" && onDeathAbilityScript == "";
    }
}