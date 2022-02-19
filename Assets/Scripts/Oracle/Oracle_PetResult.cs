using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oracle_PetResult : MonoBehaviour
{
    public string GetPetForNextBattle(Element element)
    {
        Card petCard = CardDatabase.GetRandomCardOfTypeWithElement(CardType.Creature, element, false);
        while (petCard.name.Contains("Nymph") || petCard.name.Contains("Shard") || petCard.name.Contains("Dragon"))
        {
            petCard = CardDatabase.GetRandomCardOfTypeWithElement(CardType.Creature, element, false);
        }

        return petCard.name;
    }
}
