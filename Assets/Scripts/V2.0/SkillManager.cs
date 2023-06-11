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

    public void SkillRoutineNoTarget(PlayerManager owner, IDCardPair idCard)
    {

        switch (idCard.card.skill)
        {
            case "duality":
                if (owner.isPlayer)
                {
                    Card cardToAdd = DuelManager.Instance.enemy.deckManager.GetTopCard();
                    if (cardToAdd == null) { return; }
                    owner.playerHand.AddCardToHand(new(cardToAdd));
                }
                else
                {
                    Card cardToAdd = DuelManager.Instance.player.deckManager.GetTopCard();
                    if (cardToAdd == null) { return; }
                    owner.playerHand.AddCardToHand(new(cardToAdd));
                }
                break;
            case "ignite":
                idCard.RemoveCard();

                if (owner.isPlayer)
                {
                    DuelManager.Instance.enemy.ModifyHealthLogic(20, true, false);
                }
                else
                {
                    DuelManager.Instance.player.ModifyHealthLogic(20, true, false);
                }

                var idList = DuelManager.Instance.enemy.playerCreatureField.GetAllValidCardIds();

                foreach (var idCardi in idList)
                {
                    idCardi.card.DefDamage += 1;
                    idCardi.UpdateCard();
                }

                idList = DuelManager.Instance.player.playerCreatureField.GetAllValidCardIds();

                foreach (var idCardi in idList)
                {
                    idCardi.card.DefDamage += 1;
                    idCardi.UpdateCard();
                }
                break;
            case "pandemonium":
                idList = DuelManager.Instance.player.playerCreatureField.GetAllValidCardIds();

                foreach (var idCardi in idList)
                {
                    ChoasSeed(DuelManager.Instance.player, idCardi);
                }

                idList = DuelManager.Instance.enemy.playerCreatureField.GetAllValidCardIds();

                foreach (var idCardi in idList)
                {
                    ChoasSeed(DuelManager.Instance.enemy, idCardi);
                }
                break;
            case "serendipity":
                CardType typeToAdd = ExtensionMethods.GetSerendipityWeighted();
                Element elementToAdd = Element.Entropy;

                for (int i = 0; i < 3; i++)
                {
                    owner.playerHand.AddCardToHand(CardDatabase.Instance.GetRandomCardOfTypeWithElement(typeToAdd, elementToAdd, idCard.card.iD.IsUpgraded()));

                    typeToAdd = ExtensionMethods.GetSerendipityWeighted();
                    elementToAdd = (Element)Random.Range(0, 12);
                    while (typeToAdd.Equals(CardType.Artifact) && elementToAdd.Equals(Element.Earth))
                    {
                        typeToAdd = ExtensionMethods.GetSerendipityWeighted();
                        elementToAdd = (Element)Random.Range(0, 12);
                    }
                }
                break;
            case "flying":
                Card weapon = new(owner.playerPassiveManager.GetWeapon());
                if (weapon.iD == "4t2") { return; }
                weapon.cardType = CardType.Creature;
                owner.PlayCardOnFieldLogic(weapon);
                owner.playerPassiveManager.RemoveWeapon();
                break;
            case "deadly poison":
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
                break;
            case "blitz":
                owner.SpendQuantaLogic(Element.Air, 75);
                var idCardList = owner.playerCreatureField.GetAllValidCardIds();
                foreach (var idCardi in idCardList)
                {
                    if (idCardi.card.innate.Contains("airborne"))
                    {
                        //Game_AnimationManager.shared.StartAnimation("Dive", Battlefield_ObjectIDManager.shared.GetObjectFromID(id));
                        idCardi.card.passive.Add("diving");
                        idCardi.UpdateCard();
                    }
                }
                break;
            case "bravery":
                int cardToDraw = owner.playerPassiveManager.GetMark().costElement.Equals(Element.Fire) ? 3 : 2;
                for (int i = 0; i < cardToDraw; i++)
                {
                    DuelManager.Instance.player.DrawCardFromDeckLogic();
                    DuelManager.Instance.enemy.DrawCardFromDeckLogic();
                }
                break;
            case "rain of fire":
                PlayerManager target = owner.isPlayer ? DuelManager.Instance.enemy : DuelManager.Instance.player;
                idList = target.playerCreatureField.GetAllValidCardIds();

                Game_SoundManager.shared.PlayAudioClip("Lightning");
                foreach (var idCardi in idList)
                {
                    idCardi.card.DefDamage += 3;
                    idCardi.UpdateCard();
                }
                break;
            case "thunderstorm":
                target = owner.isPlayer ? DuelManager.Instance.enemy : DuelManager.Instance.player;
                idList = target.playerCreatureField.GetAllValidCardIds();

                Game_SoundManager.shared.PlayAudioClip("Lightning");
                foreach (var idCardi in idList)
                {
                    idCardi.card.DefDamage += 2;
                    idCardi.UpdateCard();
                }
                break;
            case "sacrifice":
                owner.ModifyHealthLogic(40, true, false);
                for (int i = 0; i < 12; i++)
                {
                    if ((Element)i == Element.Death) { continue; }
                    owner.SpendQuantaLogic((Element)i, 75);
                }
                owner.sacrificeCount = 2;
                break;
            case "silence":
                owner.playerCounters.silence += 1;
                owner.silenceImage.gameObject.SetActive(owner.playerCounters.silence > 0);
                break;
            case "burrow":
                if (idCard.card.passive.Contains("burrow"))
                {
                    idCard.card.passive.Remove("burrow");
                    idCard.card.atk *= 2;
                    idCard.card.AtkModify *= 2;
                }
                else
                {
                    idCard.card.passive.Add("burrow");
                    idCard.card.atk /= 2;
                    idCard.card.AtkModify /= 2;
                }
                idCard.UpdateCard();
                break;
            case "divineshield":
                idCard.card.passive.Add("divineshield");
                idCard.UpdateCard();
                break;
            case "steam":
                idCard.card.Charge += 5;
                idCard.card.AtkModify += 5;
                idCard.card.DefModify += 5;
                idCard.UpdateCard();
                break;
            case "photosynthesis":
                Game_AnimationManager.shared.StartAnimation("QuantaGenerate", Battlefield_ObjectIDManager.shared.GetObjectFromID(BattleVars.shared.abilityOrigin.id), Element.Life);
                owner.GenerateQuantaLogic(Element.Life, 2);
                break;
            case "precognition":
                owner.DrawCardFromDeckLogic();
                DuelManager.RevealOpponentsHand();
                break;
            case "stone form":
                idCard.card.DefModify += 20;
                idCard.card.skill = "";
                idCard.card.desc = "";
                idCard.UpdateCard();
                break;
            case "poison":
                if (owner.isPlayer)
                {
                    DuelManager.Instance.enemy.playerCounters.poison += idCard.card.cardType.Equals(CardType.Spell) ? 2 : 1;
                    DuelManager.Instance.enemy.UpdatePlayerIndicators();
                }
                else
                {
                    DuelManager.Instance.player.playerCounters.poison += idCard.card.cardType.Equals(CardType.Spell) ? 2 : 1;
                    DuelManager.Instance.player.UpdatePlayerIndicators();
                }
                break;
            case "plague":
                target = owner.isPlayer ? DuelManager.Instance.enemy : DuelManager.Instance.player;
                idList = target.playerCreatureField.GetAllValidCardIds();

                foreach (var idCardi in idList)
                {
                    idCard.card.Poison += 1;
                    idCard.UpdateCard();
                }
                break;
            case "growth":
                idCard.card.DefModify += 2;
                idCard.card.AtkModify += 2;
                idCard.UpdateCard();
                break;
            case "gravity pullc":
                idCard.card.passive.Add("gravity pull");
                idCard.UpdateCard();
                break;
            case "dive":
                idCard.card.passive.Add("dive");
                idCard.card.AtkModify *= 2;
                idCard.card.atk *= 2;
                idCard.UpdateCard();
                break;
            case "unstable gas":
                owner.PlayCardOnFieldLogic(idCard.card.iD.IsUpgraded() ? CardDatabase.Instance.GetCardFromId("7n6") : CardDatabase.Instance.GetCardFromId("5om"));
                break;
            case "deja vu":
                idCard.card.skill = "";
                idCard.card.desc = "";
                Card dupe = new(idCard.card);
                owner.PlayCardOnFieldLogic(dupe);
                idCard.UpdateCard();
                break;
            case "black hole":
                PlayerManager victim = owner.isPlayer ? DuelManager.Instance.enemy : DuelManager.Instance.player;
                int hpToRestore = 0;

                for (int i = 0; i < 12; i++)
                {
                    if (victim.HasSufficientQuanta((Element)i, 3))
                    {
                        victim.SpendQuantaLogic((Element)i, 3);
                        hpToRestore += 3;
                    }
                    else if (victim.HasSufficientQuanta((Element)i, 2))
                    {
                        victim.SpendQuantaLogic((Element)i, 2);
                        hpToRestore += 2;
                    }
                    else if (victim.HasSufficientQuanta((Element)i, 1))
                    {
                        victim.SpendQuantaLogic((Element)i, 1);
                        hpToRestore++;
                    }
                }
                owner.ModifyHealthLogic(hpToRestore, false, false);
                break;
            case "luciferin":

                idList = DuelManager.Instance.enemy.playerCreatureField.GetAllValidCardIds();

                foreach (var idCardi in idList)
                {
                    if(idCardi.card.skill == "" && idCardi.card.passive.Contains("light"))
                    {
                        idCardi.card.passive.Add("light");
                        idCardi.card.desc = "Bioluminescence : \n Each turn <sprite=3> is generated";
                        idCardi.UpdateCard();
                    }
                }
                owner.ModifyHealthLogic(10, false, false);
                break;
            default:
                break;
        }
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

    private void ChoasSeed(PlayerManager targetOwner, IDCardPair iDCard)
    {
        PlayerManager originOwner = DuelManager.GetIDOwner(BattleVars.shared.abilityOrigin.id);
        int effect = Random.Range(0, 12);

        switch (effect)
        {
            case 0:
                iDCard.card.Poison++;
                break;
            case 1:
                iDCard.card.DefDamage += 5;
                break;
            case 2:
                int waterQ = originOwner.GetAllQuantaOfElement(Element.Water);
                int damageToDeal = 2 + (Mathf.FloorToInt(waterQ / 10) * 2);
                bool willFreeze = Random.Range(0, 100) > 30 + (damageToDeal * 5);

                iDCard.card.DefDamage += damageToDeal;
                if (willFreeze)
                {
                    iDCard.card.Freeze = 3;
                }
                break;
            case 3:
                waterQ = originOwner.GetAllQuantaOfElement(Element.Fire);
                damageToDeal = 2 + (Mathf.FloorToInt(waterQ / 10) * 2);

                iDCard.card.DefDamage += damageToDeal;
                break;
            case 4:
            case 5:
                originOwner.PlayCardOnFieldLogic(new Card(iDCard.card));
                break;
            case 6:
                iDCard.card.skill = "";
                iDCard.card.desc = "";
                iDCard.card.passive.Clear();
                break;
            case 7:
                Card cardToPlay = new Card(iDCard.card);
                originOwner.PlayCardOnFieldLogic(cardToPlay);
                Game_AnimationManager.shared.StartAnimation("Steal", Battlefield_ObjectIDManager.shared.GetObjectFromID(iDCard.id));
                iDCard.RemoveCard();
                return;
            case 8:
                iDCard.card.DefDamage += 3;
                break;
            case 9:
                if (iDCard.card.Freeze > 0)
                {
                    iDCard.RemoveCard();
                    return;
                }
                else
                {
                    iDCard.card.DefDamage += 4;
                }
                break;
            case 10:
                if (iDCard.card.innate.Contains("mummy"))
                {
                    Card pharoah = CardDatabase.Instance.GetCardFromId(iDCard.card.iD.IsUpgraded() ? "7qc" : "5rs");
                    iDCard.PlayCard(pharoah);
                }
                else if (iDCard.card.innate.Contains("undead"))
                {
                    Card rndCreature = iDCard.card.iD.IsUpgraded() ? CardDatabase.Instance.GetRandomEliteCreature() : CardDatabase.Instance.GetRandomCreature();
                    iDCard.PlayCard(rndCreature);
                }
                else
                {
                    Card baseCreature = CardDatabase.Instance.GetCardFromId(iDCard.card.iD);
                    targetOwner.AddCardToDeck(baseCreature);
                    iDCard.RemoveCard();
                }
                break;
            case 11:
                iDCard.card.passive.Add("gravity pull");
                break;
            default:
                break;
        }
        iDCard.UpdateCard();
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

    public void SkillRoutineWithTarget(PlayerManager targetOwner, IDCardPair iDCard)
    {
        Card card = BattleVars.shared.abilityOrigin.card;
        PlayerManager originOwner = DuelManager.GetIDOwner(BattleVars.shared.abilityOrigin.id);

        switch (card.skill)
        {
            case "wisdom":
                iDCard.card.passive.Add("psion");
                iDCard.card.desc = $"{iDCard.card.cardName}'s attacks deal spell damage.";
                break;
            case "nymph":
                originOwner.PlayCardOnFieldLogic(card.iD.IsUpgraded() ? CardDatabase.Instance.GetRandomEliteNymph(iDCard.card.costElement) : CardDatabase.Instance.GetRandomRegularNymph(iDCard.card.costElement));
                iDCard.RemoveCard();
                return;
            case "immolate":
            case "creamtion":
                iDCard.RemoveCard();
                for (int i = 0; i < 12; i++)
                {
                    targetOwner.GenerateQuantaLogic((Element)i, 1);
                }

                targetOwner.GenerateQuantaLogic(Element.Fire, card.skill == "cremation" ? 7 : 5);
                return;
            case "beserk":
                iDCard.card.AtkModify += 6;
                iDCard.card.DefModify -= 6;
                break;
            case "rage":
                iDCard.card.AtkModify += 5;
                iDCard.card.DefModify -= 5;
                break;
            case "icebolt":
                int quantaElement = originOwner.GetAllQuantaOfElement(Element.Water);
                int damageToDeal = 2 + (Mathf.FloorToInt(quantaElement / 10) * 2);
                bool willFreeze = Random.Range(0, 100) > 30 + (damageToDeal * 5);

                if (iDCard.card == null)
                {
                    targetOwner.ModifyHealthLogic(damageToDeal, true, true);
                    targetOwner.playerCounters.freeze += willFreeze ? 3 : 0;
                    return;
                }
                iDCard.card.DefDamage += damageToDeal;
                iDCard.card.Freeze += willFreeze ? 3 : 0;

                break;
            case "drain life":
                quantaElement = originOwner.GetAllQuantaOfElement(Element.Darkness);
                damageToDeal = 2 + (Mathf.FloorToInt(quantaElement / 10) * 2);

                if (iDCard.card == null)
                {
                    targetOwner.ModifyHealthLogic(damageToDeal, true, true);
                    originOwner.ModifyHealthLogic(damageToDeal, false, false);
                    return;
                }
                originOwner.ModifyHealthLogic(iDCard.card.DefNow < damageToDeal ? iDCard.card.DefNow : damageToDeal, false, false);
                iDCard.card.DefDamage += damageToDeal;

                break;
            case "fire bolt":
                quantaElement = originOwner.GetAllQuantaOfElement(Element.Fire);
                damageToDeal = 2 + (Mathf.FloorToInt(quantaElement / 10) * 2);

                if (iDCard.card == null)
                {
                    targetOwner.ModifyHealthLogic(damageToDeal, true, true);
                    return;
                }
                iDCard.card.DefDamage += damageToDeal;

                break;
            case "immortality":
            case "enchant":
                iDCard.card.innate.Add("immaterial");
                break;
            case "acceleration":
            case "overdrive":
                iDCard.card.desc = card.skill == "acceleration" ? "Acceleration: \n Gain +2 /-1 per turn" : "Overdrive: \n Gain +3 /-1 per turn";
                iDCard.card.skill = "";
                iDCard.card.passive.Add(card.skill);
                break;
            case "momentum":
                iDCard.card.AtkModify++;
                iDCard.card.DefModify++;
                iDCard.card.passive.Add(card.skill);
                break;
            case "adrenaline":
            case "antimatter":
            case "gravity pull":
                iDCard.card.passive.Add(card.skill);
                break;
            case "lobotomize":
                iDCard.card.skill = "";
                iDCard.card.desc = "";
                iDCard.card.passive.Clear();
                break;
            case "blessing":
            case "chaos power":
                int mod = card.skill == "blessing" ? 3 : Random.Range(1, 6);

                iDCard.card.DefModify += mod;
                iDCard.card.AtkModify += mod;
                break;
            case "heavy armor":
                iDCard.card.DefModify += 6;
                break;
            case "armor":
                iDCard.card.DefModify += 3;
                break;
            case "liquid shadow":
                iDCard.card.skill = "";
                iDCard.card.desc = "";
                iDCard.card.passive.Clear();
                iDCard.card.passive.Add("vampire");
                iDCard.card.Poison++;
                break;
            case "congeal":
                iDCard.card.Freeze += 4;
                break;
            case "freeze":
                iDCard.card.Freeze += 3;
                break;
            case "shockwave":
                if (iDCard.card.Freeze > 0)
                {
                    iDCard.RemoveCard();
                    return;
                }
                iDCard.card.DefDamage += 4;

                break;
            case "infection":
                iDCard.card.Poison++;
                break;
            case "infect":
                iDCard.card.Poison++;
                BattleVars.shared.abilityOrigin.RemoveCard();
                break;
            case "holy light":
                if (iDCard.card == null)
                {
                    targetOwner.ModifyHealthLogic(10, false, false);
                    return;
                }
                int damage = (iDCard.card.costElement.Equals(Element.Death) || iDCard.card.costElement.Equals(Element.Darkness)) ? -10 : 10;
                iDCard.card.DefDamage -= damage;
                if (iDCard.card.DefDamage < 0) { iDCard.card.DefDamage = 0; }
                break;
            case "heal":
                iDCard.card.DefDamage -= 5;
                if (iDCard.card.DefDamage < 0) { iDCard.card.DefDamage = 0; }
                break;
            case "nightmare":
                PlayerManager owner = originOwner;
                PlayerManager opponent = originOwner.isPlayer ? DuelManager.Instance.enemy : DuelManager.Instance.player;
                Card creature = CardDatabase.Instance.GetCardFromId(iDCard.card.iD);

                damage = 7 - opponent.GetHandCards().Count;
                opponent.FillHandWith(creature);
                opponent.ModifyHealthLogic(damage * 2, true, true);
                owner.ModifyHealthLogic(damage * 2, false, true);

                break;
            case "mitosiss":
                iDCard.card.skill = "mitosis";
                iDCard.card.desc = "Mitosis: \n Generate a daughter creature";
                iDCard.card.skillCost = iDCard.card.cost;
                iDCard.card.skillElement = iDCard.card.costElement;
                break;
            case "fractal":
                originOwner.FillHandWith(CardDatabase.Instance.GetCardFromId(iDCard.card.iD));
                originOwner.SpendQuantaLogic(Element.Aether, 75);
                return;
            case "aflatoxin":
                iDCard.card.IsAflatoxin = true;
                iDCard.card.Poison += 2;
                break;
            case "reverse time":
                if (iDCard.card.innate.Contains("mummy"))
                {
                    iDCard.PlayCard(CardDatabase.Instance.GetCardFromId(iDCard.card.iD.IsUpgraded() ? "7qc" : "5rs"));
                    return;
                }
                else if (iDCard.card.innate.Contains("undead"))
                {
                    iDCard.PlayCard(iDCard.card.iD.IsUpgraded() ? CardDatabase.Instance.GetRandomEliteCreature() : CardDatabase.Instance.GetRandomCreature());
                    return;
                }
                else
                {
                    Card baseCreature = new(iDCard.card);
                    targetOwner.AddCardToDeck(baseCreature);
                    iDCard.RemoveCard();
                    return;
                }
            case "purify":
                if (iDCard.card == null)
                {
                    targetOwner.sacrificeCount = 0;
                    targetOwner.playerCounters.nuerotoxin = 0;

                    targetOwner.playerCounters.poison -= targetOwner.playerCounters.poison > 0 ? (targetOwner.playerCounters.poison + 2) : 2;
                    return;
                }

                iDCard.card.IsAflatoxin = false;
                iDCard.card.Poison = iDCard.card.Poison > 0 ? 0 : iDCard.card.Poison;

                iDCard.card.Poison -= 2;
                break;
            case "earthquake":
                iDCard.RemoveCard();
                iDCard.RemoveCard();
                iDCard.RemoveCard();
                return;
            case "accretion":
                card.DefModify += 15;
                iDCard.RemoveCard();
                if (card.DefNow >= 45)
                {
                    originOwner.playerHand.AddCardToHand(card.iD.IsUpgraded() ? CardDatabase.Instance.GetCardFromId("74f") : CardDatabase.Instance.GetCardFromId("55v"));
                    BattleVars.shared.abilityOrigin.RemoveCard();
                    return;
                }

                BattleVars.shared.abilityOrigin.UpdateCard();
                return;
            case "destroy":
                iDCard.RemoveCard();
                return;
            case "improve":
                Game_AnimationManager.shared.StartAnimation("Mutation", Battlefield_ObjectIDManager.shared.GetObjectFromID(iDCard.id));
                iDCard.PlayCard(CardDatabase.Instance.GetMutant(iDCard.card.iD.IsUpgraded()));
                return;
            case "ready":
                iDCard.card.skillCost = 0;
                iDCard.card.passive.Add("readiness");
                break;
            case "catapult":
                damage = 100 * iDCard.card.DefNow / (100 + iDCard.card.DefNow);
                damage += iDCard.card.Freeze > 0 ? Mathf.FloorToInt(damage * 0.5f) : 0;
                iDCard.RemoveCard();

                DuelManager.GetNotIDOwner(iDCard.id).ModifyHealthLogic(damage, true, false);
                return;
            case "mutation":
                Game_AnimationManager.shared.StartAnimation("Mutation", Battlefield_ObjectIDManager.shared.GetObjectFromID(iDCard.id));
                switch (GetMutationResult())
                {
                    case MutationEnum.Kill:
                        iDCard.RemoveCard();
                        return;
                    case MutationEnum.Mutate:
                        iDCard.PlayCard(CardDatabase.Instance.GetMutant(iDCard.card.iD.IsUpgraded()));
                        break;
                    default:
                        iDCard.PlayCard(CardDatabase.Instance.GetCardFromId(iDCard.card.iD.IsUpgraded() ? "6tu" : "4ve"));
                        break;
                }
                break;
            case "guard":
                iDCard.card.innate.Add("delay");
                card.innate.Add("delay");
                if (!iDCard.card.innate.Contains("airborne"))
                {
                    iDCard.card.DefDamage += card.AtkNow;
                }
                BattleVars.shared.abilityOrigin.UpdateCard();
                break;
            case "petrify":
                for (int i = 0; i < 6; i++)
                {
                    iDCard.card.innate.Add("delay");
                }
                iDCard.card.DefModify += 20;
                break;
            case "chaos":
                ChoasSeed(targetOwner, iDCard);
                return;
            default:
                break;
        }
        iDCard.UpdateCard();
    }

}