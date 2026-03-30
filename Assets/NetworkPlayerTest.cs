using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

public class NetworkPlayerTest : MonoBehaviour
{
    public CardDatabase CardDatabase;
    private void Start()
    {
        WriteCSV();
    }
    
    private void WriteCSV()
    {
        var filename = Application.dataPath + "/cards.csv";
        TextWriter writer = new StreamWriter(filename);
        writer.WriteLine("card_background, Card_Name, Amount, Quanta_Element, Card_Image, Creature_Value, Card_Type");
        foreach (var card in CardDatabase.FullCardList)
        {
            var background = card.CardElement.FastElementString() + ".png";
            var cardName = card.CardName;
            var cardCost = card.Cost;
            var costElement = card.CostElement.FastElementString();
            var spriteName = card.cardImage.name + ".png";
            var description = Regex.Replace(card.Desc, @"\r\n?|\n", " ");
            var creatureValue = card.Type == CardType.Creature ? $"{card.Atk}|{card.Def}" : " ";
            var cardType = card.Type.FastCardTypeString();
            writer.WriteLine(background + "," + cardName + "," + cardCost + "," + costElement + "," + spriteName + "," +  creatureValue  + "," + cardType);
        }
        writer.Close();
        
        Debug.Log(filename);
    }
}
