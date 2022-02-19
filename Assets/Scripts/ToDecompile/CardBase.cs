
//// Elements.Core.IDEqualityComparer
//using System.Collections.Generic;
//using Elements.Core;

//public class IDEqualityComparer : IEqualityComparer<ID>
//{
//	public int GetHashCode(ID iD)
//	{
//		return iD.ToString().GetHashCode();
//	}

//	public bool Equals(ID id1, ID id2)
//	{
//		if (id1.owner == id2.owner && id1.field == id2.field)
//		{
//			return id1.index == id2.index;
//		}
//		return false;
//	}
//}

//public class PlayerData
//{
//	public static PlayerData shared = new PlayerData();

//	public bool isPlayerTurn;

//	public List<CardBase> playerDeck;

//	public CardBase playerMark;

//	public List<CardBase> enemyDeck;

//	public CardBase enemyMark;

//	public string username;

//	public string enemyAi;

//	public int electrum;

//	public List<CardBase> cardsInInventory;

//	public int wins;

//	public int losses;

//	public bool quickPlayEnabled;

//	public int enemyHP;
//}
