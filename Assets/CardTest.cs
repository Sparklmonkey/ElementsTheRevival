using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;
using System.Text;
using System.Linq;

public class CardTest : MonoBehaviour
{
    [SerializeField]
    private List<Card> cardList;
    public Dictionary<string, int> cardDict = new ();


    // Start is called before the first frame update
    void Start()
    { }

    private IEnumerator DisplayCards()
    {
        string toPrint = "";
        var list = CardDatabase.Instance.GetFullCardCount();
        for (int i = 0; i < list.Count; i++)
        {
            toPrint += $"{(Element)i} : {list[i]}";
        }
        //Debug.Log(toPrint);
        yield break;
    }


}