using System.Collections.Generic;
using Core.Helpers;
using UnityEngine;

namespace Battlefield.Abilities
{
    public class SingularityEndTurn : OnEndTurnAbility
    {
        public override void Activate(ID owner, Card card)
        {
            List<string> singuEffects = new() { "Chaos", "Copy", "Nova" };

            if (!card.passiveSkills.Antimatter)
            {
                card.passiveSkills.Antimatter = true;
            }

            if (card.Atk > 0)
            {
                card.Atk *= -1;
            }

            if (card.AtkModify > 0)
            {
                card.AtkModify *= -1;
            }
            
            if (!card.innateSkills.Immaterial)
            {
                singuEffects.Add("Immaterial");
            }
            if (!card.passiveSkills.Adrenaline)
            {
                singuEffects.Add("Adrenaline");
            }
            if (!card.passiveSkills.Vampire)
            {
                singuEffects.Add("Vampire");
            }

            switch (singuEffects[Random.Range(0, singuEffects.Count)])
            {
                case "Immaterial":
                    card.innateSkills.Immaterial = true;
                    break;
                case "Vampire":
                    card.passiveSkills.Vampire = true;
                    break;
                case "Chaos":
                    var chaos = Random.Range(1, 6);
                    card.AtkModify -= chaos;
                    card.DefModify += chaos;
                    break;
                case "Nova":
                    for (var y = 0; y < 12; y++)
                    {
                        EventBus<QuantaChangeLogicEvent>.Raise(new QuantaChangeLogicEvent(1, (Element)y, owner.owner, true));
                    }
                    break;
                case "Adrenaline":
                    card.passiveSkills.Adrenaline = true;
                    break;
                default:
                    Card duplicate = card.Clone();
                    EventBus<PlayAnimationEvent>.Raise(new PlayAnimationEvent(owner, "ParallelUniverse", Element.Air));
                    EventBus<AddCardPlayedOnFieldActionEvent>.Raise(new AddCardPlayedOnFieldActionEvent(duplicate, owner.IsOwnedBy(OwnerEnum.Player)));
                    EventBus<PlayCreatureOnFieldEvent>.Raise(new PlayCreatureOnFieldEvent(owner.owner, duplicate));
                    break;
            }
        }
    }
}