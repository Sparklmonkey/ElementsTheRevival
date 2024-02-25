using System.Collections.Generic;
using UnityEngine;

public class CardTest : MonoBehaviour
{
    [SerializeField]
    private List<Card> cardList;
    [SerializeField]
    private Transform card;
    public Dictionary<string, int> CardDict = new ();
    private string _oetgDeckCode = "0a0va081da061c4061up022530624t02252018pu";

    // Start is called before the first frame update
    private void Start()
    {
    }
}