using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager
{
    private static readonly SkillManager instance = new SkillManager();

    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    static SkillManager()
    {
    }

    private SkillManager()
    {
    }

    public static SkillManager Instance
    {
        get
        {
            return instance;
        }
    }
    public IEnumerator SkillRoutineNoTarget(PlayerManager owner, Card card)
    {
        if (card.skill == "duality")
        {
            if (owner.isPlayer)
            {
                Card cardToAdd = DuelManager.Instance.enemy.deckManager.GetTopCard();
                if (cardToAdd == null) { yield break; }
                owner.playerHand.AddCardToHand(new(cardToAdd));
            }
            else
            {
                Card cardToAdd = DuelManager.Instance.player.deckManager.GetTopCard();
                if (cardToAdd == null) { yield break; }
                owner.playerHand.AddCardToHand(new(cardToAdd));
            }
        }
        if (card.skill == "scarab")
        {
            owner.PlayCardOnFieldLogic(card.iD.IsUpgraded() ? CardDatabase.Instance.GetCardFromId("7qa") : CardDatabase.Instance.GetCardFromId("5rq"));
        }
        if (card.skill == "mitosis")
        {
            Card daughterCard = CardDatabase.Instance.GetCardFromId(card.iD);
            owner.PlayCardOnFieldLogic(daughterCard);
        }
        if (card.skill == "healp")
        {
            owner.ModifyHealthLogic(20, false, false);
        }
        if (card.skill == "hasten")
        {
            owner.DrawCardFromDeckLogic();
        }
        if (card.skill == "ignite")
        {
            owner.StartCoroutine(owner.RemoveCardFromFieldLogic(BattleVars.shared.originId));
            if (owner.isPlayer)
            {
                DuelManager.Instance.enemy.ModifyHealthLogic(20, true, false);
            }
            else
            {
                DuelManager.Instance.player.ModifyHealthLogic(20, true, false);
            }

            List<ID> creatureIds = DuelManager.Instance.enemy.playerCreatureField.GetAllIds();
            creatureIds.AddRange(DuelManager.Instance.player.playerCreatureField.GetAllIds());

            List<Card> creatureCards = DuelManager.Instance.enemy.playerCreatureField.GetAllCards();
            creatureCards.AddRange(DuelManager.Instance.player.playerCreatureField.GetAllCards());

            for (int i = 0; i < creatureCards.Count; i++)
            {
                creatureCards[i].DefDamage += 1;
                DuelManager.GetIDOwner(creatureIds[i]).DisplayNewCard(creatureIds[i], creatureCards[i]);
            }
        }
        if (card.skill == "pandemonium")
        {

            List<ID> creatureIds = DuelManager.Instance.enemy.playerCreatureField.GetAllIds();
            creatureIds.AddRange(DuelManager.Instance.player.playerCreatureField.GetAllIds());

            List<Card> creatureCards = DuelManager.Instance.enemy.playerCreatureField.GetAllCards();
            creatureCards.AddRange(DuelManager.Instance.player.playerCreatureField.GetAllCards());
            for (int i = 0; i < creatureCards.Count; i++)
            {
                yield return owner.StartCoroutine(ChoasSeed(DuelManager.GetIDOwner(creatureIds[i]), creatureCards[i], creatureIds[i]));
            }
        }
        if (card.skill == "serendipity")
        {
            CardType typeToAdd = ExtensionMethods.GetSerendipityWeighted();
            Element elementToAdd = Element.Entropy;

            for (int i = 0; i < 3; i++)
            {
                owner.AddCardToDeck(CardDatabase.Instance.GetRandomCardOfTypeWithElement(typeToAdd, elementToAdd, card.iD.IsUpgraded()));
                owner.DrawCardFromDeckLogic(true);
                typeToAdd = ExtensionMethods.GetSerendipityWeighted();
                elementToAdd = (Element)Random.Range(0, 12);
                while (typeToAdd.Equals(CardType.Artifact) && elementToAdd.Equals(Element.Earth))
                {
                    typeToAdd = ExtensionMethods.GetSerendipityWeighted();
                    elementToAdd = (Element)Random.Range(0, 12);
                }
                yield return null;
            }
        }
        if (card.skill == "flying")
        {
            Card weapon = new(owner.playerPassiveManager.GetWeapon());
            if (weapon.iD == "4t2") { yield break; }
            weapon.cardType = CardType.Creature;
            owner.PlayCardOnFieldLogic(weapon);
            yield return owner.StartCoroutine(owner.RemoveCardFromFieldLogic(owner.playerPassiveManager.GetWeaponID()));
        }
        if (card.skill == "deadly poison")
        {
            if (owner.isPlayer)
            {
                DuelManager.Instance.enemy.playerCounters.poison += 3;
                DuelManager.Instance.enemy.UpdatePlayerIndicators();
            }
            else
            {
                DuelManager.Instance.player.playerCounters.poison += 3;
                DuelManager.Instance.player.UpdatePlayerIndicators();
            }
        }
        if (card.skill == "blitz")
        {
            yield return owner.StartCoroutine(owner.SpendQuantaLogic(Element.Air, 75));
            List<Card> creatures = owner.playerCreatureField.GetAllCards();
            foreach (Card creature in creatures)
            {
                if (creature.innate.Contains("airborne"))
                {
                    //Game_AnimationManager.shared.StartAnimation("Dive", Battlefield_ObjectIDManager.shared.GetObjectFromID(id));
                    creature.passive.Add("diving");
                }
            }
        }
        if (card.skill == "shard")
        {
            int maxHPBuff = owner.playerPassiveManager.GetMark().costElement.Equals(Element.Light) ? 24 : 16;
            owner.ModifyMaxHealthLogic(maxHPBuff, true);
        }
        if (card.skill == "bravery")
        {
            int cardToDraw = owner.playerPassiveManager.GetMark().costElement.Equals(Element.Fire) ? 3 : 2;
            for (int i = 0; i < cardToDraw; i++)
            {
                DuelManager.Instance.player.DrawCardFromDeckLogic();
                DuelManager.Instance.enemy.DrawCardFromDeckLogic();
                yield return null;
            }
        }
        if (card.skill == "rain of fire")
        {
            PlayerManager target = owner.isPlayer ? DuelManager.Instance.enemy : DuelManager.Instance.player;
            List<Card> creatureCards = target.playerCreatureField.GetAllCards();
            List<ID> creatureIds = target.playerCreatureField.GetAllIds();
            for (int i = 0; i < creatureCards.Count; i++)
            {
                Game_SoundManager.shared.PlayAudioClip("Lightning");
                creatureCards[i].DefDamage += 3;
                target.DisplayNewCard(creatureIds[i], creatureCards[i], true);
            }
        }
        if (card.skill == "thunderstorm")
        {
            PlayerManager target = owner.isPlayer ? DuelManager.Instance.enemy : DuelManager.Instance.player;
            List<Card> creatureCards = target.playerCreatureField.GetAllCards();
            List<ID> creatureIds = target.playerCreatureField.GetAllIds();
            for (int i = 0; i < creatureCards.Count; i++)
            {
                Game_SoundManager.shared.PlayAudioClip("Lightning");
                creatureCards[i].DefDamage += 2;
                target.DisplayNewCard(creatureIds[i], creatureCards[i], true);
            }
        }
        if (card.skill == "stoneskin")
        {
            int maxHPBuff = owner.GetAllQuantaOfElement(Element.Earth);
            owner.ModifyMaxHealthLogic(maxHPBuff, true);
        }
        if (card.skill == "miracle")
        {
            int maxHp = owner.healthManager.GetMaxHealth();
            int currentHP = owner.healthManager.GetCurrentHealth();

            int hpToHeal = maxHp - currentHP - 1;

            owner.ModifyHealthLogic(hpToHeal, false, true);
            owner.SpendQuantaLogic(Element.Light, 75);
        }
        if (card.skill == "supernova")
        {
            for (int i = 0; i < 12; i++)
            {
                yield return owner.StartCoroutine(owner.GenerateQuantaLogic((Element)i, 2));
            }
            if (BattleVars.shared.isSingularity > 0)
            {
                owner.PlayCardOnFieldLogic(CardDatabase.Instance.GetCardFromId("6ub"));
            }
            BattleVars.shared.isSingularity++;
        }
        if (card.skill == "nova")
        {
            for (int i = 0; i < 12; i++)
            {
                yield return owner.StartCoroutine(owner.GenerateQuantaLogic((Element)i, 1));
            }
            if (BattleVars.shared.isSingularity > 1)
            {
                owner.PlayCardOnFieldLogic(CardDatabase.Instance.GetCardFromId("4vr"));
            }
            BattleVars.shared.isSingularity++;
        }
        if (card.skill == "patience")
        {
            owner.RemoveCardFromFieldLogic(BattleVars.shared.originId);
        }
        if (card.skill == "sacrifice")
        {
            owner.ModifyHealthLogic(40, true, false);
            for (int i = 0; i < 12; i++)
            {
                if ((Element)i == Element.Death) { continue; }
                yield return owner.StartCoroutine(owner.SpendQuantaLogic((Element)i, 75));
            }
            owner.sacrificeCount = 2;
            yield return null;
        }
        if (card.skill == "silence")
        {
            owner.playerCounters.silence += 1;
            owner.silenceImage.gameObject.SetActive(owner.playerCounters.silence > 0);
        }
        if (card.skill == "rebirth")
        {
            owner.DisplayNewCard(BattleVars.shared.originId, card.iD.IsUpgraded() ? CardDatabase.Instance.GetCardFromId("7ds") : CardDatabase.Instance.GetCardFromId("5fc"));
        }
        if (card.skill == "burrow")
        {
            if (card.passive.Contains("burrow"))
            {
                card.passive.Remove("burrow");
                card.atk *= 2;
                card.AtkModify *= 2;
            }
            else
            {
                card.passive.Add("burrow");
                card.atk /= 2;
                card.AtkModify /= 2;
            }
            owner.DisplayNewCard(BattleVars.shared.originId, card, true);
        }
        if (card.skill == "divineshield")
        {
            card.passive.Add("divineshield");
            owner.DisplayNewCard(BattleVars.shared.originId, card, true);
        }
        if (card.skill == "queen")
        {
            owner.PlayCardOnFieldLogic(card.iD.IsUpgraded() ? CardDatabase.Instance.GetCardFromId("7n4") : CardDatabase.Instance.GetCardFromId("5ok"));
        }
        if (card.skill == "steam")
        {
            card.Charge += 5;
            card.AtkModify += 5;
            card.DefModify += 5;
            owner.DisplayNewCard(BattleVars.shared.originId, card, true);
        }
        if (card.skill == "photosynthesis")
        {
            Game_AnimationManager.shared.StartAnimation("QuantaGenerate", Battlefield_ObjectIDManager.shared.GetObjectFromID(BattleVars.shared.originId), Element.Life);
            owner.StartCoroutine(owner.GenerateQuantaLogic(Element.Life, 2));
        }
        if (card.skill == "precognition")
        {
            owner.DrawCardFromDeckLogic();
            DuelManager.RevealOpponentsHand();
        }
        if (card.skill == "stone form")
        {
            card.DefModify += 20;
            card.skill = "";
            card.desc = "";
            owner.DisplayNewCard(BattleVars.shared.originId, card, true);
        }
        if (card.skill == "poison")
        {
            if (owner.isPlayer)
            {
                DuelManager.Instance.enemy.playerCounters.poison += card.cardType.Equals(CardType.Spell) ? 2 : 1;
                DuelManager.Instance.enemy.UpdatePlayerIndicators();
            }
            else
            {
                DuelManager.Instance.player.playerCounters.poison += card.cardType.Equals(CardType.Spell) ? 2 : 1;
                DuelManager.Instance.player.UpdatePlayerIndicators();
            }
        }
        if (card.skill == "plague")
        {
            PlayerManager target = owner.isPlayer ? DuelManager.Instance.enemy : DuelManager.Instance.player;
            List<Card> creatureCards = target.playerCreatureField.GetAllCards();
            List<ID> creatureIds = target.playerCreatureField.GetAllIds();
            for (int i = 0; i < creatureCards.Count; i++)
            {
                creatureCards[i].Poison++;

                target.DisplayNewCard(creatureIds[i], creatureCards[i], true);
            }
        }
        if (card.skill == "growth")
        {
            card.DefModify += 2;
            card.AtkModify += 2;
            owner.DisplayNewCard(BattleVars.shared.originId, card, true);
        }
        if (card.skill == "gravity pullc")
        {
            card.passive.Add("gravity pull");
            owner.DisplayNewCard(BattleVars.shared.originId, card, true);
        }
        if (card.skill == "dive")
        {
            card.passive.Add("dive");
            card.AtkModify *= 2;
            card.atk *= 2;
            owner.DisplayNewCard(BattleVars.shared.originId, card, true);
        }
        if (card.skill == "unstable gas")
        {
            owner.PlayCardOnFieldLogic(card.iD.IsUpgraded() ? CardDatabase.Instance.GetCardFromId("7n6") : CardDatabase.Instance.GetCardFromId("5om"));
        }
        if (card.skill == "evolve")
        {
            Card newCreature = card.iD.IsUpgraded() ? CardDatabase.Instance.GetCardFromId("77h") : CardDatabase.Instance.GetCardFromId("591");
            owner.DisplayNewCard(BattleVars.shared.originId, newCreature, true);
        }
        if (card.skill == "hatch")
        {
            Card newCreature = card.iD.IsUpgraded() ? CardDatabase.Instance.GetRandomEliteHatchCreature() : CardDatabase.Instance.GetRandomHatchCreature();
            owner.DisplayNewCard(BattleVars.shared.originId, newCreature, true, true);
        }
        if (card.skill == "deja vu")
        {
            card.skill = "";
            card.desc = "";
            Card dupe = new(card);
            owner.PlayCardOnFieldLogic(dupe);
            owner.DisplayNewCard(BattleVars.shared.originId, card, true);
        }
        if (card.skill == "lycanthropy")
        {
            card.DefModify += 5;
            card.AtkModify += 5;
            card.skill = "";
            card.desc = "";
            owner.DisplayNewCard(BattleVars.shared.originId, card, true);
        }
        if (card.skill == "dead / alive")
        {
            Game_AnimationManager.shared.StartAnimation("DeadAndAlive", Battlefield_ObjectIDManager.shared.GetObjectFromID(BattleVars.shared.originId));
            yield return owner.StartCoroutine(DuelManager.Instance.player.ActivateDeathTriggers(card.cardName.Contains("Skeleton")));
            yield return owner.StartCoroutine(DuelManager.Instance.enemy.ActivateDeathTriggers(card.cardName.Contains("Skeleton")));
        }
        if (card.skill == "ablaze")
        {
            card.AtkModify += 2;
            owner.DisplayNewCard(BattleVars.shared.originId, card, true);
        }
        if (card.skill == "black hole")
        {

            PlayerManager victim = owner.isPlayer ? DuelManager.Instance.enemy : DuelManager.Instance.player;
            int hpToRestore = 0;

            for (int i = 0; i < 12; i++)
            {
                if (victim.HasSufficientQuanta((Element)i, 3))
                {
                    yield return victim.StartCoroutine(victim.SpendQuantaLogic((Element)i, 3));
                    hpToRestore += 3;
                }
                else if (victim.HasSufficientQuanta((Element)i, 2))
                {
                    yield return victim.StartCoroutine(victim.SpendQuantaLogic((Element)i, 2));
                    hpToRestore += 2;
                }
                else if (victim.HasSufficientQuanta((Element)i, 1))
                {
                    yield return victim.StartCoroutine(victim.SpendQuantaLogic((Element)i, 1));
                    hpToRestore++;
                }
            }
            owner.ModifyHealthLogic(hpToRestore, false, false);
        }
        if (card.skill == "luciferin")
        {
            List<Card> creatures = owner.playerCreatureField.GetAllCards();
            foreach (var creature in creatures)
            {
                if (creature.skill == "" && creature.passive.Contains("light"))
                {
                    creature.passive.Add("light");
                    creature.desc = "Bioluminescence : \n Each turn <sprite=3> is generated";
                }
            }
            owner.ModifyHealthLogic(10, false, false);
        }
        yield break;
    }

    public void SetupTargetHighlights(PlayerManager owner, PlayerManager enemy, Card card)
    {
        List<IDCardPair> iDCardPairs = new();
        //Setup Possible Targets
        List<IDCardPair> ownerCreatures = owner.playerCreatureField.GetAllValidCardIds();
        List<IDCardPair> ownerPermanents = owner.playerPermanentManager.GetAllValidCardIds();
        List<IDCardPair> ownerPassives = owner.playerPassiveManager.GetAllValidCardIds();

        List<IDCardPair> enemyCreatures = enemy.playerCreatureField.GetAllValidCardIds();
        List<IDCardPair> enemyPermanents = enemy.playerPermanentManager.GetAllValidCardIds();
        List<IDCardPair> enemyPassives = enemy.playerPassiveManager.GetAllValidCardIds();

        switch (card.skill)
        {
            case "butterfly":
                if (owner.playerCounters.invisibility == 0)
                {
                    iDCardPairs.AddRange(ownerCreatures.FindAll(x => x.card.AtkNow < 3));
                }

                if (enemy.playerCounters.invisibility == 0)
                {
                    iDCardPairs.AddRange(enemyCreatures.FindAll(x => x.card.AtkNow < 3));
                }
                break;
            case "wisdom":
                //TODO: Special Check for immaterial Creatures
                if (owner.playerCounters.invisibility == 0)
                {
                    iDCardPairs.AddRange(ownerCreatures.FindAll(x => x.card.innate.Contains("immaterial") && !x.card.innate.Contains("burrow")));
                }

                if (enemy.playerCounters.invisibility == 0)
                {
                    iDCardPairs.AddRange(enemyCreatures.FindAll(x => x.card.innate.Contains("immaterial") && !x.card.innate.Contains("burrow")));
                }
                break;
            case "paradox":
                if (owner.playerCounters.invisibility == 0)
                {
                    iDCardPairs.AddRange(ownerCreatures.FindAll(x => x.card.DefNow < x.card.AtkNow));
                }

                if (enemy.playerCounters.invisibility == 0)
                {
                    iDCardPairs.AddRange(enemyCreatures.FindAll(x => x.card.DefNow < x.card.AtkNow));
                }
                break;
            case "devour":
                if (owner.playerCounters.invisibility == 0)
                {
                    iDCardPairs.AddRange(ownerCreatures.FindAll(x => x.card.DefNow < card.DefNow));
                }

                if (enemy.playerCounters.invisibility == 0)
                {
                    iDCardPairs.AddRange(enemyCreatures.FindAll(x => x.card.DefNow < card.DefNow));
                }
                break;
            case "endow":
                if (owner.playerCounters.invisibility == 0)
                {
                    iDCardPairs.AddRange(ownerCreatures.FindAll(x => CardDatabase.Instance.weaponIdList.Contains(x.card.iD)));
                    if (ownerPassives.Exists(x => x.card.cardType == CardType.Weapon))
                    {
                        iDCardPairs.Add(ownerPassives.Find(x => x.card.cardType == CardType.Weapon));
                    }
                }
                break;
            case "web":
                if (owner.playerCounters.invisibility == 0)
                {
                    iDCardPairs.AddRange(ownerCreatures.FindAll(x => x.card.innate.Contains("airborne")));
                }

                if (enemy.playerCounters.invisibility == 0)
                {
                    iDCardPairs.AddRange(enemyCreatures.FindAll(x => x.card.innate.Contains("airborne")));
                }
                break;
            case "nymph":
                if (owner.playerCounters.invisibility == 0)
                {
                    iDCardPairs.AddRange(ownerPermanents.FindAll(x => x.card.cardType.Equals(CardType.Pillar)));
                }
                break;
            case "earthquake":
                if (owner.playerCounters.invisibility == 0)
                {
                    iDCardPairs.AddRange(ownerPermanents.FindAll(x => x.card.cardType.Equals(CardType.Pillar)));
                }

                if (enemy.playerCounters.invisibility == 0)
                {
                    iDCardPairs.AddRange(enemyPermanents.FindAll(x => x.card.cardType.Equals(CardType.Pillar)));
                }
                break;
            case "immolate":
            case "cremation":
            case "catapult":
                if (owner.playerCounters.invisibility == 0)
                {
                    iDCardPairs.AddRange(ownerCreatures);
                }
                break;
            case "rage":
            case "berserk":
            case "petrify":
            case "momentum":
            case "acceleration":
            case "parallel universe":
            case "gravity pull":
            case "chaos power":
            case "reverse time":
            case "freeze":
            case "congeal":
            case "immortality":
            case "adrenaline":
            case "antimatter":
            case "overdrive":
            case "chaos":
            case "shockwave":
            case "mutation":
            case "improve":
            case "liquid shadow":
            case "infect":
            case "infection":
            case "blessing":
            case "lobotomize":
            case "aflatoxin":
            case "readiness":
            case "nightmare":
            case "armor":
            case "fractal":
            case "heal":
            case "guard":
            case "sniper":
            case "heavy armor":
                if (owner.playerCounters.invisibility == 0)
                {
                    iDCardPairs.AddRange(ownerCreatures);
                }

                if (enemy.playerCounters.invisibility == 0)
                {
                    iDCardPairs.AddRange(enemyCreatures);
                }
                break;
            case "lightning":
            case "icebolt":
            case "fire bolt":
            case "holy light":
            case "drain life":
            case "purify":
                if (owner.playerCounters.invisibility == 0)
                {
                    iDCardPairs.AddRange(ownerCreatures);
                    iDCardPairs.Add(new IDCardPair(owner.playerDisplayer.GetObjectID(), null));
                }

                if (enemy.playerCounters.invisibility == 0)
                {
                    iDCardPairs.AddRange(enemyCreatures);
                    iDCardPairs.Add(new IDCardPair(enemy.playerDisplayer.GetObjectID(), null));
                }
                break;
            case "accretion":
            case "enchant":
            case "steal":
            case "destroy":
                if (owner.playerCounters.invisibility == 0)
                {
                    iDCardPairs.AddRange(ownerPermanents);
                    if (ownerPassives.Exists(x => x.card.cardType == CardType.Weapon))
                    {
                        iDCardPairs.Add(ownerPassives.Find(x => x.card.cardType == CardType.Weapon));
                    }
                    if (ownerPassives.Exists(x => x.card.cardType == CardType.Shield))
                    {
                        iDCardPairs.Add(ownerPassives.Find(x => x.card.cardType == CardType.Shield));
                    }
                }

                if (enemy.playerCounters.invisibility == 0)
                {
                    iDCardPairs.AddRange(enemyPermanents);
                    if (enemyPassives.Exists(x => x.card.cardType == CardType.Weapon))
                    {
                        iDCardPairs.Add(enemyPassives.Find(x => x.card.cardType == CardType.Weapon));
                    }
                    if (enemyPassives.Exists(x => x.card.cardType == CardType.Shield))
                    {
                        iDCardPairs.Add(enemyPassives.Find(x => x.card.cardType == CardType.Shield));
                    }
                }
                break;
            default:
                break;
        }
        DuelManager.SetupHighlights(iDCardPairs);
    }

    private IEnumerator ChoasSeed(PlayerManager targetOwner, Card targetCard, ID targetId)
    {
        Card card = BattleVars.shared.cardOnStandBy;
        PlayerManager originOwner = DuelManager.GetIDOwner(BattleVars.shared.originId);
        int effect = Random.Range(0, 12);

        switch (effect)
        {
            case 0:
                targetCard.Poison++;
                break;
            case 1:
                targetCard.DefDamage += 5;
                break;
            case 2:
                int waterQ = originOwner.GetAllQuantaOfElement(Element.Water);
                int damageToDeal = 2 + (Mathf.FloorToInt(waterQ / 10) * 2);
                bool willFreeze = Random.Range(0, 100) > 30 + (damageToDeal * 5);

                targetCard.DefDamage += damageToDeal;
                if (willFreeze)
                {
                    targetCard.Freeze = 3;
                }
                break;
            case 3:
                waterQ = originOwner.GetAllQuantaOfElement(Element.Fire);
                damageToDeal = 2 + (Mathf.FloorToInt(waterQ / 10) * 2);

                targetCard.DefDamage += damageToDeal;
                break;
            case 4:
            case 5:
                originOwner.PlayCardOnFieldLogic(new Card(targetCard));
                break;
            case 6:
                targetCard.skill = "";
                targetCard.desc = "";
                targetCard.passive.Clear();
                break;
            case 7:
                Card cardToPlay = new Card(targetCard);
                originOwner.PlayCardOnFieldLogic(cardToPlay);
                Game_AnimationManager.shared.StartAnimation("Steal", Battlefield_ObjectIDManager.shared.GetObjectFromID(targetId));
                yield return targetOwner.StartCoroutine(targetOwner.RemoveCardFromFieldLogic(targetId));
                break;
            case 8:
                targetCard.DefDamage += 3;
                break;
            case 9:
                if (targetCard.Freeze > 0)
                {
                    yield return targetOwner.StartCoroutine(targetOwner.RemoveCardFromFieldLogic(targetId));
                }
                else
                {
                    targetCard.DefDamage += 4;
                }
                break;
            case 10:
                if (targetCard.innate.Contains("mummy"))
                {
                    Card pharoah = CardDatabase.Instance.GetCardFromId(targetCard.iD.IsUpgraded() ? "7qc" : "5rs");
                    targetOwner.DisplayNewCard(targetId, pharoah);
                }
                else if (targetCard.innate.Contains("undead"))
                {
                    Card rndCreature = targetCard.iD.IsUpgraded() ? CardDatabase.Instance.GetRandomEliteCreature() : CardDatabase.Instance.GetRandomCreature();
                    targetOwner.DisplayNewCard(targetId, rndCreature);
                }
                else
                {
                    Card baseCreature = CardDatabase.Instance.GetCardFromId(targetCard.iD);
                    targetOwner.AddCardToDeck(baseCreature);
                    yield return targetOwner.StartCoroutine(targetOwner.RemoveCardFromFieldLogic(targetId, 1, false));
                }
                break;
            case 11:
                card.passive.Add("gravity pull");
                break;
            default:
                break;
        }

        targetOwner.DisplayNewCard(targetId, targetCard);
    }
    private MutationEnum GetMutationResult()
    {
        int num = Random.Range(0, 100);
        if (num >= 90)
        {
            return MutationEnum.Kill;
        }
        else if (num >= 50)
        {
            return MutationEnum.Mutate;
        }
        else
        {
            return MutationEnum.Abomination;
        }

    }
    public IEnumerator SkillRoutineWithTarget(PlayerManager targetOwner, Card targetCard, ID targetId)
    {
        Card card = BattleVars.shared.cardOnStandBy;
        PlayerManager originOwner = DuelManager.GetIDOwner(BattleVars.shared.originId);

        switch (card.skill)
        {
            case "butterfly":
                targetCard.skill = "destroy";
                targetCard.skillCost = 3;
                targetCard.skillElement = Element.Entropy;
                targetCard.desc = "<sprite=6><sprite=6><sprite=6>: Destroy: \n Destroy the targeted permanent";
                break;
            case "wisdom":
                targetCard.passive.Add("psion");
                targetCard.desc = $"{targetCard.cardName}'s attacks deal spell damage.";
                break;
            case "paradox":
                yield return targetOwner.StartCoroutine(targetOwner.RemoveCardFromFieldLogic(targetId));
                yield break;
            case "endow":
                card.skill = targetCard.skill;
                card.skillCost = targetCard.skillCost;
                card.skillElement = targetCard.skillElement;
                card.AtkModify += targetCard.AtkNow;
                card.DefModify += targetCard.DefNow;
                targetOwner.DisplayNewCard(BattleVars.shared.originId, card);
                yield break;
            case "web":
                targetCard.innate.Remove("airborne");
                break;
            case "nymph":
                originOwner.PlayCardOnFieldLogic(card.iD.IsUpgraded() ? CardDatabase.Instance.GetRandomEliteNymph(targetCard.costElement) : CardDatabase.Instance.GetRandomRegularNymph(targetCard.costElement));
                yield return targetOwner.StartCoroutine(targetOwner.RemoveCardFromFieldLogic(targetId));
                yield break;
            case "immolate":
            case "creamtion":
                targetOwner.StartCoroutine(targetOwner.RemoveCardFromFieldLogic(targetId, 1, true));
                for (int i = 0; i < 12; i++)
                {
                    targetOwner.StartCoroutine(targetOwner.GenerateQuantaLogic((Element)i, 1));
                }

                yield return targetOwner.StartCoroutine(targetOwner.GenerateQuantaLogic(Element.Fire, card.skill == "cremation" ? 7 : 5));
                yield break;
            case "beserk":
                targetCard.AtkModify += 6;
                targetCard.DefModify -= 6;
                break;
            case "rage":
                targetCard.AtkModify += 5;
                targetCard.DefModify -= 5;
                break;
            case "icebolt":
                int quantaElement = originOwner.GetAllQuantaOfElement(Element.Water);
                int damageToDeal = 2 + (Mathf.FloorToInt(quantaElement / 10) * 2);
                bool willFreeze = Random.Range(0, 100) > 30 + (damageToDeal * 5);

                if (targetCard == null)
                {
                    targetOwner.ModifyHealthLogic(damageToDeal, true, true);
                    targetOwner.playerCounters.freeze += willFreeze ? 3 : 0;
                    yield break;
                }
                targetCard.DefDamage += damageToDeal;
                targetCard.Freeze += willFreeze ? 3 : 0;

                break;
            case "drain life":
                quantaElement = originOwner.GetAllQuantaOfElement(Element.Darkness);
                damageToDeal = 2 + (Mathf.FloorToInt(quantaElement / 10) * 2);

                if (targetCard == null)
                {
                    targetOwner.ModifyHealthLogic(damageToDeal, true, true);
                    originOwner.ModifyHealthLogic(damageToDeal, false, false);
                    yield break;
                }
                originOwner.ModifyHealthLogic(targetCard.DefNow < damageToDeal ? targetCard.DefNow : damageToDeal, false, false);
                targetCard.DefDamage += damageToDeal;

                break;
            case "fire bolt":
                quantaElement = originOwner.GetAllQuantaOfElement(Element.Fire);
                damageToDeal = 2 + (Mathf.FloorToInt(quantaElement / 10) * 2);

                if (targetCard == null)
                {
                    targetOwner.ModifyHealthLogic(damageToDeal, true, true);
                    yield break;
                }
                targetCard.DefDamage += damageToDeal;

                break;
            case "sniper":
                targetCard.DefDamage += 3;
                break;
            case "lightning":
                if (targetCard == null)
                {
                    targetOwner.ModifyHealthLogic(5, true, true);
                    yield break;
                }
                targetCard.DefDamage += 5;

                break;
            case "immortality":
            case "enchant":
                targetCard.innate.Add("immaterial");
                break;
            case "acceleration":
            case "overdrive":
                targetCard.desc = card.skill == "acceleration" ? "Acceleration: \n Gain +2 /-1 per turn" : "Overdrive: \n Gain +3 /-1 per turn";
                targetCard.skill = "";
                targetCard.passive.Add(card.skill);
                break;
            case "momentum":
                targetCard.AtkModify++;
                targetCard.DefModify++;
                goto case "gravity pull";
            case "adrenaline":
            case "antimatter":
            case "gravity pull":
                targetCard.passive.Add(card.skill);
                break;
            case "lobotomize":
                targetCard.skill = "";
                targetCard.desc = "";
                targetCard.passive.Clear();
                break;
            case "blessing":
            case "chaos power":
                int mod = card.skill == "blessing" ? 3 : Random.Range(1, 6);

                targetCard.DefModify += mod;
                targetCard.AtkModify += mod;
                break;
            case "heavy armor":
                targetCard.DefModify += 6;
                break;
            case "armor":
                targetCard.DefModify += 3;
                break;
            case "liquid shadow":
                targetCard.skill = "";
                targetCard.desc = "";
                targetCard.passive.Clear();
                targetCard.passive.Add("vampire");
                targetCard.Poison++;
                break;
            case "parallel universe":
                originOwner.PlayCardOnFieldLogic(new(targetCard));
                yield break;
            case "congeal":
                targetCard.Freeze += 4;
                break;
            case "freeze":
                targetCard.Freeze += 3;
                break;
            case "shockwave":
                if (targetCard.Freeze > 0)
                {
                    yield return targetOwner.StartCoroutine(targetOwner.RemoveCardFromFieldLogic(targetId));
                    yield break;
                }
                targetCard.DefDamage += 4;

                break;
            case "infection":
                targetCard.Poison++;
                break;
            case "infect":
                targetCard.Poison++;
                yield return originOwner.StartCoroutine(originOwner.RemoveCardFromFieldLogic(BattleVars.shared.originId));
                break;
            case "holy light":
                if (targetCard == null)
                {
                    targetOwner.ModifyHealthLogic(10, false, false);
                    yield break;
                }
                int damage = (targetCard.costElement.Equals(Element.Death) || targetCard.costElement.Equals(Element.Darkness)) ? -10 : 10;
                targetCard.DefDamage -= damage;
                if (targetCard.DefDamage < 0) { targetCard.DefDamage = 0; }
                break;
            case "heal":
                targetCard.DefDamage -= 5;
                if (targetCard.DefDamage < 0) { targetCard.DefDamage = 0; }
                break;
            case "nightmare":
                PlayerManager owner = originOwner;
                PlayerManager opponent = originOwner.isPlayer ? DuelManager.Instance.enemy : DuelManager.Instance.player;
                Card creature = CardDatabase.Instance.GetCardFromId(targetCard.iD);

                damage = 7 - opponent.GetHandCards().Count;
                opponent.FillHandWith(creature);
                opponent.ModifyHealthLogic(damage * 2, true, true);
                owner.ModifyHealthLogic(damage * 2, false, true);

                break;
            case "mitosiss":
                targetCard.skill = "mitosis";
                targetCard.desc = "Mitosis: \n Generate a daughter creature";
                targetCard.skillCost = targetCard.cost;
                targetCard.skillElement = targetCard.costElement;
                break;
            case "fractal":
                originOwner.FillHandWith(CardDatabase.Instance.GetCardFromId(targetCard.iD));
                yield return originOwner.StartCoroutine(originOwner.SpendQuantaLogic(Element.Aether, 75));
                yield break;
            case "aflatoxin":
                targetCard.IsAflatoxin = true;
                targetCard.Poison += 2;
                break;
            case "reverse time":
                if (targetCard.innate.Contains("mummy"))
                {
                    targetCard = CardDatabase.Instance.GetCardFromId(targetCard.iD.IsUpgraded() ? "7qc" : "5rs");
                }
                else if (targetCard.innate.Contains("undead"))
                {
                    targetCard = targetCard.iD.IsUpgraded() ? CardDatabase.Instance.GetRandomEliteCreature() : CardDatabase.Instance.GetRandomCreature();
                }
                else
                {
                    Card baseCreature = CardDatabase.Instance.GetCardFromId(targetCard.iD);
                    targetOwner.AddCardToDeck(baseCreature);
                    yield return targetOwner.StartCoroutine(targetOwner.RemoveCardFromFieldLogic(targetId, 1, false));
                    yield break;
                }
                break;
            case "devour":
                card.AtkModify++;
                card.DefModify++;
                if (targetCard.innate.Contains("poisonous"))
                {
                    card.Poison++;
                }
                originOwner.DisplayNewCard(BattleVars.shared.originId, card);
                yield return targetOwner.StartCoroutine(targetOwner.RemoveCardFromFieldLogic(targetId));
                yield break;
            case "steal":
                Card cardToPlay = new(targetCard);
                originOwner.PlayCardOnFieldLogic(cardToPlay);
                Game_AnimationManager.shared.StartAnimation("Steal", Battlefield_ObjectIDManager.shared.GetObjectFromID(targetId));

                yield return targetOwner.StartCoroutine(targetOwner.RemoveCardFromFieldLogic(targetId, 1, false));
                yield break;
            case "purify":
                if (targetCard == null)
                {
                    targetOwner.sacrificeCount = 0;
                    targetOwner.playerCounters.nuerotoxin = 0;

                    targetOwner.playerCounters.poison -= targetOwner.playerCounters.poison > 0 ? (targetOwner.playerCounters.poison + 2) : 2;

                    yield break;
                }

                targetCard.IsAflatoxin = false;
                targetCard.Poison = targetCard.Poison > 0 ? 0 : targetCard.Poison;

                targetCard.Poison -= 2;
                break;
            case "earthquake":
                yield return targetOwner.StartCoroutine(targetOwner.RemoveCardFromFieldLogic(targetId, 3));
                yield break;
            case "accretion":
                card.DefModify += 15;

                if (card.DefNow >= 45)
                {
                    originOwner.playerHand.AddCardToHand(card.iD.IsUpgraded() ? CardDatabase.Instance.GetCardFromId("74f") : CardDatabase.Instance.GetCardFromId("55v"));
                    yield return originOwner.StartCoroutine(originOwner.RemoveCardFromFieldLogic(BattleVars.shared.originId));
                    goto case "destroy";
                }

                originOwner.DisplayNewCard(BattleVars.shared.originId, card);
                goto case "destroy";

            case "destroy":
                yield return targetOwner.StartCoroutine(targetOwner.RemoveCardFromFieldLogic(targetId));
                yield break;
            case "improve":
                Game_AnimationManager.shared.StartAnimation("Mutation", Battlefield_ObjectIDManager.shared.GetObjectFromID(targetId));
                targetCard = CardDatabase.Instance.GetMutant(targetCard.iD.IsUpgraded());
                break;
            case "ready":
                targetCard.skillCost = 0;
                targetCard.passive.Add("readiness");
                break;
            case "catapult":
                damage = 100 * targetCard.DefNow / (100 + targetCard.DefNow);
                damage += targetCard.Freeze > 0 ? Mathf.FloorToInt(damage * 0.5f) : 0;

                yield return targetOwner.StartCoroutine(targetOwner.RemoveCardFromFieldLogic(targetId));
                DuelManager.GetNotIDOwner(targetId).ModifyHealthLogic(damage, true, false);
                break;
            case "mutation":
                switch (GetMutationResult())
                {
                    case MutationEnum.Kill:
                        Game_AnimationManager.shared.StartAnimation("Mutation", Battlefield_ObjectIDManager.shared.GetObjectFromID(targetId));
                        yield return targetOwner.StartCoroutine(targetOwner.RemoveCardFromFieldLogic(targetId));
                        yield break;
                    case MutationEnum.Mutate:
                        Game_AnimationManager.shared.StartAnimation("Mutation", Battlefield_ObjectIDManager.shared.GetObjectFromID(targetId));
                        targetCard = CardDatabase.Instance.GetMutant(targetCard.iD.IsUpgraded());
                        break;
                    default:
                        Game_AnimationManager.shared.StartAnimation("Mutation", Battlefield_ObjectIDManager.shared.GetObjectFromID(targetId));
                        targetCard = CardDatabase.Instance.GetCardFromId(targetCard.iD.IsUpgraded() ? "6tu" : "4ve");
                        break;
                }
                break;
            case "guard":
                targetCard.innate.Add("delay");
                card.innate.Add("delay");
                if (!targetCard.innate.Contains("airborne"))
                {
                    targetCard.DefDamage += card.AtkNow;
                }
                originOwner.DisplayNewCard(BattleVars.shared.originId, card);
                break;
            case "petrify":
                for (int i = 0; i < 6; i++)
                {
                    targetCard.innate.Add("delay");
                }
                targetCard.DefModify += 20;
                break;
            case "chaos":
                yield return originOwner.StartCoroutine(ChoasSeed(targetOwner, targetCard, targetId));
                yield break;
            default:
                break;
        }
        targetOwner.DisplayNewCard(targetId, targetCard);
    }

    public IEnumerator SkillRoutineNoTargetRefactor(PlayerManager owner, Card card)
    {
        PlayerManager opponent = owner.isPlayer ? DuelManager.Instance.player : DuelManager.Instance.enemy;
        switch (card.skill)
        {
            case "duelity":
                owner.playerHand.AddCardToHand(new(opponent.deckManager.GetTopCard()));
                break;
            case "scarab":
                owner.PlayCardOnFieldLogic(card.iD.IsUpgraded() ? CardDatabase.Instance.GetCardFromId("7qa") : CardDatabase.Instance.GetCardFromId("5rq"));
                break;
            case "mitosis":
                Card daughterCard = CardDatabase.Instance.GetCardFromId(card.iD);
                owner.PlayCardOnFieldLogic(daughterCard);
                break;
            case "healp":
                owner.ModifyHealthLogic(20, false, false);
                break;
            case "hasten":
                owner.DrawCardFromDeckLogic();
                break;
            case "ignite":
                owner.StartCoroutine(owner.RemoveCardFromFieldLogic(BattleVars.shared.originId));
                opponent.ModifyHealthLogic(20, true, false);
                List<ID> creatureIds = DuelManager.Instance.enemy.playerCreatureField.GetAllIds();
                creatureIds.AddRange(DuelManager.Instance.player.playerCreatureField.GetAllIds());

                List<Card> creatureCards = DuelManager.Instance.enemy.playerCreatureField.GetAllCards();
                creatureCards.AddRange(DuelManager.Instance.player.playerCreatureField.GetAllCards());

                for (int i = 0; i < creatureCards.Count; i++)
                {
                    creatureCards[i].DefDamage += 1;
                    DuelManager.GetIDOwner(creatureIds[i]).DisplayNewCard(creatureIds[i], creatureCards[i]);
                }
                break;
            case "pandemonium":
                creatureIds = DuelManager.Instance.enemy.playerCreatureField.GetAllIds();
                creatureIds.AddRange(DuelManager.Instance.player.playerCreatureField.GetAllIds());

                creatureCards = DuelManager.Instance.enemy.playerCreatureField.GetAllCards();
                creatureCards.AddRange(DuelManager.Instance.player.playerCreatureField.GetAllCards());
                for (int i = 0; i < creatureCards.Count; i++)
                {
                    yield return owner.StartCoroutine(ChoasSeed(DuelManager.GetIDOwner(creatureIds[i]), creatureCards[i], creatureIds[i]));
                }
                break;
            case "serendipity":
                CardType typeToAdd = ExtensionMethods.GetSerendipityWeighted();
                Element elementToAdd = Element.Entropy;

                for (int i = 0; i < 3; i++)
                {
                    owner.AddCardToDeck(CardDatabase.Instance.GetRandomCardOfTypeWithElement(typeToAdd, elementToAdd, card.iD.IsUpgraded()));
                    owner.DrawCardFromDeckLogic(true);
                    typeToAdd = ExtensionMethods.GetSerendipityWeighted();
                    elementToAdd = (Element)Random.Range(0, 12);
                    while (typeToAdd.Equals(CardType.Artifact) && elementToAdd.Equals(Element.Earth))
                    {
                        typeToAdd = ExtensionMethods.GetSerendipityWeighted();
                        elementToAdd = (Element)Random.Range(0, 12);
                    }
                    yield return null;
                }
                break;
            default:
                break;
        }

        if (card.skill == "flying")
        {
            Card weapon = new(owner.playerPassiveManager.GetWeapon());
            if (weapon.iD == "4t2") { yield break; }
            weapon.cardType = CardType.Creature;
            owner.PlayCardOnFieldLogic(weapon);
            yield return owner.StartCoroutine(owner.RemoveCardFromFieldLogic(owner.playerPassiveManager.GetWeaponID()));
        }
        if (card.skill == "deadly poison")
        {
            if (owner.isPlayer)
            {
                DuelManager.Instance.enemy.playerCounters.poison += 3;
                DuelManager.Instance.enemy.UpdatePlayerIndicators();
            }
            else
            {
                DuelManager.Instance.player.playerCounters.poison += 3;
                DuelManager.Instance.player.UpdatePlayerIndicators();
            }
        }
        if (card.skill == "blitz")
        {
            yield return owner.StartCoroutine(owner.SpendQuantaLogic(Element.Air, 75));
            List<Card> creatures = owner.playerCreatureField.GetAllCards();
            foreach (Card creature in creatures)
            {
                if (creature.innate.Contains("airborne"))
                {
                    //Game_AnimationManager.shared.StartAnimation("Dive", Battlefield_ObjectIDManager.shared.GetObjectFromID(id));
                    creature.passive.Add("diving");
                }
            }
        }
        if (card.skill == "shard")
        {
            int maxHPBuff = owner.playerPassiveManager.GetMark().costElement.Equals(Element.Light) ? 24 : 16;
            owner.ModifyMaxHealthLogic(maxHPBuff, true);
        }
        if (card.skill == "bravery")
        {
            int cardToDraw = owner.playerPassiveManager.GetMark().costElement.Equals(Element.Fire) ? 3 : 2;
            for (int i = 0; i < cardToDraw; i++)
            {
                DuelManager.Instance.player.DrawCardFromDeckLogic();
                DuelManager.Instance.enemy.DrawCardFromDeckLogic();
                yield return null;
            }
        }
        if (card.skill == "rain of fire")
        {
            PlayerManager target = owner.isPlayer ? DuelManager.Instance.enemy : DuelManager.Instance.player;
            List<Card> creatureCards = target.playerCreatureField.GetAllCards();
            List<ID> creatureIds = target.playerCreatureField.GetAllIds();
            for (int i = 0; i < creatureCards.Count; i++)
            {
                Game_SoundManager.shared.PlayAudioClip("Lightning");
                creatureCards[i].DefDamage += 3;
                target.DisplayNewCard(creatureIds[i], creatureCards[i], true);
            }
        }
        if (card.skill == "thunderstorm")
        {
            PlayerManager target = owner.isPlayer ? DuelManager.Instance.enemy : DuelManager.Instance.player;
            List<Card> creatureCards = target.playerCreatureField.GetAllCards();
            List<ID> creatureIds = target.playerCreatureField.GetAllIds();
            for (int i = 0; i < creatureCards.Count; i++)
            {
                Game_SoundManager.shared.PlayAudioClip("Lightning");
                creatureCards[i].DefDamage += 2;
                target.DisplayNewCard(creatureIds[i], creatureCards[i], true);
            }
        }
        if (card.skill == "stoneskin")
        {
            int maxHPBuff = owner.GetAllQuantaOfElement(Element.Earth);
            owner.ModifyMaxHealthLogic(maxHPBuff, true);
        }
        if (card.skill == "miracle")
        {
            int maxHp = owner.healthManager.GetMaxHealth();
            int currentHP = owner.healthManager.GetCurrentHealth();

            int hpToHeal = maxHp - currentHP - 1;

            owner.ModifyHealthLogic(hpToHeal, false, true);
            owner.SpendQuantaLogic(Element.Light, 75);
        }
        if (card.skill == "supernova")
        {
            for (int i = 0; i < 12; i++)
            {
                yield return owner.StartCoroutine(owner.GenerateQuantaLogic((Element)i, 2));
            }
            if (BattleVars.shared.isSingularity > 0)
            {
                owner.PlayCardOnFieldLogic(CardDatabase.Instance.GetCardFromId("6ub"));
            }
            BattleVars.shared.isSingularity++;
        }
        if (card.skill == "nova")
        {
            for (int i = 0; i < 12; i++)
            {
                yield return owner.StartCoroutine(owner.GenerateQuantaLogic((Element)i, 1));
            }
            if (BattleVars.shared.isSingularity > 1)
            {
                owner.PlayCardOnFieldLogic(CardDatabase.Instance.GetCardFromId("4vr"));
            }
            BattleVars.shared.isSingularity++;
        }
        if (card.skill == "patience")
        {
            owner.RemoveCardFromFieldLogic(BattleVars.shared.originId);
        }
        if (card.skill == "sacrifice")
        {
            owner.ModifyHealthLogic(40, true, false);
            for (int i = 0; i < 12; i++)
            {
                if ((Element)i == Element.Death) { continue; }
                yield return owner.StartCoroutine(owner.SpendQuantaLogic((Element)i, 75));
            }
            owner.sacrificeCount = 2;
            yield return null;
        }
        if (card.skill == "silence")
        {
            owner.playerCounters.silence += 1;
            owner.silenceImage.gameObject.SetActive(owner.playerCounters.silence > 0);
        }
        if (card.skill == "rebirth")
        {
            owner.DisplayNewCard(BattleVars.shared.originId, card.iD.IsUpgraded() ? CardDatabase.Instance.GetCardFromId("7ds") : CardDatabase.Instance.GetCardFromId("5fc"));
        }
        if (card.skill == "burrow")
        {
            if (card.passive.Contains("burrow"))
            {
                card.passive.Remove("burrow");
                card.atk *= 2;
                card.AtkModify *= 2;
            }
            else
            {
                card.passive.Add("burrow");
                card.atk /= 2;
                card.AtkModify /= 2;
            }
            owner.DisplayNewCard(BattleVars.shared.originId, card, true);
        }
        if (card.skill == "divineshield")
        {
            card.passive.Add("divineshield");
            owner.DisplayNewCard(BattleVars.shared.originId, card, true);
        }
        if (card.skill == "queen")
        {
            owner.PlayCardOnFieldLogic(card.iD.IsUpgraded() ? CardDatabase.Instance.GetCardFromId("7n4") : CardDatabase.Instance.GetCardFromId("5ok"));
        }
        if (card.skill == "steam")
        {
            card.Charge += 5;
            card.AtkModify += 5;
            owner.DisplayNewCard(BattleVars.shared.originId, card, true);
        }
        if (card.skill == "photosynthesis")
        {
            Game_AnimationManager.shared.StartAnimation("QuantaGenerate", Battlefield_ObjectIDManager.shared.GetObjectFromID(BattleVars.shared.originId), Element.Life);
            owner.StartCoroutine(owner.GenerateQuantaLogic(Element.Life, 2));
        }
        if (card.skill == "precognition")
        {
            owner.DrawCardFromDeckLogic();
            DuelManager.RevealOpponentsHand();
        }
        if (card.skill == "stone form")
        {
            card.DefModify += 20;
            card.skill = "";
            card.desc = "";
            owner.DisplayNewCard(BattleVars.shared.originId, card, true);
        }
        if (card.skill == "poison")
        {
            if (owner.isPlayer)
            {
                DuelManager.Instance.enemy.playerCounters.poison += card.cardType.Equals(CardType.Spell) ? 2 : 1;
                DuelManager.Instance.enemy.UpdatePlayerIndicators();
            }
            else
            {
                DuelManager.Instance.player.playerCounters.poison += card.cardType.Equals(CardType.Spell) ? 2 : 1;
                DuelManager.Instance.player.UpdatePlayerIndicators();
            }
        }
        if (card.skill == "plague")
        {
            PlayerManager target = owner.isPlayer ? DuelManager.Instance.enemy : DuelManager.Instance.player;
            List<Card> creatureCards = target.playerCreatureField.GetAllCards();
            List<ID> creatureIds = target.playerCreatureField.GetAllIds();
            for (int i = 0; i < creatureCards.Count; i++)
            {
                creatureCards[i].Poison++;

                target.DisplayNewCard(creatureIds[i], creatureCards[i], true);
            }
        }
        if (card.skill == "growth")
        {
            card.DefModify += 2;
            card.AtkModify += 2;
            owner.DisplayNewCard(BattleVars.shared.originId, card, true);
        }
        if (card.skill == "gravity pullc")
        {
            card.passive.Add("gravity pull");
            owner.DisplayNewCard(BattleVars.shared.originId, card, true);
        }
        if (card.skill == "dive")
        {
            card.passive.Add("dive");
            card.AtkModify *= 2;
            card.atk *= 2;
            owner.DisplayNewCard(BattleVars.shared.originId, card, true);
        }
        if (card.skill == "unstable gas")
        {
            owner.PlayCardOnFieldLogic(card.iD.IsUpgraded() ? CardDatabase.Instance.GetCardFromId("7n6") : CardDatabase.Instance.GetCardFromId("5om"));
        }
        if (card.skill == "evolve")
        {
            Card newCreature = card.iD.IsUpgraded() ? CardDatabase.Instance.GetCardFromId("77h") : CardDatabase.Instance.GetCardFromId("591");
            owner.DisplayNewCard(BattleVars.shared.originId, newCreature, true);
        }
        if (card.skill == "hatch")
        {
            Card newCreature = card.iD.IsUpgraded() ? CardDatabase.Instance.GetRandomEliteHatchCreature() : CardDatabase.Instance.GetRandomHatchCreature();
            owner.DisplayNewCard(BattleVars.shared.originId, newCreature, true);
        }
        if (card.skill == "deja vu")
        {
            card.skill = "";
            card.desc = "";
            Card dupe = card.passive.Contains("mutant") ? CardDatabase.Instance.GetMutant(false, card) : new(card);
            owner.PlayCardOnFieldLogic(dupe);
            owner.DisplayNewCard(BattleVars.shared.originId, card, true);
        }
        if (card.skill == "lycanthropy")
        {
            card.DefModify += 5;
            card.AtkModify += 5;
            card.skill = "";
            card.desc = "";
            owner.DisplayNewCard(BattleVars.shared.originId, card, true);
        }
        if (card.skill == "dead / alive")
        {
            Game_AnimationManager.shared.StartAnimation("DeadAndAlive", Battlefield_ObjectIDManager.shared.GetObjectFromID(BattleVars.shared.originId));
            yield return owner.StartCoroutine(DuelManager.Instance.player.ActivateDeathTriggers(card.cardName.Contains("Skeleton")));
            yield return owner.StartCoroutine(DuelManager.Instance.enemy.ActivateDeathTriggers(card.cardName.Contains("Skeleton")));
        }
        if (card.skill == "ablaze")
        {
            card.AtkModify += 2;
            owner.DisplayNewCard(BattleVars.shared.originId, card, true);
        }
        if (card.skill == "black hole")
        {

            PlayerManager victim = owner.isPlayer ? DuelManager.Instance.enemy : DuelManager.Instance.player;
            int hpToRestore = 0;

            for (int i = 0; i < 12; i++)
            {
                if (victim.HasSufficientQuanta((Element)i, 3))
                {
                    yield return victim.StartCoroutine(victim.SpendQuantaLogic((Element)i, 3));
                    hpToRestore += 3;
                }
                else if (victim.HasSufficientQuanta((Element)i, 2))
                {
                    yield return victim.StartCoroutine(victim.SpendQuantaLogic((Element)i, 2));
                    hpToRestore += 2;
                }
                else if (victim.HasSufficientQuanta((Element)i, 1))
                {
                    yield return victim.StartCoroutine(victim.SpendQuantaLogic((Element)i, 1));
                    hpToRestore++;
                }
            }
            owner.ModifyHealthLogic(hpToRestore, false, false);
        }
        if (card.skill == "luciferin")
        {
            List<Card> creatures = owner.playerCreatureField.GetAllCards();
            foreach (var creature in creatures)
            {
                if (creature.skill == "" && creature.passive.Contains("light"))
                {
                    creature.passive.Add("light");
                    creature.desc = "Bioluminescence : \n Each turn <sprite=3> is generated";
                }
            }
            owner.ModifyHealthLogic(10, false, false);
        }
        yield break;
    }

}