using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestViewAllCards : MonoBehaviour
{
    public List<Card> cardList;
    public GameObject cardPrefab;
    public Transform contentView;
    // Start is called before the first frame update
    //void Start()
    //{
        //SetupStarterDecks();
        //StarterDeck starterDeck = Resources.Load<StarterDeck>("StarterDecks/Aether");
        //Debug.Log(starterDeck.Test5.Count);
        //foreach (Card card in cardList)
        //{
        //    GameObject cardObject = Instantiate(cardPrefab, contentView);
        //    cardObject.GetComponent<CardDisplay>().SetupCardView(card);
        //}
    //}

    public void ChangeDeckView(int element)
    {
        string elementToFind = ((Element)element).ToString();

        StarterDeck starterDeck = Resources.Load<StarterDeck>("StarterDecks/" + elementToFind);

        starterDeck.deck = starterDeck.deck.SortDeck();
        foreach (Transform child in contentView)
        {
            Destroy(child.gameObject);
        }

        foreach (Card card in starterDeck.deck)
        {
            GameObject cardObject = Instantiate(cardPrefab, contentView);
            cardObject.GetComponent<CardDisplay>().SetupCardView(card, null);
        }
    }


    private readonly string baseSOpath = @"Cards";

    void SetupStarterDecks()
    {
        StarterDeck starterDeck = Resources.Load<StarterDeck>("StarterDecks/Aether");
        starterDeck.deck = GetNewAetherDeck();
        starterDeck = Resources.Load<StarterDeck>("StarterDecks/Air");
        starterDeck.deck = GetNewAirDeck();
        starterDeck = Resources.Load<StarterDeck>("StarterDecks/Darkness");
        starterDeck.deck = GetNewDarknessDeck();
        starterDeck = Resources.Load<StarterDeck>("StarterDecks/Entropy");
        starterDeck.deck = GetNewEntropyDeck();
        starterDeck = Resources.Load<StarterDeck>("StarterDecks/Earth");
        starterDeck.deck = GetNewEarthDeck();
        starterDeck = Resources.Load<StarterDeck>("StarterDecks/Life");
        starterDeck.deck = GetNewLifeDeck();
        starterDeck = Resources.Load<StarterDeck>("StarterDecks/Light");
        starterDeck.deck = GetNewLightDeck();
        starterDeck = Resources.Load<StarterDeck>("StarterDecks/Death");
        starterDeck.deck = GetNewDeathDeck();
        starterDeck = Resources.Load<StarterDeck>("StarterDecks/Water");
        starterDeck.deck = GetNewWaterDeck();
        starterDeck = Resources.Load<StarterDeck>("StarterDecks/Time");
        starterDeck.deck = GetNewTimeDeck();
        starterDeck = Resources.Load<StarterDeck>("StarterDecks/Gravity");
        starterDeck.deck = GetNewGravityDeck();
        starterDeck = Resources.Load<StarterDeck>("StarterDecks/Fire");
        starterDeck.deck = GetNewFireDeck();
    }

    private List<Card> GetNewDeathDeck()
    {
        List<Card> list = new List<Card>
            {
                Resources.Load<Card>(baseSOpath + "/Creatures/Bone Dragon"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Virus"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Virus"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Virus"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Virus"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Flesh Spider"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Vulture"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Vulture"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Skeleton"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Skeleton"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Skeleton"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Sapphire Charger"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Graviton Mercenary"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Otyugh"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Gravity Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Gravity Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Gravity Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Quantum Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Quantum Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Quantum Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Bone Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Bone Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Bone Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Bone Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Bone Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Bone Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Bone Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Bone Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Bone Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Bone Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Bone Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Bone Pillar"),
                Resources.Load<Card>(baseSOpath + "/Spells/Poison"),
                Resources.Load<Card>(baseSOpath + "/Spells/Poison"),
                Resources.Load<Card>(baseSOpath + "/Spells/Plague"),
                Resources.Load<Card>(baseSOpath + "/Spells/Momentum"),
                Resources.Load<Card>(baseSOpath + "/Shields/Bone Wall"),
                Resources.Load<Card>(baseSOpath + "/Artifacts/Boneyard"),
                Resources.Load<Card>(baseSOpath + "/Artifacts/Boneyard"),
                Resources.Load<Card>(baseSOpath + "/Weapons/Dagger")
            };
       
        return list;
    }

    private List<Card> GetNewDarknessDeck()
    {
        List<Card> list = new List<Card>
        {
            Resources.Load<Card>(baseSOpath + "/Creatures/Black Dragon"),
            Resources.Load<Card>(baseSOpath + "/Creatures/Devourer"),
            Resources.Load<Card>(baseSOpath + "/Creatures/Devourer"),
            Resources.Load<Card>(baseSOpath + "/Creatures/Devourer"),
            Resources.Load<Card>(baseSOpath + "/Creatures/Devourer"),
            Resources.Load<Card>(baseSOpath + "/Creatures/Parasite"),
            Resources.Load<Card>(baseSOpath + "/Creatures/Parasite"),
            Resources.Load<Card>(baseSOpath + "/Creatures/Parasite"),
            Resources.Load<Card>(baseSOpath + "/Creatures/Spark"),
            Resources.Load<Card>(baseSOpath + "/Creatures/Immortal"),
            Resources.Load<Card>(baseSOpath + "/Pillars/Aether Pillar"),
            Resources.Load<Card>(baseSOpath + "/Pillars/Aether Pillar"),
            Resources.Load<Card>(baseSOpath + "/Pillars/Aether Pillar"),
            Resources.Load<Card>(baseSOpath + "/Pillars/Aether Pillar"),
            Resources.Load<Card>(baseSOpath + "/Pillars/Quantum Pillar"),
            Resources.Load<Card>(baseSOpath + "/Pillars/Quantum Pillar"),
            Resources.Load<Card>(baseSOpath + "/Pillars/Quantum Pillar"),
            Resources.Load<Card>(baseSOpath + "/Pillars/Obsidian Pillar"),
            Resources.Load<Card>(baseSOpath + "/Pillars/Obsidian Pillar"),
            Resources.Load<Card>(baseSOpath + "/Pillars/Obsidian Pillar"),
            Resources.Load<Card>(baseSOpath + "/Pillars/Obsidian Pillar"),
            Resources.Load<Card>(baseSOpath + "/Pillars/Obsidian Pillar"),
            Resources.Load<Card>(baseSOpath + "/Pillars/Obsidian Pillar"),
            Resources.Load<Card>(baseSOpath + "/Pillars/Obsidian Pillar"),
            Resources.Load<Card>(baseSOpath + "/Pillars/Obsidian Pillar"),
            Resources.Load<Card>(baseSOpath + "/Pillars/Obsidian Pillar"),
            Resources.Load<Card>(baseSOpath + "/Pillars/Obsidian Pillar"),
            Resources.Load<Card>(baseSOpath + "/Pillars/Obsidian Pillar"),
            Resources.Load<Card>(baseSOpath + "/Pillars/Obsidian Pillar"),
            Resources.Load<Card>(baseSOpath + "/Spells/Steal"),
            Resources.Load<Card>(baseSOpath + "/Spells/Steal"),
            Resources.Load<Card>(baseSOpath + "/Spells/Steal"),
            Resources.Load<Card>(baseSOpath + "/Spells/Drain Life"),
            Resources.Load<Card>(baseSOpath + "/Spells/Drain Life"),
            Resources.Load<Card>(baseSOpath + "/Spells/Drain Life"),
            Resources.Load<Card>(baseSOpath + "/Spells/Lightning"),
            Resources.Load<Card>(baseSOpath + "/Spells/Parallel Universe"),
            Resources.Load<Card>(baseSOpath + "/Shields/Dusk Mantle"),
            Resources.Load<Card>(baseSOpath + "/Artifacts/Nightfall"),
            Resources.Load<Card>(baseSOpath + "/Weapons/Dagger")
        };

        return list;
    }

    private List<Card> GetNewEarthDeck()
    {
        List<Card> list = new List<Card>
        {
            Resources.Load<Card>(baseSOpath + "/Creatures/Stone Dragon"),
            Resources.Load<Card>(baseSOpath + "/Creatures/Antlion"),
            Resources.Load<Card>(baseSOpath + "/Creatures/Antlion"),
            Resources.Load<Card>(baseSOpath + "/Creatures/Hematite Golem"),
            Resources.Load<Card>(baseSOpath + "/Creatures/Graboid"),
            Resources.Load<Card>(baseSOpath + "/Creatures/Graboid"),
            Resources.Load<Card>(baseSOpath + "/Creatures/Gnome Rider"),
            Resources.Load<Card>(baseSOpath + "/Creatures/Gnome Rider"),
            Resources.Load<Card>(baseSOpath + "/Creatures/Shrieker"),
            Resources.Load<Card>(baseSOpath + "/Creatures/Horned Frog"),
            Resources.Load<Card>(baseSOpath + "/Creatures/Forest Spirit"),
            Resources.Load<Card>(baseSOpath + "/Creatures/Cockatrice"),
            Resources.Load<Card>(baseSOpath + "/Weapons/Dagger"),
            Resources.Load<Card>(baseSOpath + "/Pillars/Emerald Pillar"),
            Resources.Load<Card>(baseSOpath + "/Pillars/Emerald Pillar"),
            Resources.Load<Card>(baseSOpath + "/Pillars/Emerald Pillar"),
            Resources.Load<Card>(baseSOpath + "/Pillars/Emerald Pillar"),
            Resources.Load<Card>(baseSOpath + "/Pillars/Quantum Pillar"),
            Resources.Load<Card>(baseSOpath + "/Pillars/Quantum Pillar"),
            Resources.Load<Card>(baseSOpath + "/Pillars/Quantum Pillar"),
            Resources.Load<Card>(baseSOpath + "/Pillars/Stone Pillar"),
            Resources.Load<Card>(baseSOpath + "/Pillars/Stone Pillar"),
            Resources.Load<Card>(baseSOpath + "/Pillars/Stone Pillar"),
            Resources.Load<Card>(baseSOpath + "/Pillars/Stone Pillar"),
            Resources.Load<Card>(baseSOpath + "/Pillars/Stone Pillar"),
            Resources.Load<Card>(baseSOpath + "/Pillars/Stone Pillar"),
            Resources.Load<Card>(baseSOpath + "/Pillars/Stone Pillar"),
            Resources.Load<Card>(baseSOpath + "/Pillars/Stone Pillar"),
            Resources.Load<Card>(baseSOpath + "/Pillars/Stone Pillar"),
            Resources.Load<Card>(baseSOpath + "/Pillars/Stone Pillar"),
            Resources.Load<Card>(baseSOpath + "/Pillars/Stone Pillar"),
            Resources.Load<Card>(baseSOpath + "/Pillars/Stone Pillar"),
            Resources.Load<Card>(baseSOpath + "/Pillars/Stone Pillar"),
            Resources.Load<Card>(baseSOpath + "/Spells/Earthquake"),
            Resources.Load<Card>(baseSOpath + "/Spells/Earthquake"),
            Resources.Load<Card>(baseSOpath + "/Spells/Plate Armor"),
            Resources.Load<Card>(baseSOpath + "/Spells/Plate Armor"),
            Resources.Load<Card>(baseSOpath + "/Spells/Enchant Artifact"),
            Resources.Load<Card>(baseSOpath + "/Spells/Heal"),
            Resources.Load<Card>(baseSOpath + "/Shields/Titanium Shield")
        };
        return list;
    }

    private List<Card> GetNewEntropyDeck()
    {
        List<Card> list = new List<Card>
            {
                Resources.Load<Card>(baseSOpath + "/Pillars/Quantum Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Quantum Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Quantum Pillar"),
                Resources.Load<Card>(baseSOpath + "/Weapons/Dagger"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Amethyst Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Amethyst Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Amethyst Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Amethyst Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Amethyst Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Amethyst Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Amethyst Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Amethyst Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Amethyst Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Amethyst Pillar"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Maxwell's Demon"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Abomination"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Abomination"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Abomination"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Purple Dragon"),
                Resources.Load<Card>(baseSOpath + "/Shields/Dissipation Shield"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Lycanthrope"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Lycanthrope"),
                Resources.Load<Card>(baseSOpath + "/Spells/Chaos Seed"),
                Resources.Load<Card>(baseSOpath + "/Spells/Chaos Seed"),
                Resources.Load<Card>(baseSOpath + "/Spells/Chaos Seed"),
                Resources.Load<Card>(baseSOpath + "/Spells/Mutation"),
                Resources.Load<Card>(baseSOpath + "/Spells/Mutation"),
                Resources.Load<Card>(baseSOpath + "/Spells/Mutation"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Bone Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Bone Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Bone Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Bone Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Bone Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Bone Pillar"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Virus"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Flesh Spider"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Vulture"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Skeleton"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Skeleton"),
                Resources.Load<Card>(baseSOpath + "/Spells/Poison")
            };
       
        return list;
    }

    private List<Card> GetNewGravityDeck()
    {
        List<Card> list = new List<Card>
            {
                Resources.Load<Card>(baseSOpath + "/Pillars/Quantum Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Quantum Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Quantum Pillar"),
                Resources.Load<Card>(baseSOpath + "/Weapons/Dagger"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Gravity Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Gravity Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Gravity Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Gravity Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Gravity Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Gravity Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Gravity Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Gravity Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Gravity Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Gravity Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Gravity Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Gravity Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Gravity Pillar"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Sapphire Charger"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Sapphire Charger"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Armagio"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Armagio"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Graviton Mercenary"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Graviton Mercenary"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Colossal Dragon"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Otyugh"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Otyugh"),
                Resources.Load<Card>(baseSOpath + "/Shields/Gravity Shield"),
                Resources.Load<Card>(baseSOpath + "/Spells/Momentum"),
                Resources.Load<Card>(baseSOpath + "/Spells/Momentum"),
                Resources.Load<Card>(baseSOpath + "/Spells/Gravity Pull"),
                Resources.Load<Card>(baseSOpath + "/Spells/Gravity Pull"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Stone Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Stone Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Stone Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Stone Pillar"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Hematite Golem"),
                Resources.Load<Card>(baseSOpath + "/Spells/Plate Armor"),
                Resources.Load<Card>(baseSOpath + "/Spells/Plate Armor"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Graboid"),
                Resources.Load<Card>(baseSOpath + "/Spells/Enchant Artifact")
            };
       
        return list;
    }

    private List<Card> GetNewFireDeck()
    {
        List<Card> list = new List<Card>
            {
                Resources.Load<Card>(baseSOpath + "/Pillars/Quantum Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Quantum Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Quantum Pillar"),
                Resources.Load<Card>(baseSOpath + "/Weapons/Dagger"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Burning Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Burning Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Burning Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Burning Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Burning Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Burning Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Burning Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Burning Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Burning Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Burning Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Burning Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Burning Pillar"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Ash Eater"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Ash Eater"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Ash Eater"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Ash Eater"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Crimson Dragon"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Fire Spirit"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Fire Spirit"),
                Resources.Load<Card>(baseSOpath + "/Shields/Fire Shield"),
                Resources.Load<Card>(baseSOpath + "/Spells/Fire Bolt"),
                Resources.Load<Card>(baseSOpath + "/Spells/Fire Bolt"),
                Resources.Load<Card>(baseSOpath + "/Spells/Fire Bolt"),
                Resources.Load<Card>(baseSOpath + "/Spells/Deflagration"),
                Resources.Load<Card>(baseSOpath + "/Spells/Deflagration"),
                Resources.Load<Card>(baseSOpath + "/Spells/Rain of Fire"),
                Resources.Load<Card>(baseSOpath + "/Spells/Rain of Fire"),
                Resources.Load<Card>(baseSOpath + "/Spells/Immolation"),
                Resources.Load<Card>(baseSOpath + "/Spells/Immolation"),
                Resources.Load<Card>(baseSOpath + "/Spells/Immolation"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Water Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Water Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Water Pillar"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Blue Crawler"),
                Resources.Load<Card>(baseSOpath + "/Spells/Freeze"),
                Resources.Load<Card>(baseSOpath + "/Spells/Purify")
            };
       
        return list;
    }

    private List<Card> GetNewLifeDeck()
    {
        List<Card> list = new List<Card>
            {
                Resources.Load<Card>(baseSOpath + "/Pillars/Quantum Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Quantum Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Quantum Pillar"),
                Resources.Load<Card>(baseSOpath + "/Weapons/Dagger"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Emerald Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Emerald Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Emerald Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Emerald Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Emerald Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Emerald Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Emerald Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Emerald Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Emerald Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Emerald Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Emerald Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Emerald Pillar"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Horned Frog"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Horned Frog"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Emerald Dragon"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Rustler"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Rustler"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Cockatrice"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Cockatrice"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Cockatrice"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Forest Spirit"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Forest Spirit"),
                Resources.Load<Card>(baseSOpath + "/Spells/Heal"),
                Resources.Load<Card>(baseSOpath + "/Spells/Heal"),
                Resources.Load<Card>(baseSOpath + "/Shields/Thorn Carapace"),
                Resources.Load<Card>(baseSOpath + "/Shields/Emerald Shield"),
                Resources.Load<Card>(baseSOpath + "/Artifacts/Empathic Bond"),
                Resources.Load<Card>(baseSOpath + "/Artifacts/Empathic Bond"),
                Resources.Load<Card>(baseSOpath + "/Spells/Fire Bolt"),
                Resources.Load<Card>(baseSOpath + "/Spells/Deflagration"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Ash Eater"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Ash Eater"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Burning Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Burning Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Burning Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Burning Pillar")
            };
       
        return list;
    }

    private List<Card> GetNewWaterDeck()
    {
        List<Card> list = new List<Card>
            {
                Resources.Load<Card>(baseSOpath + "/Pillars/Quantum Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Quantum Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Quantum Pillar"),
                Resources.Load<Card>(baseSOpath + "/Weapons/Dagger"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Water Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Water Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Water Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Water Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Water Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Water Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Water Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Water Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Water Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Water Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Water Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Water Pillar"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Chrysaora"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Chrysaora"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Ice Dragon"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Blue Crawler"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Blue Crawler"),
                Resources.Load<Card>(baseSOpath + "/Spells/Freeze"),
                Resources.Load<Card>(baseSOpath + "/Spells/Freeze"),
                Resources.Load<Card>(baseSOpath + "/Spells/Freeze"),
                Resources.Load<Card>(baseSOpath + "/Spells/Freeze"),
                Resources.Load<Card>(baseSOpath + "/Spells/Ice Bolt"),
                Resources.Load<Card>(baseSOpath + "/Spells/Ice Bolt"),
                Resources.Load<Card>(baseSOpath + "/Spells/Ice Bolt"),
                Resources.Load<Card>(baseSOpath + "/Spells/Ice Bolt"),
                Resources.Load<Card>(baseSOpath + "/Spells/Purify"),
                Resources.Load<Card>(baseSOpath + "/Shields/Ice Shield"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Light Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Light Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Light Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Light Pillar"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Photon"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Photon"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Pegasus"),
                Resources.Load<Card>(baseSOpath + "/Spells/Holy Light"),
                Resources.Load<Card>(baseSOpath + "/Spells/Blessing")
            };
       
        return list;
    }

    private List<Card> GetNewTimeDeck()
    {
        List<Card> list = new List<Card>
            {
                Resources.Load<Card>(baseSOpath + "/Pillars/Quantum Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Quantum Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Quantum Pillar"),
                Resources.Load<Card>(baseSOpath + "/Weapons/Dagger"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Time Factory"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Time Factory"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Time Factory"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Time Factory"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Time Factory"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Time Factory"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Time Factory"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Time Factory"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Time Factory"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Time Factory"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Time Factory"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Time Factory"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Time Factory"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Time Factory"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Deja Vu"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Deja Vu"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Deja Vu"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Fate Egg"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Fate Egg"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Fate Egg"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Devonian Dragon"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Anubis"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Anubis"),
                Resources.Load<Card>(baseSOpath + "/Spells/Reverse Time"),
                Resources.Load<Card>(baseSOpath + "/Spells/Reverse Time"),
                Resources.Load<Card>(baseSOpath + "/Artifacts/Golden Hourglass"),
                Resources.Load<Card>(baseSOpath + "/Artifacts/Golden Hourglass"),
                Resources.Load<Card>(baseSOpath + "/Shields/Procrastination"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Obsidian Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Obsidian Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Obsidian Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Obsidian Pillar"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Devourer"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Devourer"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Parasite"),
                Resources.Load<Card>(baseSOpath + "/Spells/Steal")
            };
       
        return list;
    }

    private List<Card> GetNewLightDeck()
    {
        List<Card> list = new List<Card>
            {
                Resources.Load<Card>(baseSOpath + "/Pillars/Quantum Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Quantum Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Quantum Pillar"),
                Resources.Load<Card>(baseSOpath + "/Weapons/Dagger"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Light Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Light Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Light Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Light Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Light Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Light Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Light Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Light Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Light Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Light Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Light Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Light Pillar"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Photon"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Photon"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Pegasus"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Pegasus"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Pegasus"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Golden Dragon"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Guardian Angel"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Guardian Angel"),
                Resources.Load<Card>(baseSOpath + "/Spells/Holy Light"),
                Resources.Load<Card>(baseSOpath + "/Spells/Holy Light"),
                Resources.Load<Card>(baseSOpath + "/Spells/Blessing"),
                Resources.Load<Card>(baseSOpath + "/Spells/Blessing"),
                Resources.Load<Card>(baseSOpath + "/Spells/Blessing"),
                Resources.Load<Card>(baseSOpath + "/Shields/Solar Shield"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Wind Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Wind Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Wind Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Wind Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Wind Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Wind Pillar"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Dragonfly"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Wyrm"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Firefly"),
                Resources.Load<Card>(baseSOpath + "/Spells/Thunderstorm")
            };
       
        return list;
    }

    private List<Card> GetNewAirDeck()
    {
        List<Card> list = new List<Card>
            {
                Resources.Load<Card>(baseSOpath + "/Pillars/Quantum Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Quantum Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Quantum Pillar"),
                Resources.Load<Card>(baseSOpath + "/Weapons/Dagger"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Wind Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Wind Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Wind Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Wind Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Wind Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Wind Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Wind Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Wind Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Wind Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Wind Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Wind Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Wind Pillar"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Dragonfly"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Dragonfly"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Dragonfly"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Wyrm"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Wyrm"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Wyrm"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Azure Dragon"),
                Resources.Load<Card>(baseSOpath + "/Spells/Thunderstorm"),
                Resources.Load<Card>(baseSOpath + "/Spells/Thunderstorm"),
                Resources.Load<Card>(baseSOpath + "/Spells/Thunderstorm"),
                Resources.Load<Card>(baseSOpath + "/Spells/Flying Weapon"),
                Resources.Load<Card>(baseSOpath + "/Shields/Fog Shield"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Deja Vu"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Fate Egg"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Fate Egg"),
                Resources.Load<Card>(baseSOpath + "/Spells/Reverse Time"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Time Factory"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Time Factory"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Time Factory"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Time Factory"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Firefly Queen"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Firefly"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Firefly"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Firefly")
            };
       
        return list;
    }

    private List<Card> GetNewAetherDeck()
    {
        List<Card> list = new List<Card>
            {
                Resources.Load<Card>(baseSOpath + "/Pillars/Quantum Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Quantum Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Quantum Pillar"),
                Resources.Load<Card>(baseSOpath + "/Weapons/Dagger"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Aether Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Aether Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Aether Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Aether Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Aether Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Aether Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Aether Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Aether Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Aether Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Aether Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Aether Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Aether Pillar"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Spark"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Spark"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Spark"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Spark"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Immortal"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Immortal"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Phase Dragon"),
                Resources.Load<Card>(baseSOpath + "/Spells/Lightning"),
                Resources.Load<Card>(baseSOpath + "/Spells/Lightning"),
                Resources.Load<Card>(baseSOpath + "/Spells/Lightning"),
                Resources.Load<Card>(baseSOpath + "/Spells/Lightning"),
                Resources.Load<Card>(baseSOpath + "/Spells/Parallel Universe"),
                Resources.Load<Card>(baseSOpath + "/Spells/Parallel Universe"),
                Resources.Load<Card>(baseSOpath + "/Spells/Parallel Universe"),
                Resources.Load<Card>(baseSOpath + "/Shields/Dimensional Shield"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Abomination"),
                Resources.Load<Card>(baseSOpath + "/Creatures/Lycanthrope"),
                Resources.Load<Card>(baseSOpath + "/Spells/Chaos Seed"),
                Resources.Load<Card>(baseSOpath + "/Spells/Nova"),
                Resources.Load<Card>(baseSOpath + "/Spells/Mutation"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Amethyst Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Amethyst Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Amethyst Pillar"),
                Resources.Load<Card>(baseSOpath + "/Pillars/Amethyst Pillar")
            };
       
        return list;
    }
}


