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
        if(card.skill == "duality")
        {
            if (owner.isPlayer)
            {
                Card cardToAdd = DuelManager.enemy.deckManager.GetTopCard();
                if (cardToAdd == null) { yield break; }
                owner.playerHand.AddCardToHand(new (cardToAdd));
            }
            else
            {
                Card cardToAdd = DuelManager.player.deckManager.GetTopCard();
                if (cardToAdd == null) { yield break; }
                owner.playerHand.AddCardToHand(new (cardToAdd));
            }
        }
        if (card.skill == "scarab")
        {
            owner.PlayCardOnFieldLogic(card.iD.IsUpgraded() ? CardDatabase.GetCardFromId("7qa") : CardDatabase.GetCardFromId("5rq"));
        }
        if (card.skill == "mitosis")
        {
            Card daughterCard = CardDatabase.GetCardFromId(card.iD);
            owner.PlayCardOnFieldLogic(daughterCard);
        }
        if (card.skill == "healp")
        {
            owner.StartCoroutine(owner.ModifyHealthLogic(20, false, false));
        }
        if(card.skill == "hasten")
        {
            owner.DrawCardFromDeckLogic();
        }
        if(card.skill == "ignite")
        {
            owner.StartCoroutine(owner.RemoveCardFromFieldLogic(BattleVars.shared.originId));
            if (owner.isPlayer)
            {
                yield return DuelManager.enemy.StartCoroutine(DuelManager.enemy.ModifyHealthLogic(20, true, false));
            }
            else
            {
                yield return DuelManager.player.StartCoroutine(DuelManager.player.ModifyHealthLogic(20, true, false));
            }

            List<ID> creatureIds = DuelManager.enemy.playerCreatureField.GetAllIds();
            creatureIds.AddRange(DuelManager.player.playerCreatureField.GetAllIds());

            List<Card> creatureCards = DuelManager.enemy.playerCreatureField.GetAllCards();
            creatureCards.AddRange(DuelManager.player.playerCreatureField.GetAllCards());

            for (int i = 0; i < creatureCards.Count; i++)
            {
                creatureCards[i].DefDamage += 1;
                DuelManager.GetIDOwner(creatureIds[i]).DisplayNewCard(creatureIds[i], creatureCards[i]);
            }
        }
        if (card.skill == "pandemonium")
        {

            List<ID> creatureIds = DuelManager.enemy.playerCreatureField.GetAllIds();
            creatureIds.AddRange(DuelManager.player.playerCreatureField.GetAllIds());

            List<Card> creatureCards = DuelManager.enemy.playerCreatureField.GetAllCards();
            creatureCards.AddRange(DuelManager.player.playerCreatureField.GetAllCards());
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
                owner.AddCardToDeck(CardDatabase.GetRandomCardOfTypeWithElement(typeToAdd, elementToAdd, card.iD.IsUpgraded()));
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
            Card weapon = new (owner.playerPassiveManager.GetWeapon());
            if(weapon.iD == "4t2") { yield break; }
            weapon.cardType = CardType.Creature;
            owner.PlayCardOnFieldLogic(weapon);
            yield return owner.StartCoroutine(owner.RemoveCardFromFieldLogic(owner.playerPassiveManager.GetWeaponID()));
        }
        if (card.skill == "deadly poison")
        {
            if (owner.isPlayer)
            {
                DuelManager.enemy.playerCounters.poison += 3;
                DuelManager.enemy.UpdatePlayerIndicators();
            }
            else
            {
                DuelManager.player.playerCounters.poison += 3;
                DuelManager.player.UpdatePlayerIndicators();
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
                DuelManager.player.DrawCardFromDeckLogic();
                DuelManager.enemy.DrawCardFromDeckLogic();
                yield return null;
            }
        }
        if (card.skill == "rain of fire")
        {
            PlayerManager target = owner.isPlayer ? DuelManager.enemy : DuelManager.player;
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
            PlayerManager target = owner.isPlayer ? DuelManager.enemy : DuelManager.player;
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

            yield return owner.StartCoroutine(owner.ModifyHealthLogic(hpToHeal, false, true));
            yield return owner.StartCoroutine(owner.SpendQuantaLogic(Element.Light, 75));
        }
        if (card.skill == "supernova")
        {
            for (int i = 0; i < 12; i++)
            {
                yield return owner.StartCoroutine(owner.GenerateQuantaLogic((Element)i, 2));
            }
            if (BattleVars.shared.isSingularity > 0)
            {
                owner.PlayCardOnFieldLogic(CardDatabase.GetCardFromId("6ub"));
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
                owner.PlayCardOnFieldLogic(CardDatabase.GetCardFromId("4vr"));
            }
            BattleVars.shared.isSingularity++;
        }
        if (card.skill == "patience")
        {
            owner.RemoveCardFromFieldLogic(BattleVars.shared.originId);
        }
        if (card.skill == "sacrifice")
        {
            yield return owner.StartCoroutine(owner.ModifyHealthLogic(40, true, false));
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
            owner.DisplayNewCard(BattleVars.shared.originId, card.iD.IsUpgraded() ? CardDatabase.GetCardFromId("7ds") : CardDatabase.GetCardFromId("5fc"));
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
            owner.PlayCardOnFieldLogic(card.iD.IsUpgraded() ? CardDatabase.GetCardFromId("7n4") : CardDatabase.GetCardFromId("5ok"));
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
                DuelManager.enemy.playerCounters.poison += card.cardType.Equals(CardType.Spell) ? 2 : 1;
                DuelManager.enemy.UpdatePlayerIndicators();
            }
            else
            {
                DuelManager.player.playerCounters.poison += card.cardType.Equals(CardType.Spell) ? 2 : 1;
                DuelManager.player.UpdatePlayerIndicators();
            }
        }
        if (card.skill == "plague")
        {
            PlayerManager target = owner.isPlayer ? DuelManager.enemy : DuelManager.player;
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
            owner.PlayCardOnFieldLogic(card.iD.IsUpgraded() ? CardDatabase.GetCardFromId("7n6") : CardDatabase.GetCardFromId("5om"));
        }
        if (card.skill == "evolve")
        {
            Card newCreature = card.iD.IsUpgraded() ? CardDatabase.GetCardFromId("77h") : CardDatabase.GetCardFromId("591");
            owner.DisplayNewCard(BattleVars.shared.originId, newCreature, true);
        }
        if (card.skill == "hatch")
        {
            Card newCreature = card.iD.IsUpgraded() ? CardDatabase.GetRandomEliteHatchCreature() : CardDatabase.GetRandomHatchCreature();
            owner.DisplayNewCard(BattleVars.shared.originId, newCreature, true);
        }
        if (card.skill == "deja vu")
        {
            card.skill = "";
            card.desc = "";
            Card dupe = new (card);
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
            yield return owner.StartCoroutine(DuelManager.player.ActivateDeathTriggers(card.cardName.Contains("Skeleton")));
            yield return owner.StartCoroutine(DuelManager.enemy.ActivateDeathTriggers(card.cardName.Contains("Skeleton")));
        }
        if (card.skill == "ablaze")
        {
            card.AtkModify += 2;
            owner.DisplayNewCard(BattleVars.shared.originId, card, true);
        }
        if (card.skill == "black hole")
        {

            PlayerManager victim = owner.isPlayer ? DuelManager.enemy : DuelManager.player;
            int hpToRestore = 0;

            for (int i = 0; i < 12; i++)
            {
                if (victim.HasSufficientQuanta((Element)i, 3))
                {
                    yield return victim.StartCoroutine(victim.SpendQuantaLogic((Element)i, 3));
                    hpToRestore++;
                }
            }
            yield return owner.StartCoroutine(owner.ModifyHealthLogic(hpToRestore, false, false));
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
            yield return owner.StartCoroutine(owner.ModifyHealthLogic(10, false, false));
        }
        yield break;
    }

    public void SetupTargetHighlights(PlayerManager owner, PlayerManager enemy, Card card)
    {
        DuelManager.validTargets = new List<ID>();
        if (card.skill == "butterfly")
        {
            List<ID> permIds = owner.playerCreatureField.GetAllIds();
            List<Card> permCards = owner.playerCreatureField.GetAllCards();

            if (owner.playerCounters.invisibility == 0)
            {
                for (int i = 0; i < permCards.Count; i++)
                {
                    if (!permCards[i].innate.Contains("immaterial") && !permCards[i].innate.Contains("burrow") && permCards[i].AtkModify < 3)
                    {
                        owner.creatureDisplayers[permIds[i].Index].ShouldShowTarget(true);
                        DuelManager.validTargets.Add(permIds[i]);
                    }
                }
            }
            permIds = enemy.playerCreatureField.GetAllIds();
            permCards = enemy.playerCreatureField.GetAllCards();

            if (enemy.playerCounters.invisibility == 0)
            {
                for (int i = 0; i < permCards.Count; i++)
                {
                    if (!permCards[i].innate.Contains("immaterial") && !permCards[i].innate.Contains("burrow") && permCards[i].AtkModify < 3)
                    {
                        enemy.creatureDisplayers[permIds[i].Index].ShouldShowTarget(true);
                        DuelManager.validTargets.Add(permIds[i]);
                    }
                }
            }
        }
        if(card.skill == "wisdom")
        {
            List<ID> permIds = owner.playerCreatureField.GetAllIds();
            List<Card> permCards = owner.playerCreatureField.GetAllCards();

            if (owner.playerCounters.invisibility == 0)
            {
                for (int i = 0; i < permCards.Count; i++)
                {
                    if (permCards[i].innate.Contains("immaterial"))
                    {
                        owner.creatureDisplayers[permIds[i].Index].ShouldShowTarget(true);
                        DuelManager.validTargets.Add(permIds[i]);
                    }
                }
            }
            permIds = enemy.playerCreatureField.GetAllIds();
            permCards = enemy.playerCreatureField.GetAllCards();

            if (enemy.playerCounters.invisibility == 0)
            {
                for (int i = 0; i < permCards.Count; i++)
                {
                    if (permCards[i].innate.Contains("immaterial"))
                    {
                        enemy.creatureDisplayers[permIds[i].Index].ShouldShowTarget(true);
                        DuelManager.validTargets.Add(permIds[i]);
                    }
                }
            }
        }
        if(card.skill == "paradox")
        {
            List<ID> permIds = owner.playerCreatureField.GetAllIds();
            List<Card> permCards = owner.playerCreatureField.GetAllCards();

            if (owner.playerCounters.invisibility == 0)
            {
                for (int i = 0; i < permCards.Count; i++)
                {
                    if (!permCards[i].innate.Contains("immaterial") && !permCards[i].passive.Contains("burrow") && permCards[i].DefNow < permCards[i].AtkNow)
                    {
                        owner.creatureDisplayers[permIds[i].Index].ShouldShowTarget(true);
                        DuelManager.validTargets.Add(permIds[i]);
                    }
                }
            }
            permIds = enemy.playerCreatureField.GetAllIds();
            permCards = enemy.playerCreatureField.GetAllCards();

            if (enemy.playerCounters.invisibility == 0)
            {
                for (int i = 0; i < permCards.Count; i++)
                {
                    if (!permCards[i].innate.Contains("immaterial") && !permCards[i].passive.Contains("burrow") && permCards[i].DefNow < permCards[i].AtkNow)
                    {
                        enemy.creatureDisplayers[permIds[i].Index].ShouldShowTarget(true);
                        DuelManager.validTargets.Add(permIds[i]);
                    }
                }
            }
        }
        if(card.skill == "devour")
        {
            List<ID> permIds = owner.playerCreatureField.GetAllIds();
            List<Card> permCards = owner.playerCreatureField.GetAllCards();

            if (owner.playerCounters.invisibility == 0)
            {
                for (int i = 0; i < permCards.Count; i++)
                {
                    if (!permCards[i].innate.Contains("immaterial") && !permCards[i].passive.Contains("burrow") && permCards[i].DefNow < card.DefNow)
                    {
                        owner.creatureDisplayers[permIds[i].Index].ShouldShowTarget(true);
                        DuelManager.validTargets.Add(permIds[i]);
                    }
                }
            }
            permIds = enemy.playerCreatureField.GetAllIds();
            permCards = enemy.playerCreatureField.GetAllCards();

            if (enemy.playerCounters.invisibility == 0)
            {
                for (int i = 0; i < permCards.Count; i++)
                {
                    if (!permCards[i].innate.Contains("immaterial") && permCards[i].DefNow < card.DefNow)
                    {
                        enemy.creatureDisplayers[permIds[i].Index].ShouldShowTarget(true);
                        DuelManager.validTargets.Add(permIds[i]);
                    }
                }
            }
        }
        if(card.skill == "endow")
        {
            List<ID> permIds = owner.playerCreatureField.GetAllIds();
            List<Card> permCards = owner.playerCreatureField.GetAllCards();

            if (owner.playerCounters.invisibility == 0)
            {
                for (int i = 0; i < permCards.Count; i++)
                {
                    if (!permCards[i].innate.Contains("immaterial") && CardDatabase.weaponIdList.Contains(permCards[i].iD))
                    {
                        owner.creatureDisplayers[permIds[i].Index].ShouldShowTarget(true);
                        DuelManager.validTargets.Add(permIds[i]);
                    }
                }
            }
            if (!owner.playerPassiveManager.GetWeapon().innate.Contains("immaterial") && owner.playerPassiveManager.GetWeapon().skill != "none")
            {
                owner.passiveDisplayers[1].ShouldShowTarget(true);
                DuelManager.validTargets.Add(owner.playerPassiveManager.GetWeaponID());
            }
        }
        if(card.skill == "web")
        {
            List<ID> permIds = owner.playerCreatureField.GetAllIds();
            List<Card> permCards = owner.playerCreatureField.GetAllCards();

            if (owner.playerCounters.invisibility == 0)
            {
                for (int i = 0; i < permCards.Count; i++)
                {
                    if (!permCards[i].innate.Contains("immaterial") && !permCards[i].innate.Contains("burrow") && permCards[i].innate.Contains("airborne"))
                    {
                        owner.creatureDisplayers[permIds[i].Index].ShouldShowTarget(true);
                        DuelManager.validTargets.Add(permIds[i]);
                    }
                }
            }
            
            permIds = enemy.playerCreatureField.GetAllIds();
            permCards = enemy.playerCreatureField.GetAllCards();

            if (enemy.playerCounters.invisibility == 0)
            {
                for (int i = 0; i < permCards.Count; i++)
                {
                    if (!permCards[i].innate.Contains("immaterial") && !permCards[i].innate.Contains("burrow") && permCards[i].innate.Contains("airborne"))
                    {
                        enemy.creatureDisplayers[permIds[i].Index].ShouldShowTarget(true);
                        DuelManager.validTargets.Add(permIds[i]);
                    }
                }
            }
        }
        if (card.skill == "nymph")
        {
            List<ID> permIds = owner.playerPermanentManager.GetAllIds();
            List<Card> permCards = owner.playerPermanentManager.GetAllCards();

            for (int i = 0; i < permCards.Count; i++)
            {
                if (permCards[i].cardType.Equals(CardType.Pillar) && !permCards[i].innate.Contains("immaterial"))
                {
                    owner.permanentDisplayers[permIds[i].Index].ShouldShowTarget(true);
                    DuelManager.validTargets.Add(permIds[i]);
                }
            }
        }

        if (card.skill == "immolate" || card.skill == "cremation" || card.skill == "catapult")
        {
            List<ID> permIds = owner.playerCreatureField.GetAllIds();
            List<Card> permCards = owner.playerCreatureField.GetAllCards();

            if (owner.playerCounters.invisibility == 0)
            {
                for (int i = 0; i < permCards.Count; i++)
                {
                    if (!permCards[i].innate.Contains("immaterial") && !permCards[i].innate.Contains("burrow"))
                    {
                        owner.creatureDisplayers[permIds[i].Index].ShouldShowTarget(true);
                        DuelManager.validTargets.Add(permIds[i]);
                    }
                }
            }
        }

        if (card.skill == "rage" || card.skill == "berserk" || card.skill == "petrify"
            || card.skill == "momentum" || card.skill == "acceleration" || card.skill == "parallel universe"
            || card.skill == "gravity pull" || card.skill == "chaos power" || card.skill == "reverse time"
            || card.skill == "freeze" || card.skill == "congeal" || card.skill == "immortality"
            || card.skill == "adrenaline" || card.skill == "antimatter" || card.skill == "overdrive"
            || card.skill == "chaos" || card.skill == "shockwave" || card.skill == "mutation"
            || card.skill == "improve" || card.skill == "liquid shadow" || card.skill == "infect"
            || card.skill == "infection" || card.skill == "blessing" || card.skill == "lobotomize"
            || card.skill == "aflatoxin" || card.skill == "readiness" || card.skill == "nightmare"
            || card.skill == "armor" || card.skill == "fractal" || card.skill == "heal" 
            || card.skill == "lightning" || card.skill == "drain life" || card.skill == "holy light"
            || card.skill == "purify" || card.skill == "icebolt" || card.skill == "fire bolt"
            || card.skill == "heavy armor" || card.skill == "sniper" || card.skill == "guard")
        {
            List<ID> permIds = owner.playerCreatureField.GetAllIds();
            List<Card> permCards = owner.playerCreatureField.GetAllCards();

            if(owner.playerCounters.invisibility == 0)
            {
                for (int i = 0; i < permCards.Count; i++)
                {
                    if (!permCards[i].innate.Contains("immaterial") && !permCards[i].innate.Contains("burrow"))
                    {
                        owner.creatureDisplayers[permIds[i].Index].ShouldShowTarget(true);
                        DuelManager.validTargets.Add(permIds[i]);
                    }
                }
            }
            permIds = enemy.playerCreatureField.GetAllIds();
            permCards = enemy.playerCreatureField.GetAllCards();

            if (enemy.playerCounters.invisibility == 0)
            {
                for (int i = 0; i < permCards.Count; i++)
                {
                    if (!permCards[i].innate.Contains("immaterial") && !permCards[i].innate.Contains("burrow"))
                    {
                        enemy.creatureDisplayers[permIds[i].Index].ShouldShowTarget(true);
                        DuelManager.validTargets.Add(permIds[i]);
                    }
                }
            }
        }

        if (card.skill == "drain life" || card.skill == "lightning" || card.skill == "purify" 
            || card.skill == "holy light" || card.skill == "fire bolt" || card.skill == "icebolt")
        {
            enemy.playerDisplayer.ShouldShowTarget(true);
            DuelManager.validTargets.Add(enemy.playerDisplayer.GetObjectID());
            owner.playerDisplayer.ShouldShowTarget(true);
            DuelManager.validTargets.Add(owner.playerDisplayer.GetObjectID());
        }

        if(card.skill == "earthquake" || card.skill == "destroy" || card.skill == "steal" || card.skill == "accretion" || card.skill == "enchant")
        {
            if(enemy.playerCounters.invisibility == 0)
            {
                List<ID> permIds = enemy.playerPermanentManager.GetAllIds();
                List<Card> permCards = enemy.playerPermanentManager.GetAllCards();

                for (int i = 0; i < permCards.Count; i++)
                {
                    if (!permCards[i].innate.Contains("immaterial"))
                    {
                        enemy.permanentDisplayers[permIds[i].Index].ShouldShowTarget(true);
                        DuelManager.validTargets.Add(permIds[i]);
                    }
                }

            }

            if (owner.playerCounters.invisibility == 0)
            {
                List<ID> permIds = owner.playerPermanentManager.GetAllIds();
                List<Card> permCards = owner.playerPermanentManager.GetAllCards();

                for (int i = 0; i < permCards.Count; i++)
                {
                    if (!permCards[i].innate.Contains("immaterial"))
                    {
                        owner.permanentDisplayers[permIds[i].Index].ShouldShowTarget(true);
                        DuelManager.validTargets.Add(permIds[i]);
                    }
                }

            }
        }

        if (card.skill == "destroy" || card.skill == "steal" || card.skill == "accretion" || card.skill == "enchant")
        {
            if (enemy.playerCounters.invisibility == 0)
            {
                if (!enemy.playerPassiveManager.GetWeapon().innate.Contains("immaterial") && enemy.playerPassiveManager.GetWeapon().skill != "none")
                {
                    enemy.passiveDisplayers[1].ShouldShowTarget(true);
                    DuelManager.validTargets.Add(enemy.playerPassiveManager.GetWeaponID());
                }
                if (!enemy.playerPassiveManager.GetShield().innate.Contains("immaterial") && enemy.playerPassiveManager.GetShield().skill != "none")
                {
                    enemy.passiveDisplayers[2].ShouldShowTarget(true);
                    DuelManager.validTargets.Add(enemy.playerPassiveManager.GetWeaponID());
                }
            }

            if (owner.playerCounters.invisibility == 0)
            {
                if (!owner.playerPassiveManager.GetWeapon().innate.Contains("immaterial") && owner.playerPassiveManager.GetWeapon().skill != "none")
                {
                    owner.passiveDisplayers[1].ShouldShowTarget(true);
                    DuelManager.validTargets.Add(enemy.playerPassiveManager.GetWeaponID());
                }
                if (!owner.playerPassiveManager.GetShield().innate.Contains("immaterial") && owner.playerPassiveManager.GetShield().skill != "none")
                {
                    owner.passiveDisplayers[2].ShouldShowTarget(true);
                    DuelManager.validTargets.Add(enemy.playerPassiveManager.GetWeaponID());
                }
            }
        }

    }
    public IEnumerator SkillRoutineWithTarget(PlayerManager targetOwner, Card targetCard, ID targetId)
    {
        Card card = BattleVars.shared.cardOnStandBy;
        PlayerManager originOwner = DuelManager.GetIDOwner(BattleVars.shared.originId);
        if (card.skill == "butterfly")
        {
            targetCard.skill = "destroy";
            targetCard.skillCost = 3;
            targetCard.skillElement = Element.Entropy;
            targetCard.desc = "<sprite=6><sprite=6><sprite=6>: Destroy: \n Destroy the targeted permanent";
            targetOwner.DisplayNewCard(targetId, targetCard);
        }
        if (card.skill == "wisdom")
        {
            targetCard.passive.Add("psion");
            targetCard.desc = $"{targetCard.cardName}'s attacks deal spell damage.";
            targetOwner.DisplayNewCard(targetId, targetCard);
        }
        if (card.skill == "paradox")
        {
            yield return targetOwner.StartCoroutine(targetOwner.RemoveCardFromFieldLogic(targetId));
        }
        if (card.skill == "endow")
        {
            card.skill = targetCard.skill;
            card.skillCost = targetCard.skillCost;
            card.skillElement = targetCard.skillElement;
            card.AtkModify += targetCard.AtkNow;
            card.DefModify += targetCard.DefNow;
            targetOwner.DisplayNewCard(BattleVars.shared.originId, card);
        }
        if (card.skill == "web")
        {
            targetCard.innate.Remove("airborne");
            targetOwner.DisplayNewCard(targetId, targetCard);
        }
        if (card.skill == "nymph")
        {
            originOwner.PlayCardOnFieldLogic(card.iD.IsUpgraded() ? CardDatabase.GetRandomEliteNymph(targetCard.costElement) : CardDatabase.GetRandomRegularNymph(targetCard.costElement));
            yield return targetOwner.StartCoroutine(targetOwner.RemoveCardFromFieldLogic(targetId));
        }

        if (card.skill == "immolate" || card.skill == "cremation")
        {

            yield return targetOwner.StartCoroutine(targetOwner.RemoveCardFromFieldLogic(targetId, 1, true));
            for (int i = 0; i < 12; i++)
            {
                targetOwner.StartCoroutine(targetOwner.GenerateQuantaLogic((Element)i, 1));
                yield return null;
            }

            targetOwner.StartCoroutine(targetOwner.GenerateQuantaLogic(Element.Fire, card.skill == "cremation" ? 7 : 5));
            yield return null;
        }

        if (card.skill == "berserk" || card.skill == "rage")
        {
            targetCard.AtkModify += card.skill == "rage" ? 5 : 6;
            targetCard.DefModify += card.skill == "rage" ? -5 : -6;
            targetOwner.DisplayNewCard(targetId, targetCard);
        }

        if (card.skill == "icebolt")
        {
            int waterQ = originOwner.GetAllQuantaOfElement(Element.Water);
            int damageToDeal = 2 + (Mathf.FloorToInt(waterQ / 10) * 2);
            bool willFreeze = Random.Range(0, 100) > 30 + (damageToDeal * 5);

            if (targetCard == null)
            {
                targetOwner.StartCoroutine(targetOwner.ModifyHealthLogic(damageToDeal, true, true));
                if (willFreeze)
                {
                    targetOwner.playerCounters.freeze = 3;
                }
            }
            else
            {
                targetCard.DefDamage += damageToDeal;
                if (willFreeze)
                {
                    targetCard.Freeze = 3;
                }
                targetOwner.DisplayNewCard(targetId, targetCard);
            }
        }
        if (card.skill == "drain life")
        {
            int darkQ = originOwner.GetAllQuantaOfElement(Element.Darkness);
            int damageToDeal = 2 + (Mathf.FloorToInt(darkQ / 10) * 2);

            if (targetCard == null)
            {
                targetOwner.StartCoroutine(targetOwner.ModifyHealthLogic(damageToDeal, true, true));
                originOwner.StartCoroutine(originOwner.ModifyHealthLogic(damageToDeal, false, false));
            }
            else
            {
                originOwner.StartCoroutine(originOwner.ModifyHealthLogic(targetCard.DefNow < damageToDeal ? targetCard.DefNow : damageToDeal, false, false));

                targetCard.DefDamage += damageToDeal;

                targetOwner.DisplayNewCard(targetId, targetCard);
            }
        }

        if (card.skill == "fire bolt")
        {
            int waterQ = originOwner.GetAllQuantaOfElement(Element.Fire);
            int damageToDeal = 2 + (Mathf.FloorToInt(waterQ / 10) * 2);

            if (targetCard == null)
            {
                targetOwner.StartCoroutine(targetOwner.ModifyHealthLogic(damageToDeal, true, true));
            }
            else
            {
                targetCard.DefDamage += damageToDeal;
                targetOwner.DisplayNewCard(targetId, targetCard);
            }
        }
        if (card.skill == "sniper")
        {
            targetCard.DefDamage += 3;
            targetOwner.DisplayNewCard(targetId, targetCard);
        }
        if (card.skill == "lightning")
        {
            if (targetCard == null)
            {
                targetOwner.StartCoroutine(targetOwner.ModifyHealthLogic(5, true, true));
            }
            else
            {

                targetCard.DefDamage += 5;

                targetOwner.DisplayNewCard(targetId, targetCard);
            }
        }
        if (card.skill == "immortality" || card.skill == "enchant")
        {
            targetCard.innate.Add("immaterial");
            targetOwner.DisplayNewCard(targetId, targetCard);
        }
        if (card.skill == "acceleration")
        {
            targetCard.desc = "Acceleration: \n Gain +2 /-1 per turn";
            targetCard.skill = "";
            targetCard.passive.Add(card.skill);
            targetOwner.DisplayNewCard(targetId, targetCard);
        }
        if(card.skill == "overdrive")
        {
            targetCard.desc = "Overdrive: \n Gain +3 /-1 per turn";
            targetCard.skill = "";
            targetCard.passive.Add(card.skill);
            targetOwner.DisplayNewCard(targetId, targetCard);
        }
        if (card.skill == "adrenaline" || card.skill == "antimatter"
             || card.skill == "momentum" || card.skill == "gravity pull")
        {

            targetCard.passive.Add(card.skill);
            targetOwner.DisplayNewCard(targetId, targetCard);
        }

        if (card.skill == "lobotomize")
        {
            targetCard.skill = "";
            targetCard.desc = "";
            targetCard.passive.Clear();
            targetOwner.DisplayNewCard(targetId, targetCard);
        }
        if (card.skill == "blessing" || card.skill == "chaos power")
        {
            int mod = card.skill == "blessing" ? 3 : Random.Range(1, 6);

            targetCard.DefModify += mod;
            targetCard.AtkModify += mod;
            targetOwner.DisplayNewCard(targetId, targetCard);
        }
        if (card.skill == "heavy armor" || card.skill == "armor")
        {

            targetCard.DefModify += card.skill == "heavy armor" ? 6 : 3;
            targetOwner.DisplayNewCard(targetId, targetCard);
        }
        if (card.skill == "liquid shadow")
        {
            targetCard.skill = "";
            targetCard.desc = "";
            targetCard.passive.Clear();
            targetCard.passive.Add("vampire");
            targetCard.Poison++;
            targetOwner.DisplayNewCard(targetId, targetCard);
        }
        if (card.skill == "parallel universe")
        {
            originOwner.PlayCardOnFieldLogic(new (targetCard));
        }
        if (card.skill == "freeze" || card.skill == "congeal")
        {
            targetCard.Freeze = card.skill == "freeze" ? 3 : 4;
            targetOwner.DisplayNewCard(targetId, targetCard);
        }
        if (card.skill == "shockwave")
        {
            if (targetCard.Freeze > 0)
            {
                targetOwner.StartCoroutine(targetOwner.RemoveCardFromFieldLogic(targetId));
            }
            else
            {
                targetCard.DefDamage += 4;
                targetOwner.DisplayNewCard(targetId, targetCard);
            }
        }
        if (card.skill == "infect" || card.skill == "infection")
        {
            targetCard.Poison++;
            targetOwner.DisplayNewCard(targetId, targetCard);
            if (card.skill == "infect") { originOwner.StartCoroutine(originOwner.RemoveCardFromFieldLogic(BattleVars.shared.originId)); }
        }
        if (card.skill == "holy light")
        {
            if (targetCard == null)
            {
                targetOwner.StartCoroutine(targetOwner.ModifyHealthLogic(10, false, false));
            }
            else
            {
                if (targetCard.costElement.Equals(Element.Death) || targetCard.costElement.Equals(Element.Darkness))
                {
                    targetCard.DefDamage += 10;
                }
                else
                {
                    targetCard.DefDamage -= 10;
                    if (targetCard.DefDamage < 0) { targetCard.DefDamage = 0; }
                }
                targetOwner.DisplayNewCard(targetId, targetCard);
            }
        }
        if (card.skill == "heal")
        {
            targetCard.DefDamage -= 5;
            if (targetCard.DefDamage < 0) { targetCard.DefDamage = 0; }
            targetOwner.DisplayNewCard(targetId, targetCard);
        }
        if (card.skill == "nightmare")
        {
            PlayerManager player = DuelManager.player;
            PlayerManager ai = DuelManager.enemy;
            Card creature = CardDatabase.GetCardFromId(targetCard.iD);
            if (originOwner.isPlayer)
            {
                int damage = 7 - ai.GetHandCards().Count;
                ai.FillHandWith(creature);
                yield return ai.StartCoroutine(ai.ModifyHealthLogic(damage * 2, true, true));
                yield return player.StartCoroutine(player.ModifyHealthLogic(damage * 2, false, true));
            }
            else
            {
                int damage = 7 - player.GetHandCards().Count;
                player.FillHandWith(creature);
                yield return ai.StartCoroutine(ai.ModifyHealthLogic(damage * 2, false, true));
                yield return player.StartCoroutine(player.ModifyHealthLogic(damage * 2, true, true));
            }
        }
        if(card.skill == "mitosiss")
        {
            targetCard.skill = "mitosis";
            targetCard.desc = "Mitosis: \n Generate a daughter creature";
            targetCard.skillCost = targetCard.cost;
            targetCard.skillElement = targetCard.costElement;
            targetOwner.DisplayNewCard(targetId, targetCard);
        }
        if(card.skill == "fractal")
        {
            originOwner.FillHandWith(CardDatabase.GetCardFromId(targetCard.iD));
            yield return originOwner.StartCoroutine(originOwner.SpendQuantaLogic(Element.Aether, 75));
        }
        if(card.skill == "aflatoxin")
        {
            targetCard.IsAflatoxin = true;
            targetOwner.DisplayNewCard(targetId, targetCard);
        }
        if(card.skill == "reverse time")
        {
            if (targetCard.innate.Contains("mummy"))
            {
                Card pharoah = CardDatabase.GetCardFromId(targetCard.iD.IsUpgraded() ? "7qc" : "5rs");
                targetOwner.DisplayNewCard(targetId, pharoah);
            }
            else if (targetCard.innate.Contains("undead"))
            {
                Card rndCreature = targetCard.iD.IsUpgraded() ? CardDatabase.GetRandomEliteCreature() : CardDatabase.GetRandomCreature();
                targetOwner.DisplayNewCard(targetId, rndCreature);
            }
            else
            {
                Card baseCreature = CardDatabase.GetCardFromId(targetCard.iD);
                targetOwner.AddCardToDeck(baseCreature);

                yield return targetOwner.StartCoroutine(targetOwner.RemoveCardFromFieldLogic(targetId, 1, false));
            }
        }
        if(card.skill == "devour")
        {
            card.AtkModify++;
            card.DefModify++;
            if (targetCard.innate.Contains("poisonous"))
            {
                card.Poison++;
            }
            originOwner.DisplayNewCard(BattleVars.shared.originId, card);
            yield return targetOwner.StartCoroutine(targetOwner.RemoveCardFromFieldLogic(targetId));
        }

        if(card.skill == "steal")
        {
            Card cardToPlay = new (targetCard);
            originOwner.PlayCardOnFieldLogic(cardToPlay);
            Game_AnimationManager.shared.StartAnimation("Steal", Battlefield_ObjectIDManager.shared.GetObjectFromID(targetId));

            yield return targetOwner.StartCoroutine(targetOwner.RemoveCardFromFieldLogic(targetId, 1, false));
        }

        if(card.skill == "purify")
        {
            if(targetCard != null)
            {
                targetCard.IsAflatoxin = false;
                if (targetCard.Poison > 0)
                {
                    targetCard.Poison = -2;
                }
                else
                {
                    targetCard.Poison -= 2;
                }
                targetOwner.DisplayNewCard(targetId, targetCard);
            }
            else
            {
                targetOwner.sacrificeCount = 0;
                targetOwner.playerCounters.nuerotoxin = 0;

                if (targetOwner.playerCounters.poison > 0)
                {
                    targetOwner.playerCounters.poison = -2;
                }
                else
                {
                    targetOwner.playerCounters.poison -= 2;
                }
            }
        }
        if(card.skill == "earthquake")
        {
            yield return targetOwner.StartCoroutine(targetOwner.RemoveCardFromFieldLogic(targetId, 3));
        }

        if (card.skill == "destroy")
        {
            yield return targetOwner.StartCoroutine(targetOwner.RemoveCardFromFieldLogic(targetId));
        }

        if (card.skill == "accretion")
        {
            yield return targetOwner.StartCoroutine(targetOwner.RemoveCardFromFieldLogic(targetId));

            card.DefModify += 15;

            if (card.DefNow >= 45)
            {
                originOwner.playerHand.AddCardToHand(card.iD.IsUpgraded() ? CardDatabase.GetCardFromId("74f") : CardDatabase.GetCardFromId("55v"));
                yield return originOwner.StartCoroutine(originOwner.RemoveCardFromFieldLogic(BattleVars.shared.originId));
                yield break;
            }

            originOwner.DisplayNewCard(BattleVars.shared.originId, card);
        }

        if (card.skill == "improve")
        {
            Game_AnimationManager.shared.StartAnimation("Mutation", Battlefield_ObjectIDManager.shared.GetObjectFromID(targetId));
            Card mutant = CardDatabase.GetMutant(targetCard.iD.IsUpgraded());
            targetOwner.DisplayNewCard(targetId, mutant);
        }
        if(card.skill == "readiness")
        {
            targetCard.skillCost = 0;
            targetCard.passive.Add("readiness");
        }
        if(card.skill == "catapult")
        {
            int damage = 100 * targetCard.DefNow / (100 + targetCard.DefNow);
            if (targetCard.Freeze > 0)
            {
                damage += (int)(damage * 0.5f);
            }
            yield return targetOwner.StartCoroutine(targetOwner.RemoveCardFromFieldLogic(targetId));
            yield return DuelManager.GetNotIDOwner(targetId).StartCoroutine(DuelManager.GetNotIDOwner(targetId).ModifyHealthLogic(damage, true, false));
        }
        if(card.skill == "mutation")
        {
            switch (GetMutationResult())
            {
                case MutationEnum.Kill:
                    Game_AnimationManager.shared.StartAnimation("Mutation", Battlefield_ObjectIDManager.shared.GetObjectFromID(targetId));
                    yield return targetOwner.StartCoroutine(targetOwner.RemoveCardFromFieldLogic(targetId));
                    break;
                case MutationEnum.Mutate:
                    Game_AnimationManager.shared.StartAnimation("Mutation", Battlefield_ObjectIDManager.shared.GetObjectFromID(targetId));
                    Card mutant = CardDatabase.GetMutant(targetCard.iD.IsUpgraded());
                    targetOwner.DisplayNewCard(targetId, mutant);
                    break;
                case MutationEnum.Abomination:
                    Game_AnimationManager.shared.StartAnimation("Mutation", Battlefield_ObjectIDManager.shared.GetObjectFromID(targetId));
                    Card abomination = CardDatabase.GetCardFromId(targetCard.iD.IsUpgraded() ? "6tu" : "4ve");
                    targetOwner.DisplayNewCard(targetId, abomination);
                    break;
                default:
                    break;
            }
        }
        if(card.skill == "guard")
        {
            targetCard.innate.Add("delay");
            card.innate.Add("delay");
            if (!targetCard.innate.Contains("airborne"))
            {
                targetCard.DefDamage += card.AtkNow; 
            }
            originOwner.DisplayNewCard(BattleVars.shared.originId, card);
            targetOwner.DisplayNewCard(targetId, targetCard);
        }
        if(card.skill == "petrify")
        {
            targetCard.innate.Add("delay");
            targetCard.innate.Add("delay");
            targetCard.innate.Add("delay");
            targetCard.innate.Add("delay");
            targetCard.innate.Add("delay");
            targetCard.innate.Add("delay");
            targetCard.DefModify += 20;
            targetOwner.DisplayNewCard(targetId, targetCard);
        }
        if (card.skill == "chaos")
        {
            yield return originOwner.StartCoroutine(ChoasSeed(targetOwner, targetCard, targetId));
        }
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
                targetOwner.DisplayNewCard(targetId, targetCard);
                break;
            case 1:
                targetCard.DefDamage += 5;
                targetOwner.DisplayNewCard(targetId, targetCard);
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
                targetOwner.DisplayNewCard(targetId, targetCard);
                break;
            case 3:
                waterQ = originOwner.GetAllQuantaOfElement(Element.Fire);
                damageToDeal = 2 + (Mathf.FloorToInt(waterQ / 10) * 2);

                targetCard.DefDamage += damageToDeal;
                targetOwner.DisplayNewCard(targetId, targetCard);

                break;
            case 4:
                originOwner.PlayCardOnFieldLogic(new (targetCard));
                break;
            case 5:
                originOwner.PlayCardOnFieldLogic(new (targetCard));
                break;
            case 6:
                targetCard.skill = "";
                targetCard.desc = "";
                targetCard.passive.Clear();
                targetOwner.DisplayNewCard(targetId, targetCard);
                break;
            case 7:
                Card cardToPlay = new (targetCard);
                originOwner.PlayCardOnFieldLogic(cardToPlay);
                Game_AnimationManager.shared.StartAnimation("Steal", Battlefield_ObjectIDManager.shared.GetObjectFromID(targetId));

                yield return targetOwner.StartCoroutine(targetOwner.RemoveCardFromFieldLogic(targetId));
                break;
            case 8:
                targetCard.DefDamage += 3;
                targetOwner.DisplayNewCard(targetId, targetCard);
                break;
            case 9:
                if (targetCard.Freeze > 0)
                {
                    targetOwner.StartCoroutine(targetOwner.RemoveCardFromFieldLogic(targetId));
                }
                else
                {
                    targetCard.DefDamage += 4;
                    targetOwner.DisplayNewCard(targetId, targetCard);
                }
                break;
            case 10:
                if (targetCard.innate.Contains("mummy"))
                {
                    Card pharoah = CardDatabase.GetCardFromId(targetCard.iD.IsUpgraded() ? "7qc" : "5rs");
                    targetOwner.DisplayNewCard(targetId, pharoah);
                }
                else if (targetCard.innate.Contains("undead"))
                {
                    Card rndCreature = targetCard.iD.IsUpgraded() ? CardDatabase.GetRandomEliteCreature() : CardDatabase.GetRandomCreature();
                    targetOwner.DisplayNewCard(targetId, rndCreature);
                }
                else
                {
                    Card baseCreature = CardDatabase.GetCardFromId(targetCard.iD);
                    targetOwner.AddCardToDeck(baseCreature);

                    yield return targetOwner.StartCoroutine(targetOwner.RemoveCardFromFieldLogic(targetId, 1, false));
                }
                break;
            case 11:
                card.passive.Add("gravity pull");
                targetOwner.DisplayNewCard(targetId, card, true);
                break;
            default:
                break;
        }
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
}
