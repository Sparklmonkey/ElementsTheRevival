using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICardComponent
{

}





//public IEnumerator Event_DeathTrigger(MonoBehaviour sender, EventUtility.ActiveCardArgs args)
//{

//    if (_activeCard.passive.Contains("scavenger"))
//    {
//        _activeCard.AtkModify += 1;
//        _activeCard.DefModify += 1;
//    }

//    if (_activeCard.skill == "soul catch")
//    {
//        yield return StartCoroutine(_owner.GenerateQuantaLogic(Element.Death, _activeCard.iD.IsUpgraded() ? 3 : 2));
//    }
//    if (_activeCard.skill == "boneyard" && !args.isSkeletonDeath)
//    {
//        _owner.PlayCardOnFieldLogic(CardDatabase.Instance.GetCardFromId(_activeCard.iD.IsUpgraded() ? "716" : "52m"));
//    }

//    if (_activeCard.skill == "bones" && !args.isSkeletonDeath)
//    {
//        _owner.AddPlayerCounter(CounterEnum.Bone, 2);
//    }
//    yield break;
//}