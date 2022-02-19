


//// Elements.Duel.Logic.OnPlayAddDeathTrigger
//using Elements.Core;
//using Elements.Duel.Logic;



//// Elements.Duel.Logic.PassiveFieldLogic
//using System.Collections.Generic;
//using Elements.Core;
//using Elements.Duel.Logic;
//using Elements.Duel.Visuals;
//using UnityEngine;

//public class PassiveFieldLogic : IFieldLogic
//{
//	private List<CardObjectOnField> passivesInPlay;

//	private Dictionary<ID, CardBase> passiveCards = new Dictionary<ID, CardBase>(new IDEqualityComparer());

//	private ID baseID;

//	private PlayerManager ownerPlayer;

//	public PassiveFieldLogic(PlayerManager ownerPlayer)
//	{
//		this.ownerPlayer = ownerPlayer;
//	}

//	private void ShowFullDetails(ID id)
//	{
//		GameObject.Find("CardDisplayManager").GetComponent<CardDisplayManager>().SetupCardDisplay(id);
//	}

//	public void SetupVisuals(ID owner, Transform transform)
//	{
//		passivesInPlay = new List<CardObjectOnField>(transform.GetComponentsInChildren<CardObjectOnField>());
//		baseID = new ID(owner.owner + "100");
//		foreach (CardObjectOnField item in passivesInPlay)
//		{
//			ID newID = new ID(baseID);
//			item.SetupObject(newID.index, delegate
//			{
//				ShowFullDetails(newID);
//			});
//			passiveCards[newID] = null;
//			baseID.index++;
//		}
//	}

//	public void BuffCard(int power, int toughness, ID cardID)
//	{
//	}

//	public void DamageCard(int amount, ID cardID)
//	{
//	}

//	public void DestroyCard(ID cardID)
//	{
//		passivesInPlay[cardID.index].ClearCardObject();
//		passiveCards[cardID] = null;
//	}

//	public void DestroyPillarStack(ID cardID)
//	{
//	}

//	public List<ID> GetAllIdsInPlay()
//	{
//		List<ID> list = new List<ID>();
//		foreach (KeyValuePair<ID, CardBase> passiveCard in passiveCards)
//		{
//			if (passiveCard.Value != null)
//			{
//				list.Add(passiveCard.Key);
//			}
//		}
//		return list;
//	}

//	public CardBase GetCard(ID cardID)
//	{
//		return passiveCards[cardID];
//	}

//	public List<CardBase> GetCardAllCardsInPlay()
//	{
//		List<CardBase> list = new List<CardBase>();
//		foreach (KeyValuePair<ID, CardBase> passiveCard in passiveCards)
//		{
//			if (passiveCard.Value != null)
//			{
//				list.Add(passiveCard.Value);
//			}
//		}
//		return list;
//	}

//	public bool HasSpace()
//	{
//		return true;
//	}

//	public void HealCard(int amount, ID cardID)
//	{
//	}

//	public ID PlayCard(CardBase card)
//	{
//		switch (card.cardType)
//		{
//		case CardType.Weapon:
//		{
//			passivesInPlay[0].FirstSetupCardOnField(card.name, 0, 0, card.cardType.ToString());
//			baseID.index = 0;
//			ID iD3 = new ID(baseID);
//			passiveCards[iD3] = card;
//			return new ID(iD3);
//		}
//		case CardType.Shield:
//		{
//			passivesInPlay[1].FirstSetupCardOnField(card.name, 0, 0, card.cardType.ToString());
//			baseID.index = 1;
//			ID iD2 = new ID(baseID);
//			passiveCards[iD2] = card;
//			return new ID(iD2);
//		}
//		case CardType.Mark:
//		{
//			passivesInPlay[2].FirstSetupCardOnField(card.name, 0, 0, card.cardType.ToString());
//			baseID.index = 2;
//			ID iD = new ID(baseID);
//			passiveCards[iD] = card;
//			return new ID(iD);
//		}
//		default:
//			return null;
//		}
//	}

//	public CardBase StealCard(ID cardID)
//	{
//		CardBase result = passiveCards[cardID];
//		DestroyCard(cardID);
//		return result;
//	}

//	internal void UpdateBoneShield(int boneShieldCount)
//	{
//		passivesInPlay[1].SetStack(boneShieldCount);
//	}

//	internal void UpdateBoard()
//	{
//		foreach (KeyValuePair<ID, CardBase> passiveCard in passiveCards)
//		{
//			if (passiveCard.Value != null)
//			{
//				passiveCard.Value.firstTurn = false;
//			}
//		}
//	}

//	internal void GenerateMarkQuanta()
//	{
//		Element element = passiveCards[new ID(DuelManagerLogic.isUserTurn ? "P102" : "E102")].element;
//		ownerPlayer.GenerateQuanta(element, 1);
//		passivesInPlay[2].StartQuantaGenerateAnimation(element);
//	}
//}

//// Elements.Duel.Logic.PermanentFieldLogic
//using System.Collections.Generic;
//using Elements.Core;
//using Elements.Duel.Logic;
//using Elements.Duel.Visuals;
//using UnityEngine;

//public class PermanentFieldLogic : IFieldLogic
//{
//	private List<CardObjectOnField> permanentsOnField;

//	private Dictionary<ID, CardBase> permanentCards = new Dictionary<ID, CardBase>(new IDEqualityComparer());

//	private int permanentsInPlayCount;

//	private ID baseID;

//	private PlayerManager ownerPlayer;

//	public PermanentFieldLogic(PlayerManager ownerPlayer)
//	{
//		this.ownerPlayer = ownerPlayer;
//	}

//	private void ShowFullDetails(ID id)
//	{
//		GameObject.Find("CardDisplayManager").GetComponent<CardDisplayManager>().SetupCardDisplay(id);
//	}

//	public void SetupVisuals(ID owner, Transform transform)
//	{
//		permanentsOnField = new List<CardObjectOnField>(transform.GetComponentsInChildren<CardObjectOnField>());
//		baseID = new ID(owner.owner + "200");
//		foreach (CardObjectOnField item in permanentsOnField)
//		{
//			ID newID = new ID(baseID);
//			item.SetupObject(baseID.index, delegate
//			{
//				ShowFullDetails(newID);
//			});
//			permanentCards[newID] = null;
//			baseID.index++;
//		}
//	}

//	public void BuffCard(int power, int toughness, ID cardID)
//	{
//	}

//	public void DamageCard(int amount, ID cardID)
//	{
//	}

//	public void DestroyCard(ID cardID)
//	{
//		if (permanentsOnField[cardID.index].DestroyPillarStack(1))
//		{
//			permanentCards[cardID] = null;
//		}
//	}

//	public void DestroyPillarStack(ID cardID)
//	{
//		if (permanentsOnField[cardID.index].DestroyPillarStack(3))
//		{
//			permanentCards[cardID] = null;
//		}
//	}

//	public List<ID> GetAllIdsInPlay()
//	{
//		List<ID> list = new List<ID>();
//		foreach (KeyValuePair<ID, CardBase> permanentCard in permanentCards)
//		{
//			if (permanentCard.Value != null)
//			{
//				list.Add(permanentCard.Key);
//			}
//		}
//		return list;
//	}

//	public CardBase GetCard(ID cardID)
//	{
//		return permanentCards[cardID];
//	}

//	public List<CardBase> GetCardAllCardsInPlay()
//	{
//		List<CardBase> list = new List<CardBase>();
//		foreach (KeyValuePair<ID, CardBase> permanentCard in permanentCards)
//		{
//			if (permanentCard.Value != null)
//			{
//				for (int i = 0; i < permanentsOnField[permanentCard.Key.index].GetStackCount(); i++)
//				{
//					list.Add(permanentCard.Value);
//				}
//			}
//		}
//		return list;
//	}

//	public bool HasSpace()
//	{
//		return permanentsInPlayCount < permanentsOnField.Count;
//	}

//	public void HealCard(int amount, ID cardID)
//	{
//	}

//	public ID PlayCard(CardBase card)
//	{
//		if (card.cardType.Equals(CardType.Pillar) && permanentCards.CompareCard(card))
//		{
//			ID iDOfCard = permanentCards.GetIDOfCard(card);
//			permanentsOnField[iDOfCard.index].StackPillar();
//			return new ID(iDOfCard);
//		}
//		baseID.index = Random.Range(0, permanentCards.Count);
//		while (permanentCards[baseID] != null)
//		{
//			baseID.index = Random.Range(0, permanentsOnField.Count);
//		}
//		permanentsInPlayCount++;
//		permanentsOnField[baseID.index].FirstSetupCardOnField(card.name, 0, 0, card.cardType.ToString());
//		permanentCards[baseID] = card;
//		return new ID(baseID);
//	}

//	public CardBase StealCard(ID cardID)
//	{
//		CardBase result = permanentCards[cardID];
//		DestroyCard(cardID);
//		return result;
//	}

//	public bool GenerateQuantaTurnEnd(Element element)
//	{
//		foreach (KeyValuePair<ID, CardBase> permanentCard in permanentCards)
//		{
//			if (!(permanentCard.Value != null) || !permanentCard.Value.cardType.Equals(CardType.Pillar))
//			{
//				continue;
//			}
//			int stackCount = permanentsOnField[permanentCard.Key.index].GetStackCount();
//			Element element2;
//			if (permanentCard.Value.name.Contains("Pendulum"))
//			{
//				element2 = (permanentCard.Value.pendulumTurn ? permanentCard.Value.element : DuelManagerLogic.GetMarkElement());
//				permanentCard.Value.pendulumTurn = !permanentCard.Value.pendulumTurn;
//			}
//			else
//			{
//				element2 = permanentCard.Value.element;
//			}
//			if (!element.Equals(element2))
//			{
//				continue;
//			}
//			if (element2.Equals(Element.Other))
//			{
//				List<int> list = new List<int>
//				{
//					0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
//					0, 0
//				};
//				for (int i = 0; i < stackCount; i++)
//				{
//					list[Random.Range(0, 12)]++;
//				}
//				for (int j = 0; j < list.Count; j++)
//				{
//					if (list[j] > 0)
//					{
//						ownerPlayer.GenerateQuanta((Element)j, list[j]);
//					}
//				}
//			}
//			else
//			{
//				ownerPlayer.GenerateQuanta(element2, stackCount);
//			}
//			permanentsOnField[permanentCard.Key.index].StartQuantaGenerateAnimation(element2);
//			return true;
//		}
//		return false;
//	}

//	public List<int> GetAllElementsToGenerate()
//	{
//		List<int> list = new List<int>
//		{
//			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
//			0, 0
//		};
//		foreach (CardBase item in GetCardAllCardsInPlay())
//		{
//			if (item.cardType.Equals(CardType.Pillar))
//			{
//				if (item.name.Contains("Pendulum"))
//				{
//					Element index = (item.pendulumTurn ? item.element : DuelManagerLogic.GetMarkElement());
//					item.pendulumTurn = !item.pendulumTurn;
//					list[(int)index]++;
//				}
//				else if (item.element.Equals(Element.Other))
//				{
//					list[Random.Range(0, 12)]++;
//					list[Random.Range(0, 12)]++;
//					list[Random.Range(0, 12)]++;
//				}
//				else
//				{
//					list[(int)item.element]++;
//				}
//			}
//		}
//		return list;
//	}

//	internal void UpdateBoard()
//	{
//		foreach (KeyValuePair<ID, CardBase> permanentCard in permanentCards)
//		{
//			if (permanentCard.Value != null)
//			{
//				permanentCard.Value.firstTurn = false;
//			}
//		}
//	}
//}

//// Elements.Duel.Logic.PlayerManager
//using System;
//using System.Collections.Generic;
//using Elements.Core;
//using Elements.Duel.Logic;
//using UnityEngine;

//public class PlayerManager
//{
//	private DeckLogic playerDeck;

//	private HandLogic playerHand;

//	private HealthLogic playerHealth;

//	private QuantaPoolLogic playerQuanta;

//	private PlayFieldLogic playField;

//	private int poison;

//	private bool isPlayer;

//	public int boneShieldCount;

//	private int purityCounters;

//	public int GetAllQuanta(Element element)
//	{
//		return playerQuanta.GetAllQuantaOfElement(element);
//	}

//	public int GetLightEmittingCreatures()
//	{
//		return playField.GetLightEmittingCreatures();
//	}

//	internal void EclipsePlayed()
//	{
//		playField.EclipsePlayed();
//	}

//	public void NightfallPlayed()
//	{
//		playField.NightfallPlayed();
//	}

//	internal void PurifyPlayer()
//	{
//		poison = 0;
//		purityCounters += 2;
//	}

//	internal void DamageAllCreatures(int damage)
//	{
//		playField.DamageAllCreatures(damage);
//	}

//	internal void EclipseRemoved()
//	{
//		playField.EclipseRemoved();
//	}

//	internal bool IsAlive()
//	{
//		return playerHealth.GetHealth() > 0;
//	}

//	public PlayerManager(List<CardBase> deck, CardBase mark, int playerMaxHealth, bool isPlayer, Transform parentObject)
//	{
//		playerDeck = new DeckLogic(deck, parentObject.Find("Deck"));
//		playerHand = new HandLogic(playerDeck.GetStartingHand(), parentObject.Find("Hand"), isPlayer);
//		playerHealth = new HealthLogic(playerMaxHealth, parentObject.Find("Health"));
//		playerQuanta = new QuantaPoolLogic(parentObject.Find("QuantaPool"));
//		this.isPlayer = isPlayer;
//		playField = new PlayFieldLogic(new ID(isPlayer ? "P000" : "E000"), parentObject.Find("Creatures"), parentObject.Find("Permanents"), parentObject.Find("Passive"), this);
//		playerHealth.UpdatePoisonCounter(0);
//		PlayCard(mark);
//	}

//	internal void HealCreature(ID target, int amount)
//	{
//		throw new NotImplementedException();
//	}

//	internal void NightfallRemoved()
//	{
//		playField.NightfallRemoved();
//	}

//	public void StartTurn()
//	{
//		playerHand.AddCard(playerDeck.DrawCard());
//		if (!isPlayer)
//		{
//			DuelManagerLogic.EndTurn();
//		}
//	}

//	internal List<CardBase> GetAllCreatures()
//	{
//		return playField.GetAllCreatures();
//	}

//	public bool GenerateQuanta(Element elementToGenerate)
//	{
//		return playField.GetElementsToGenerate(elementToGenerate);
//	}

//	internal void DamagePlayer(int amount, bool isFromCreature = false)
//	{
//		if (isFromCreature)
//		{
//			int num = amount;
//			ID creatureWithGravity = playField.GetCreatureWithGravity();
//			while (creatureWithGravity != null && num > 0)
//			{
//				num = -DamageCardWithLeftOver(creatureWithGravity, amount);
//				creatureWithGravity = playField.GetCreatureWithGravity();
//			}
//			if (num <= 0)
//			{
//				return;
//			}
//		}
//		playerHealth.ModifyHealth(amount, isDamage: true);
//	}

//	private int DamageCardWithLeftOver(ID creatureWithGravity, int amount)
//	{
//		return playField.DamageCardWithLeftOver(creatureWithGravity, amount);
//	}

//	public CardBase GetCardInfo(ID cardID)
//	{
//		if (cardID.field == Field.Hand)
//		{
//			return playerHand.GetCard(cardID);
//		}
//		return playField.GetCard(cardID);
//	}

//	public void DrawCard()
//	{
//		playerHand.AddCard(playerDeck.DrawCard());
//	}

//	public bool CanPlayCard(Element element, int cost)
//	{
//		return playerQuanta.HaveSufficientQuanta(element, cost);
//	}

//	public bool HasSpace(ID cardID)
//	{
//		return playerQuanta.HaveSufficientQuanta(GetCardInfo(cardID).element, GetCardInfo(cardID).cost);
//	}

//	public ID PlayCard(CardBase card)
//	{
//		return playField.PlayCard(card);
//	}

//	public void PlayCard(ID cardID)
//	{
//		CardBase cardInfo = GetCardInfo(cardID);
//		IBaseOnPlayAbility scriptFromName = cardInfo.onCardPlayAction.GetScriptFromName<IBaseOnPlayAbility>();
//		ID owner = playField.PlayCard(cardInfo);
//		scriptFromName?.ActiveActionWhenPlayed(owner);
//		playerQuanta.SpendQuanta(cardInfo.element, cardInfo.cost);
//		playerHand.PlayCard(cardID);
//	}

//	public Element GetMarkElement()
//	{
//		return playField.GetCard(isPlayer ? new ID("P102") : new ID("E102")).element;
//	}

//	public void PosionAllCreatures()
//	{
//		playField.PoisonAllCreatures();
//	}

//	public void PosionSelf()
//	{
//		poison++;
//		playerHealth.UpdatePoisonCounter(poison);
//	}

//	public void HealSelf(int amount)
//	{
//		playerHealth.ModifyHealth(amount, isDamage: false);
//	}

//	public void AddCardToDeck(CardBase targetCard)
//	{
//		playerDeck.ReturnCard(targetCard);
//	}

//	public void UpdateBoard()
//	{
//		playField.UpdateBoard();
//	}

//	public void GenerateQuanta(Element element, int amount)
//	{
//		playerQuanta.AddQuanta(element, amount);
//	}

//	public void DestroyCardStack(ID target)
//	{
//		playField.DestroyCardStack(target);
//	}

//	public void ModifyCreature(int power, int toughness, ID target)
//	{
//		playField.ModifyCreature(power, toughness, target);
//	}

//	internal void GenerateMarkQuanta()
//	{
//		playField.GenerateMarkQuanta();
//	}

//	public void ScrambleQuanta()
//	{
//		playerQuanta.ScrambleQuanta();
//	}

//	public void DamageCard(ID target, int v)
//	{
//		playField.DamageCard(v, target);
//	}

//	public void DestroyCard(ID target)
//	{
//		playField.DestroyCard(target);
//	}

//	public int GetHandCount()
//	{
//		return playerHand.GetHandCount();
//	}

//	public void SpendQuanta(Element elementCost, int cost)
//	{
//		playerQuanta.SpendQuanta(elementCost, cost);
//	}

//	public void UpdateCounters()
//	{
//		playField.UpdateCounters();
//	}

//	internal void UpdateBoneShield()
//	{
//		playField.UpdateBoneShield(boneShieldCount);
//	}

//	internal void PoisonDamagePurity()
//	{
//		DamagePlayer(poison);
//		if (purityCounters > 0)
//		{
//			HealSelf(purityCounters);
//		}
//	}
//}

//// Elements.Duel.Logic.PlayFieldLogic
//using System.Collections.Generic;
//using Elements.Core;
//using Elements.Duel.Logic;
//using UnityEngine;

//public class PlayFieldLogic
//{
//	private CreatureFieldLogic creatureField;

//	private PermanentFieldLogic permanentField;

//	private PassiveFieldLogic passiveField;

//	private PlayerManager ownerPlayer;

//	public PlayFieldLogic(ID owner, Transform creatureField, Transform permanentField, Transform passiveField, PlayerManager ownerPlayer)
//	{
//		this.creatureField = new CreatureFieldLogic();
//		this.creatureField.SetupVisuals(owner, creatureField);
//		this.permanentField = new PermanentFieldLogic(ownerPlayer);
//		this.permanentField.SetupVisuals(owner, permanentField);
//		this.passiveField = new PassiveFieldLogic(ownerPlayer);
//		this.passiveField.SetupVisuals(owner, passiveField);
//		this.ownerPlayer = ownerPlayer;
//	}

//	internal void EclipsePlayed()
//	{
//		creatureField.EclipsePlayed();
//	}

//	public void NightfallPlayed()
//	{
//		creatureField.NightfallPlayed();
//	}

//	internal void DamageAllCreatures(int damage)
//	{
//		foreach (ID item in creatureField.GetAllIdsInPlay())
//		{
//			creatureField.GetCard(item).invisibilityCount = 0;
//			DamageCard(damage, item);
//		}
//	}

//	internal void EclipseRemoved()
//	{
//		creatureField.EclipseRemoved();
//	}

//	internal int GetLightEmittingCreatures()
//	{
//		return creatureField.GetLightEmittingCreatures();
//	}

//	private IFieldLogic GetField(Field field)
//	{
//		return field switch
//		{
//			Field.Creature => creatureField, 
//			Field.Passive => passiveField, 
//			Field.Permanent => permanentField, 
//			_ => null, 
//		};
//	}

//	internal void DestroyCardStack(ID target)
//	{
//		GetField(target.field).DestroyPillarStack(target);
//	}

//	internal void PoisonAllCreatures()
//	{
//		creatureField.PoisonAllCreatures();
//	}

//	internal void ModifyCreature(int power, int toughness, ID target)
//	{
//		creatureField.BuffCard(power, toughness, target);
//	}

//	internal void NightfallRemoved()
//	{
//		creatureField.NightfallRemoved();
//	}

//	internal void DamageCard(int v, ID target)
//	{
//		GetField(target.field).DamageCard(v, target);
//	}

//	public CardBase GetCard(ID cardID)
//	{
//		return GetField(cardID.field).GetCard(cardID);
//	}

//	internal List<CardBase> GetAllCreatures()
//	{
//		return creatureField.GetCardAllCardsInPlay();
//	}

//	internal void DestroyCard(ID target)
//	{
//		GetCard(target).onCardPlayAction.GetScriptFromName<IBaseOnPlayAbility>()?.ActiveActionWhenDestroyed(target);
//		GetField(target.field).DestroyCard(target);
//	}

//	internal ID GetCreatureWithGravity()
//	{
//		return creatureField.GetCreatureWithGravity();
//	}

//	internal int DamageCardWithLeftOver(ID creatureWithGravity, int amount)
//	{
//		return creatureField.DamageCardWithLeftOver(creatureWithGravity, amount);
//	}

//	public ID PlayCard(CardBase cardToPlay)
//	{
//		cardToPlay.firstTurn = true;
//		if (cardToPlay.cardType.Equals(CardType.Creature))
//		{
//			return creatureField.PlayCard(cardToPlay);
//		}
//		if (cardToPlay.cardType.Equals(CardType.Pillar) || cardToPlay.cardType.Equals(CardType.Artifact))
//		{
//			return permanentField.PlayCard(cardToPlay);
//		}
//		if (cardToPlay.cardType.Equals(CardType.Weapon) || cardToPlay.cardType.Equals(CardType.Shield) || cardToPlay.cardType.Equals(CardType.Mark))
//		{
//			return passiveField.PlayCard(cardToPlay);
//		}
//		return null;
//	}

//	public bool GetElementsToGenerate(Element elementToGenerate)
//	{
//		return permanentField.GenerateQuantaTurnEnd(elementToGenerate);
//	}

//	internal void UpdateBoard()
//	{
//		creatureField.UpdateBoard();
//		permanentField.UpdateBoard();
//		passiveField.UpdateBoard();
//	}

//	internal void UpdateCounters()
//	{
//		creatureField.UpdateCounters();
//	}

//	internal void UpdateBoneShield(int boneShieldCount)
//	{
//		passiveField.UpdateBoneShield(boneShieldCount);
//	}

//	internal void GenerateMarkQuanta()
//	{
//		passiveField.GenerateMarkQuanta();
//	}
//}

//// Elements.Duel.Logic.QuantaPoolLogic
//using System.Collections.Generic;
//using Elements.Core;
//using Elements.Duel.Logic;
//using Elements.Duel.Visuals;
//using UnityEngine;

//public class QuantaPoolLogic
//{
//	private List<int> quantaPool = new List<int>
//	{
//		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
//		0, 0
//	};

//	private List<int> rndIndexForQuanta = new List<int>
//	{
//		0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
//		10, 11
//	};

//	private QuantaVisuals quantumPoolVisual;

//	public QuantaPoolLogic(Transform quantaObject)
//	{
//		quantumPoolVisual = new QuantaVisuals(quantaObject);
//	}

//	public void AddQuanta(Element element, int amount)
//	{
//		quantaPool[(int)element] += amount;
//		quantumPoolVisual.UpdateSingleVisual((int)element, quantaPool[(int)element]);
//	}

//	public void ScrambleQuanta()
//	{
//		List<int> list = new List<int>();
//		Elements.Duel.Logic.ExtensionMethods.Shuffle(rndIndexForQuanta);
//		for (int i = 0; i < rndIndexForQuanta.Count; i++)
//		{
//			list.Add(quantaPool[rndIndexForQuanta[i]]);
//		}
//		for (int j = 0; j < list.Count; j++)
//		{
//			quantaPool[j] = list[j];
//		}
//	}

//	public bool HaveSufficientQuanta(Element element, int cost)
//	{
//		if (element.Equals(Element.Other))
//		{
//			return Elements.Duel.Logic.ExtensionMethods.SumIntList(quantaPool) >= cost;
//		}
//		return quantaPool[(int)element] >= cost;
//	}

//	public void SpendQuanta(Element element, int cost)
//	{
//		if (element.Equals(Element.Other))
//		{
//			int num = cost;
//			while (num > 0)
//			{
//				int num2 = Random.Range(0, 12);
//				if (quantaPool[num2] > 0)
//				{
//					quantaPool[num2]--;
//					num--;
//					quantumPoolVisual.UpdateSingleVisual(num2, quantaPool[num2]);
//				}
//			}
//		}
//		else
//		{
//			quantaPool[(int)element] -= cost;
//			quantumPoolVisual.UpdateSingleVisual((int)element, quantaPool[(int)element]);
//		}
//	}

//	public int GetAllQuantaOfElement(Element element)
//	{
//		if (element.Equals(Element.Other))
//		{
//			return Elements.Duel.Logic.ExtensionMethods.SumIntList(quantaPool);
//		}
//		return quantaPool[(int)element];
//	}

//	public int BlackHole()
//	{
//		int num = 0;
//		for (int i = 0; i < quantaPool.Count; i++)
//		{
//			if (quantaPool[i] > 3)
//			{
//				num++;
//				quantaPool[i] -= 3;
//			}
//		}
//		return num;
//	}
//}

//// Elements.Duel.Logic.TargetInstructionLogic
//using UnityEngine;

//public class TargetInstructionLogic : MonoBehaviour
//{
//	[SerializeField]
//	private GameObject opponentButton;

//	[SerializeField]
//	private GameObject selfButton;

//	public void HideShowPlayerButtons(bool opponentValid, bool selfValid)
//	{
//		opponentButton.SetActive(opponentValid);
//		selfButton.SetActive(selfValid);
//	}
//}

//// Elements.Duel.Logic.WeaponArsenic
//using Elements.Core;
//using Elements.Duel.Logic;
