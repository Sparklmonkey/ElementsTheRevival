using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInPlay : MonoBehaviour
{
    private void Awake()
    {
        IndexOnField = int.Parse(name.Split("_")[1]);
    }

    private EffectBody _effectBody;
    private ActiveCardDisplay _cardDisplay;
    private PlayerManager _owner;
    private int _turnsInPlay = 0;
    private Card _activeCard { get; set; }

    public Card ActiveCard { get { return _activeCard; } }
    public int IndexOnField;
    public IEnumerator ActivateCard(Card cardToPlay, PlayerManager owner)
    {
        _owner = owner;
        _effectBody = new EffectBody();
        _activeCard = new(cardToPlay); 
        switch (_activeCard.iD)
        {
            case "7q9":
            case "5rp":
                _turnsInPlay = 1;
                break;
            case "7n8":
            case "5oo":
                _turnsInPlay = 5;
                break;
            case "61t":
            case "80d":
            case "5v2":
            case "7ti":
                _turnsInPlay = 3;
                break;
            default:
                break;
        }

        switch (_activeCard.skill)
        {
            case "eclipse":
            case "nightfall":
                DuelManager.Instance.UpdateNightFallEclipse(true, _activeCard.skill);
                break;
            case "bones":
                _owner.AddPlayerCounter(PlayerCounters.Bone, BattleVars.shared.cardOnStandBy == null ? 7 : 1);
                break;
            case "patience":
                _owner.AddPlayerCounter(PlayerCounters.Patience, 1);
                break;
            case "freedom":
                _owner.AddPlayerCounter(PlayerCounters.Freedom, 1);
                break;
            case "cloak":
                _owner.AddPlayerCounter(PlayerCounters.Invisibility, 3);
                break;
            case "flood":
                DuelManager.Instance.AddFloodCount(1);
                break;
            default:
                break;
        }

        if (_activeCard.innate.Contains("swarm"))
        {
            _owner.AddPlayerCounter(PlayerCounters.Scarab, 1);
        }
        if (_activeCard.innate.Contains("chimera"))
        {
            (int, int) chimeraPwrHP = _owner.GetChimeraStats();

            _activeCard.AtkModify += chimeraPwrHP.Item1;
            _activeCard.DefModify += chimeraPwrHP.Item2;
        }
        if (_activeCard.passive.Contains("sanctuary"))
        {
            _owner.AddPlayerCounter(PlayerCounters.Sanctuary, 1);
        }

        if (_activeCard.passive.Contains("scavenger") 
            || _activeCard.skill == "soul catch" 
            || _activeCard.skill == "boneyard" 
            || _activeCard.skill == "bones")
        {
            DuelManager.Instance.RegisterDeathTrigger(Event_DeathTrigger, true);
        }
        yield break;
    }

    public IEnumerator RemoveCardFromPlay()
    {
        bool removeCard = true;
        switch (_activeCard.skill)
        {
            case "eclipse":
            case "nightfall":
                DuelManager.Instance.player.CheckEclipseNightfall(false, _activeCard.skill);
                DuelManager.Instance.enemy.CheckEclipseNightfall(false, _activeCard.skill);
                break;
            case "cloak":
                _owner.AddPlayerCounter(PlayerCounters.Invisibility, -1);
                break;
            case "patience":
                _owner.AddPlayerCounter(PlayerCounters.Patience, -1);
                break;
            case "freedom":
                _owner.AddPlayerCounter(PlayerCounters.Freedom, -1);
                break;
            case "flood":
                DuelManager.Instance.AddFloodCount(-1);
                break;
            default:
                break;
        }

        if (_effectBody.IsAflatoxin)
        {
            yield return StartCoroutine(ActivateCard(CardDatabase.Instance.GetCardFromId("6ro"), _owner));
            removeCard = false;
        }
        if (_activeCard.passive.Contains("phoenix"))
        {
            bool shouldRebirth = BattleVars.shared.cardOnStandBy?.skill == "reverse time";
            if (shouldRebirth)
            {
                _activeCard = CardDatabase.Instance.GetCardFromId(_activeCard.iD.IsUpgraded() ? "7dt" : "5fd");
                removeCard = false;
            }
        }

        if (_activeCard.innate.Contains("swarm"))
        {
            _owner.AddPlayerCounter(PlayerCounters.Scarab, -1);
        }

        if (_activeCard.passive.Contains("sanctuary"))
        {
            _owner.AddPlayerCounter(PlayerCounters.Sanctuary, -1);
        }

        bool shouldActivateDeath = BattleVars.shared.cardOnStandBy?.skill != "reverse time" && _activeCard.cardType.Equals(CardType.Creature);
        if (shouldActivateDeath)
        {
            yield return StartCoroutine(DuelManager.Instance.player.ActivateDeathTriggers(_activeCard.cardName.Contains("Skeleton")));
            yield return StartCoroutine(DuelManager.Instance.enemy.ActivateDeathTriggers(_activeCard.cardName.Contains("Skeleton")));
        }

        if (removeCard)
        {
            if (_activeCard.passive.Contains("scavenger")
                || _activeCard.skill == "soul catch"
                || _activeCard.skill == "boneyard"
                || _activeCard.skill == "bones")
            {
                DuelManager.Instance.RegisterDeathTrigger(Event_DeathTrigger, false);
            }
            _activeCard = null;
            _owner = null;
        }

        //playerCreatureField.DestroyCard(cardID.Index);
        //creatureDisplayers[cardID.Index].PlayDissolveAnimation();
        //break;
    }

    public void ModifyEffect()
    {

    }

    //Event Triggers

    public IEnumerator Event_DeathTrigger(MonoBehaviour sender, EventUtility.ActiveCardArgs args)
    {

        if (_activeCard.passive.Contains("scavenger"))
        {
            _activeCard.AtkModify += 1;
            _activeCard.DefModify += 1;
        }

        if (_activeCard.skill == "soul catch")
        {
            yield return StartCoroutine(_owner.GenerateQuantaLogic(Element.Death, _activeCard.iD.IsUpgraded() ? 3 : 2));
        }
        if (_activeCard.skill == "boneyard" && !args.isSkeletonDeath)
        {
            _owner.PlayCardOnFieldLogic(CardDatabase.Instance.GetCardFromId(_activeCard.iD.IsUpgraded() ? "716" : "52m"));
        }

        if (_activeCard.skill == "bones" && !args.isSkeletonDeath)
        {
            _owner.AddPlayerCounter(PlayerCounters.Bone, 2);
        }
        yield break;
    }

    public IEnumerator Event_CreatureAttack(MonoBehaviour sender, EventUtility.ActiveCardArgs args)
    {
        if (ActiveCard.AtkNow != 0)
        {
            int adrCount = ActiveCard.passive.Contains("adrenaline") ? DuelManager.AdrenalineDamageList[Mathf.Abs(ActiveCard.AtkNow) - 1].Count : 1;
            int atkMod = ActiveCard.passive.Contains("dive") ? ActiveCard.AtkNow * 2 : ActiveCard.AtkNow;

            for (int i = 0; i < adrCount; i++)
            {
                if (ActiveCard.Freeze == 0 && !ActiveCard.IsDelayed)
                {
                    bool isFreedomEffect = Random.Range(0, 100) < (25 * _owner.freedomCount) && ActiveCard.innate.Contains("airborne");

                    if (atkMod != 0)
                    {
                        //Gravity Check
                        //args.enemy.ShieldCheck();
                        // Shield Check


                    }
                }
            }
        }
    }
    //public IEnumerator Event_EndTurnEffect(MonoBehaviour sender, EventUtility.ActiveCardArgs args)
    //{

    //}
    //public IEnumerator Event_CreatureEndTurn(MonoBehaviour sender, EventUtility.CreatureEventArgs args)
    //{
    
    //        for (int i = 0; i < adrenalineAtkCount; i++)
    //        {
    //            if (Freeze == 0 || !IsDelayed)
    //            {
    //                if (atkNow > 0 && !isFreedomEffect)
    //                {
    //                    //Do Gravity Check

    //                    //Do Shield Check

    //                    if (passive.Contains("nuerotoxin"))
    //                    {
    //                        args.enemy.playerCounters.nuerotoxin = 1;
    //                        args.enemy.UpdatePlayerIndicators();
    //                    }
    //                    if (atkNow != 0)
    //                    {
    //                        Game_SoundManager.shared.PlayAudioClip("CreatureDamage");

    //                        if (passive.Contains("venom"))
    //                        {
    //                            args.enemy.playerCounters.poison++;
    //                        }
    //                        if (passive.Contains("deadly venom"))
    //                        {
    //                            args.enemy.playerCounters.poison += 2;
    //                        }
    //                        if (passive.Contains("nuerotoxin"))
    //                        {
    //                            args.enemy.playerCounters.nuerotoxin = 1;
    //                        }
    //                        args.enemy.UpdatePlayerIndicators();
    //                        if (passive.Contains("vampire"))
    //                        {
    //                            args.owner.StartCoroutine(args.owner.ModifyHealthLogic(atkNow, false, false));
    //                        }

    //                        //Show Creature Attack
    //                        //StartCoroutine(creatureDisplayers[iD.Index].ShowDamage(atkNow));

    //                        //Send Damage
    //                        //yield return StartCoroutine(opponent.ReceivePhysicalDamage(isFreedomEffect ? Mathf.FloorToInt(atkNow * 1.5f) : atkNow));
    //                    }
    //                }

    //            }
    //        }
    //    }



    //    for (int i = 0; i < creatureCards.Count; i++)
    //    {

    //        skipCreatureAttack:
    //        if (adrenalineIndex < 2)
    //        {

    //            creature.AbilityUsed = false;
    //            if (creature.passive.Contains("dive"))
    //            {
    //                creature.passive.Remove("dive");
    //                creature.atk /= 2;
    //                creature.AtkModify /= 2;
    //            }

    //            if (!creature.IsDelayed
    //                && creature.passive?.Count > 0)
    //            {
    //                if (creature.passive.Contains("air"))
    //                {
    //                    Game_AnimationManager.shared.StartAnimation("QuantaGenerate", Battlefield_ObjectIDManager.shared.GetObjectFromID(creatureIds[i]), Element.Air);
    //                    StartCoroutine(GenerateQuantaLogic(Element.Air, 1));
    //                }
    //                if (creature.passive.Contains("earth"))
    //                {
    //                    Game_AnimationManager.shared.StartAnimation("QuantaGenerate", Battlefield_ObjectIDManager.shared.GetObjectFromID(creatureIds[i]), Element.Earth);
    //                    StartCoroutine(GenerateQuantaLogic(Element.Earth, 1));
    //                }
    //                if (creature.passive.Contains("fire"))
    //                {
    //                    Game_AnimationManager.shared.StartAnimation("QuantaGenerate", Battlefield_ObjectIDManager.shared.GetObjectFromID(creatureIds[i]), Element.Fire);
    //                    StartCoroutine(GenerateQuantaLogic(Element.Fire, 1));
    //                }
    //                if (creature.passive.Contains("light"))
    //                {
    //                    Game_AnimationManager.shared.StartAnimation("QuantaGenerate", Battlefield_ObjectIDManager.shared.GetObjectFromID(creatureIds[i]), Element.Light);
    //                    StartCoroutine(GenerateQuantaLogic(Element.Light, 1));
    //                }
    //                if (creature.passive.Contains("devourer"))
    //                {
    //                    if (opponent.GetAllQuantaOfElement(Element.Other) > 0 && opponent.sanctuaryCount == 0)
    //                    {
    //                        StartCoroutine(opponent.SpendQuantaLogic(Element.Other, 1));
    //                        Game_AnimationManager.shared.StartAnimation("QuantaGenerate", Battlefield_ObjectIDManager.shared.GetObjectFromID(creatureIds[i]), Element.Darkness);
    //                        StartCoroutine(GenerateQuantaLogic(Element.Darkness, 1));
    //                    }
    //                }
    //                if (creature.passive.Contains("overdrive"))
    //                {
    //                    creature.AtkModify += 3;
    //                    creature.DefModify -= 1;
    //                }
    //                if (creature.passive.Contains("acceleration"))
    //                {
    //                    creature.AtkModify += 2;
    //                    creature.DefModify -= 1;
    //                }
    //                if (creature.passive.Contains("infest"))
    //                {
    //                    PlayCardOnFieldLogic(CardDatabase.Instance.GetCardFromId("4t8"));
    //                }
    //                if (adrenalineIndex < 1 && creature.passive.Contains("singularity"))
    //                {
    //                    List<string> singuEffects = new List<string>();
    //                    if (creature.AtkNow > 0)
    //                    {
    //                        creature.atk = -(Mathf.Abs(creature.atk));
    //                        creature.AtkModify = -(Mathf.Abs(creature.AtkModify));
    //                    }

    //                    if (!creature.innate.Contains("immaterial"))
    //                    {
    //                        singuEffects.Add("Immaterial");
    //                    }
    //                    if (!creature.passive.Contains("adrenaline"))
    //                    {
    //                        singuEffects.Add("Addrenaline");
    //                    }
    //                    if (!creature.passive.Contains("vampire"))
    //                    {
    //                        singuEffects.Add("Vampire");
    //                    }
    //                    singuEffects.Add("Chaos");
    //                    singuEffects.Add("Copy");
    //                    singuEffects.Add("Nova");

    //                    switch (singuEffects[UnityEngine.Random.Range(0, singuEffects.Count)])
    //                    {
    //                        case "Immaterial":
    //                            creature.innate.Add("immaterial");
    //                            break;
    //                        case "Vampire":
    //                            creature.passive.Add("vampire");
    //                            break;
    //                        case "Chaos":
    //                            int chaos = UnityEngine.Random.Range(1, 6);
    //                            creature.AtkModify += chaos;
    //                            creature.DefModify += chaos;
    //                            break;
    //                        case "Nova":
    //                            for (int y = 0; y < 12; y++)
    //                            {
    //                                yield return opponent.StartCoroutine(opponent.GenerateQuantaLogic((Element)i, 1));
    //                            }
    //                            break;
    //                        case "Addrenaline":
    //                            creature.passive.Add("adrenaline");
    //                            break;
    //                        default:
    //                            Card duplicate = new Card(creature);
    //                            Game_AnimationManager.shared.StartAnimation("ParallelUniverse", Battlefield_ObjectIDManager.shared.GetObjectFromID(iD));
    //                            PlayCardOnFieldLogic(duplicate);
    //                            break;
    //                    }
    //                    yield return null;
    //                }
    //                if (creature.passive.Contains("swarm"))
    //                {
    //                    creature.DefModify += scarabsPlayed;
    //                }
    //            }

    //            if (creature.Freeze > 0)
    //            {
    //                creature.Freeze--;
    //            }

    //            if (creature.Charge > 0)
    //            {
    //                creature.Charge--;
    //                creature.AtkModify--;

    //            }
    //            if (creature.IsDelayed)
    //            {
    //                creature.innate.Remove("delay");
    //            }

    //            int healthChange = creature.Poison;
    //            creature.DefDamage += healthChange;
    //            if (creature.DefDamage < 0) { creature.DefDamage = 0; }

    //            if (creature.DefNow <= 0)
    //            {
    //                yield return StartCoroutine(RemoveCardFromFieldLogic(iD));
    //                continue;
    //            }

    //            creatureDisplayers[iD.Index].DisplayCard(creature, false);
    //        }

    //        if (creature.AtkNow != 0)
    //        {
    //            if (creature.passive.Contains("adrenaline"))
    //            {
    //                adrenalineIndex++;
    //                if (DuelManager.AdrenalineDamageList[Mathf.Abs(creature.AtkNow) - 1].Count < adrenalineIndex)
    //                {
    //                    atkNow = DuelManager.AdrenalineDamageList[Mathf.Abs(creature.AtkNow) - 1][adrenalineIndex];
    //                    if (creature.passive.Contains("antimatter"))
    //                    {
    //                        atkNow = -atkNow;
    //                    }
    //                    goto adrenalineCheck;
    //                }
    //            }
    //        }
    //    }
    //}
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
