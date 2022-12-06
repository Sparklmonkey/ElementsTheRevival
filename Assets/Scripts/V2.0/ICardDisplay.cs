using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICardDisplay 
{
    //Card Properties
    public Card CardOnDisplay { get; set; }
    public void SetCard(Card card);
    public Card GetCardOnDisplay();
    public void SetupDisplay(OwnerEnum owner, FieldEnum field);
    public void DisplayCard(Card cardToDisplay);
    public void ChangeStackCount(int newCount);
    public void ClearDisplay();

}
