using System;
using Elements.Duel.Manager;
using Elements.Duel.Visual;
using UnityEngine;
using UnityEngine.EventSystems;

[Serializable]
public class IDCardPair : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public ID id;
    public Card card;
    public int stackCount;
    public bool isPlayer;
    public event Action<Card, int> OnCardChanged;
    public event Action<Card, int> OnCardRemoved;

    public event Action<IDCardPair, bool> OnHoverObject;
    public event Action<IDCardPair> OnClickObject;
    public event Action<int> OnCreatureAttack;

    public event Action<bool> OnBeingTargeted;
    public event Action<bool> OnBeingPlayable;

    void Start()
    {
        var parentName = transform.parent.gameObject.name;

        if (parentName != "EnemySide" && parentName != "PlayerSide")
        {
            int index = int.Parse(parentName.Split("_")[1]) - 1;
            if (parentName.Contains("Creature"))
            {
                id = new(isPlayer ? OwnerEnum.Player : OwnerEnum.Opponent, FieldEnum.Creature, index);
            }
            else if (parentName.Contains("Permanent"))
            {
                id = new(isPlayer ? OwnerEnum.Player : OwnerEnum.Opponent, FieldEnum.Permanent, index);
            }
            else if (parentName.Contains("Hand"))
            {
                id = new(isPlayer ? OwnerEnum.Player : OwnerEnum.Opponent, FieldEnum.Hand, index);
            }
            var effectDisplay = GetComponent<EffectDisplayManager>();
            if (effectDisplay != null)
            {
                OnCardChanged += effectDisplay.UpdateEffectDisplay;
            }

            var cardDisplayer = GetComponent<CardDisplayer>();
            OnCardChanged += cardDisplayer.DisplayCard;
            OnCardRemoved += cardDisplayer.HideCard;
            OnBeingTargeted += cardDisplayer.ShouldShowTarget;
            OnBeingPlayable += cardDisplayer.ShouldShowUsableGlow;
        }

        OnClickObject += DuelManager.Instance.IdCardTapped;

        if (parentName != "EnemySide" && parentName != "PlayerSide" && !parentName.Contains("Passive"))
        {
            transform.parent.gameObject.SetActive(false);
        }
    }

    public bool IsTargetable()
    {
        var isCardTargetable = HasCard() ? !card.innate.Contains("immaterial") && !card.passive.Contains("burrow") : false;
        if (id.Field == FieldEnum.Player)
        {
            isCardTargetable = true;
        }
        return isCardTargetable;
    }

    public IDCardPair(ID id, Card card)
    {
        this.id = id;
        this.card = card;
    }

    public void PlayCard(Card card)
    {
        this.card = card;
        if (card.cardType == CardType.Pillar)
        {
            stackCount++;
        }
        else
        {
            stackCount = 1;
        }
        OnCardChanged?.Invoke(card, stackCount);
    }

    public void UpdateCard()
    {
        if (card.cardType.Equals(CardType.Creature))
        {
            if (card.DefDamage < 0) { card.DefDamage = 0; }
            if (card.DefNow <= 0)
            {
                RemoveCard();
                return;
            }
        }
        OnCardChanged?.Invoke(card, stackCount);
    }

    public void RemoveCard()
    {
        stackCount--;
        OnCardRemoved?.Invoke(card, stackCount);
        if (stackCount == 0)
        {
            card = null;
        }
    }

    public Card GetCard() => card;

    internal bool HasCard()
    {
        return card != null && card.iD != "4t2" && card.iD != "4t1" && card.cardName != "";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!HasCard()) { return; }
        OnClickObject?.Invoke(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!HasCard()) { return; }
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 objectSize = new (rectTransform.rect.height, rectTransform.rect.width);
        ToolTipCanvas.Instance.SetupToolTip(new Vector2(transform.position.x, transform.position.y), objectSize, card, id.Index + 1, id.Field == FieldEnum.Creature);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ToolTipCanvas.Instance.HideToolTip();
    }

    public bool IsFromHand()
    {
        return id.Field.Equals(FieldEnum.Hand);
    }

    //Generate Quanta Event
    public void GeneratePillarQuanta(QuantaManager quantaManager)
    {
        if (card.cardName.Contains("Pendulum"))
        {
            quantaManager.ChangeQuanta(card.skillElement, card.costElement == Element.Other ? 3 : 1, true);
            StartCoroutine(Game_AnimationManager.shared.PlayAnimation("QuantaGenerate", transform, card.costElement));

            card.skillElement = card.skillElement == card.costElement ? DuelManager.GetIDOwner(id).playerPassiveManager.GetMark().card.costElement : card.costElement;
        }
        else
        {
            if(card.cardType == CardType.Mark && id.Owner == OwnerEnum.Opponent)
            {
                quantaManager.ChangeQuanta(card.costElement, BattleVars.shared.enemyAiData.maxHP >= 150 ? 3 : 1, true);
            }
            else
            {
                quantaManager.ChangeQuanta(card.costElement, card.costElement == Element.Other ? 3 : 1, true);
                StartCoroutine(Game_AnimationManager.shared.PlayAnimation("QuantaGenerate", transform, card.costElement));
            }
        }
    }

    public void IsTargeted(bool shouldShowTarget)
    {
        OnBeingTargeted?.Invoke(shouldShowTarget);
    }

    public void IsPlayable(bool shouldShowGlow)
    {
        OnBeingPlayable?.Invoke(shouldShowGlow);
    }
    //Creature Attack Event
    public void CreatureAttackEvent(PlayerManager owner, PlayerManager opponent)
    {
        int adrenalineIndex = 0;
        int atkNow = card.AtkNow;

        bool shouldSkip = DuelManager.IsSundialInPlay() || owner.playerCounters.patience > 0 || card.Freeze > 0 || card.IsDelayed || atkNow == 0;

        if (!shouldSkip)
        {
            bool isFreedomEffect = UnityEngine.Random.Range(0, 100) < (25 * owner.playerCounters.freedom) && card.costElement.Equals(Element.Air);
            Game_SoundManager.shared.PlayAudioClip("CreatureDamage");
            opponent.ManageGravityCreatures(out atkNow, out card);
            if(card.DefNow <= 0)
            {
                RemoveCard();
                return;
            }

            if (!card.passive.Contains("momentum"))
            {
                opponent.ManageShield(out atkNow, out card);
                if (card.DefNow <= 0)
                {
                    RemoveCard();
                    return;
                }
            }

            if (atkNow > 0)
            {
                if (card.passive.Contains("venom"))
                {
                    opponent.AddPlayerCounter(PlayerCounters.Poison, 1);
                }
                if (card.passive.Contains("deadly venom"))
                {
                    opponent.AddPlayerCounter(PlayerCounters.Poison, 2);
                }
                if (card.passive.Contains("nuerotoxin"))
                {
                    opponent.AddPlayerCounter(PlayerCounters.Nuerotoxin, 1);
                }
            }
            if (atkNow != 0)
            {
                if (card.passive.Contains("vampire"))
                {
                    owner.ModifyHealthLogic(atkNow, false, false);
                }
                OnCreatureAttack?.Invoke(atkNow);
                opponent.ReceivePhysicalDamage(isFreedomEffect ? Mathf.FloorToInt(atkNow * 1.5f) : atkNow);
            }
        }


    }
    //public IEnumerator CreatureCheckStep()
    //{
    //    int index = 0;
    //    for (int i = 0; i < creatureCards.Count; i++)
    //    {
    //    adrenalineCheck:


    //    skipCreatureAttack:
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
    //                    GenerateQuantaLogic(Element.Air, 1);
    //                }
    //                if (creature.passive.Contains("earth"))
    //                {
    //                    Game_AnimationManager.shared.StartAnimation("QuantaGenerate", Battlefield_ObjectIDManager.shared.GetObjectFromID(creatureIds[i]), Element.Earth);
    //                    GenerateQuantaLogic(Element.Earth, 1);
    //                }
    //                if (creature.passive.Contains("fire"))
    //                {
    //                    Game_AnimationManager.shared.StartAnimation("QuantaGenerate", Battlefield_ObjectIDManager.shared.GetObjectFromID(creatureIds[i]), Element.Fire);
    //                    GenerateQuantaLogic(Element.Fire, 1);
    //                }
    //                if (creature.passive.Contains("light"))
    //                {
    //                    Game_AnimationManager.shared.StartAnimation("QuantaGenerate", Battlefield_ObjectIDManager.shared.GetObjectFromID(creatureIds[i]), Element.Light);
    //                    GenerateQuantaLogic(Element.Light, 1);
    //                }
    //                if (creature.passive.Contains("devourer"))
    //                {
    //                    if (opponent.GetAllQuantaOfElement(Element.Other) > 0 && opponent.sanctuaryCount == 0)
    //                    {
    //                        opponent.SpendQuantaLogic(Element.Other, 1);
    //                        Game_AnimationManager.shared.StartAnimation("QuantaGenerate", Battlefield_ObjectIDManager.shared.GetObjectFromID(creatureIds[i]), Element.Darkness);
    //                        GenerateQuantaLogic(Element.Darkness, 1);
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
    //                                opponent.GenerateQuantaLogic((Element)i, 1);
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

    //    yield return StartCoroutine(WeaponAttackStep());
    //}
}
