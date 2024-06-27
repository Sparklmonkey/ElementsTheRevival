using System.Collections.Generic;
using UnityEngine;

public class AiDeckBuilder
{
    public List<string> GetRandomDeck()
    {
        
        List<string> deckToReturn = new();
        for (var i = 0; i < 10; i++)
        {
            deckToReturn.Add(CardDatabase.Instance.GetRandomCard(CardType.Pillar, false, true).Id);
        }

        for (var i = 0; i < 20; i++)
        {
            deckToReturn.Add(CardDatabase.Instance.GetRandomCard(CardType.Creature, false, true).Id);
        }

        for (var i = 0; i < 10; i++)
        {
            deckToReturn.Add(CardDatabase.Instance.GetRandomCard(CardType.Spell, false, true).Id);
        }

        return deckToReturn;
    }

    public List<Card> GetHalfBloodDeck(Element primary, Element secondary)
    {
        List<Card> deckToReturn = new();
        var intance = CardDatabase.Instance;
        //Get Quantum Pillars
        for (var i = 0; i < 4; i++)
        {
            deckToReturn.Add(intance.GetCardFromId("4sa"));
        }

        var shouldBeUpgraded = Random.Range(0, 100) < 30;
        //Get Major Element Pillars
        for (var i = 0; i < 20; i++)
        {
            deckToReturn.Add(intance.GetRandomCardOfTypeWithElement(CardType.Pillar, primary, shouldBeUpgraded));
            shouldBeUpgraded = Random.Range(0, 100) < 30;
        }
        //Get Major Element Cards

        //Weapon
        for (var i = 0; i < 3; i++)
        {
            deckToReturn.Add(intance.GetRandomCardOfTypeWithElement(CardType.Weapon, primary, shouldBeUpgraded));
            shouldBeUpgraded = Random.Range(0, 100) < 30;
        }

        //Shield
        for (var i = 0; i < 3; i++)
        {
            deckToReturn.Add(intance.GetRandomCardOfTypeWithElement(CardType.Shield, primary, shouldBeUpgraded));
            shouldBeUpgraded = Random.Range(0, 100) < 30;
        }

        // Creatures
        for (var i = 0; i < 10; i++)
        {
            deckToReturn.Add(intance.GetRandomCardOfTypeWithElement(CardType.Creature, primary, shouldBeUpgraded));
            shouldBeUpgraded = Random.Range(0, 100) < 30;
        }

        //Spells
        for (var i = 0; i < 10; i++)
        {
            deckToReturn.Add(intance.GetRandomCardOfTypeWithElement(CardType.Spell, primary, shouldBeUpgraded));
            shouldBeUpgraded = Random.Range(0, 100) < 30;
        }

        //Artifacts
        if (!primary.Equals(Element.Entropy) && !primary.Equals(Element.Fire) && !primary.Equals(Element.Other))
        {
            for (var i = 0; i < 4; i++)
            {
                deckToReturn.Add(intance.GetRandomCardOfTypeWithElement(CardType.Artifact, primary, shouldBeUpgraded));
                shouldBeUpgraded = Random.Range(0, 100) < 30;
            }
        }

        //Get Minor Element Cards
        // Creatures
        for (var i = 0; i < 5; i++)
        {
            deckToReturn.Add(intance.GetRandomCard(CardType.Creature, shouldBeUpgraded, true, secondary, true));
            shouldBeUpgraded = Random.Range(0, 100) < 30;
        }

        //Spells
        for (var i = 0; i < 4; i++)
        {
            deckToReturn.Add(intance.GetRandomCard(CardType.Spell, shouldBeUpgraded, true, secondary, true));
            shouldBeUpgraded = Random.Range(0, 100) < 30;
        }
        
        //Artifacts
        if (!primary.Equals(Element.Entropy) && !primary.Equals(Element.Fire) && !primary.Equals(Element.Other))
        {
            for (var i = 0; i < 1; i++)
            {
                deckToReturn.Add(intance.GetRandomCard(CardType.Artifact, shouldBeUpgraded, true, secondary, true));
                shouldBeUpgraded = Random.Range(0, 100) < 30;
            }
        }

        return deckToReturn;
    }
}