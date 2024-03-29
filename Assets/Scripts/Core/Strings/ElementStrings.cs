using System;
using System.Collections.Generic;
using UnityEngine;

public static class ElementStrings
{
    private static string _aetherDesc = "Aether is the domain of the immobile things; it is where the stars reside and the media where all the energy waves move. Aether elementals excel in manipulating the dimensions and the electricity; most of the creatures they generate are immaterial.";
    private static string _airDesc = "Air elementals control any gaseous substance and airborne beings. The numerous air creatures take advantage of their freedom of movement to deal extra damage at the opponent.";
    private static string _darknessDesc = "Darkness elementals are masters of subtleness; they can hide, absorb energies and manage the creatures that dwell in the shadows. Darkness elementals can rely on skills like “steal” or “dusk mantle”.";
    private static string _lightDesc = "Light elementals dispense blessings and befriend angelic and righteous entities. Light elementals can exalt their creatures, heal them, or deal damage to death and darkness elements.";
    private static string _deathDesc = "Death elementals deal with poisons and infections; they will slowly kill their opponent's allies and make profit of the taken souls.";
    private static string _earthDesc = "Earth elementals have power over minerals, metals and terrestrial beings. Earth creatures have special skills that can be used for protection, like burrow: when burrowed, a creature deals less damage, but can not be harmed by common skills and spells. Earth elementals also take advantage of advanced shields that will reduce the damage inflicted to them.";
    private static string _entropyDesc = "Entropy is randomness; it is that thing that makes a glass unfixable once shattered. Entropy elementals deal with disarray and chaotic creatures; they love to confuse their opponent and gamble with luck and probabilities.";
    private static string _timeDesc = "A time elemental have control over the order of the events, he can slow down his opponent, reverse his actions, or hasten himself.";
    private static string _fireDesc = "Fire elementals are highly destructive; they rely on skills like “fireball”, or “destroy” to wipe out the enemy arsenal. Fire creatures deal higher than average damage but are often brittle.";
    private static string _gravityDesc = "Gravity elementals deal with mass and size; their ability to accelerate objects makes them formidable enemies. Gravity elementals have skills like “momentum”, that will make a gravity creature unstoppable and “gravity pull” that will waste the enemy attacks on a resistant or unessential creature.";
    private static string _lifeDesc = "Life elementals rely on a wide variety of powerful and different creatures; they can heal themselves and improve their ability to summon creatures using skills like “photosynthesis”.";
    private static string _waterDesc = "Ice, alchemy and aquatic beings are under the control of water elementals. Some examples of water elementals skills are: “freeze”, “ice bolt”, and “purify”, which is used to remove poisons. Water elementals have a good balance between offensive and defensive skills.";

    public static string GetElementDescription(Element element)
    {
        switch (element)
        {
            case Element.Aether:
                return _aetherDesc;
            case Element.Air:
                return _airDesc;
            case Element.Darkness:
                return _darknessDesc;
            case Element.Light:
                return _lightDesc;
            case Element.Death:
                return _deathDesc;
            case Element.Earth:
                return _earthDesc;
            case Element.Entropy:
                return _entropyDesc;
            case Element.Time:
                return _timeDesc;
            case Element.Fire:
                return _fireDesc;
            case Element.Gravity:
                return _gravityDesc;
            case Element.Life:
                return _lifeDesc;
            case Element.Water:
                return _waterDesc;
            case Element.Other:
                return _aetherDesc;
            default:
                return _aetherDesc;
        }
    }

    private static List<string> _elementHeadString = new()
    {"Today, the world will seem just out of reach.",
"Inspiration will come today, but don't hold your breath.",
"Misdirection and secrets rule the day today.",
"Today, the old must give way to the new.",
"Today is a day to remain firmly grounded.",
"Today, everything just seems to fall apart.",
"Today will be highly energetic.",
"Today, everything just seems to fall into place.",
"Today is a great day to be alive!",
"Today will be illuminating -- even enlightening.",
"Take your time today.",
"Today is a day to go with the ebb and flow of things.",
"Today will be surprisingly mundane." };


    private static List<string> _alchemyCardNames = new() { "Black Hole", "Liquid Shadow", "Antimatter", "Luciferin", "Quintessence", "Nymph's Tears", "Aflatoxin", "Basilisk Blood", "Adrenaline", "Rage Potion", "Unstable Gas", "Precognition" };
    public static string GetFortuneHeadString(CardType cardType, Element element, string cardName)
    {
        switch (cardType)
        {
            case CardType.Pillar:
                return "Today will be the foundation of something significant.";
            case CardType.Creature:
                if (cardName.Contains("Dragon"))
                {
                    return "Today is a BIG day.";
                }
                if (cardName.Contains("Nymph"))
                {
                    return "Today, everything comes together.";
                }
                return _elementHeadString[(int)element];
            case CardType.Spell:
                if (_alchemyCardNames.Contains(cardName))
                {
                    return "Today, everything changes.";
                }
                return _elementHeadString[(int)element];
            case CardType.Weapon:
                return "Today is a day to take risks!";
            case CardType.Shield:
                return "Today, you will need to be on your guard.";
            default:
                return _elementHeadString[(int)element];
        }
    }

    public static string GetCardBodyString(string cardName)
    {
        var json = Resources.Load<TextAsset>("JSON/OracleBodyStrings");

        var cardBodyStringArray = JsonUtility.FromJson<CardBodyStringArray>(json.text);

        foreach (var item in cardBodyStringArray.cardBodyStrings)
        {
            if (item.cardName == cardName)
            {
                return item.bodyString;
            }
        }

        return "If you're not careful, you'll find yourself surrounded by a crowd of identical mindsets. In a bad way.";
    }

}

[Serializable]
public class CardBodyStringArray
{
    public List<CardBodyString> cardBodyStrings;
}

[Serializable]
public class CardBodyString
{
    public string cardName;
    public string bodyString;
}