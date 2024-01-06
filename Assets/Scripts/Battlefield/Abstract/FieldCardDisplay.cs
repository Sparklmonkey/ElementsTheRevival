using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public abstract class FieldCardDisplay
{
    public Card Card { get; private set; }
    public ID Id { get; private set; }

    public void SetupId(ID newId) => Id = newId;
}
