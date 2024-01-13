using Elements.Duel.Visual;
using System.Collections.Generic;
using UnityEngine;

namespace Elements.Duel.Manager
{
    public class QuantaManager
    {
        private QuantaPool _quantaPool;
        private List<QuantaDisplayer> _quantaDisplayers;
        private OwnerEnum _owner;
        
        public QuantaManager(OwnerEnum owner)
        {
            _owner = owner;
            _quantaSpentLogicBinding = new EventBinding<QuantaChangeLogicEvent>(UpdateQuantaManager);
            EventBus<QuantaChangeLogicEvent>.Register(_quantaSpentLogicBinding);
        }
        
        private EventBinding<QuantaChangeLogicEvent> _quantaSpentLogicBinding;
    
        public void OnDisable() {
            EventBus<QuantaChangeLogicEvent>.Unregister(_quantaSpentLogicBinding);
        }

        private void UpdateQuantaManager(QuantaChangeLogicEvent quantaChangeLogicEvent)
        {
            if (!quantaChangeLogicEvent.Owner.Equals(_owner)) return;
            ChangeQuanta(quantaChangeLogicEvent.Element, quantaChangeLogicEvent.Amount, quantaChangeLogicEvent.IsAdd);
        }

        private void ChangeQuanta(Element element, int amount, bool isAdd)
        {
            if (element.Equals(Element.Other))
            {
                if (isAdd)
                {
                    while (amount > 0)
                    {
                        var rndElement = (Element)Random.Range(0, 12);
                        var newAmount = _quantaPool.AddQuanta(rndElement, 1);
                        EventBus<QuantaChangeVisualEvent>.Raise(new QuantaChangeVisualEvent(newAmount, rndElement, _owner));
                        amount--;
                    }
                }
                else
                {
                    var elementList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
                    while (amount > 0)
                    {
                        var index = Random.Range(0, elementList.Count);
                        var rndElement = (Element)elementList[index];
                        if (_quantaPool.GetQuantaAmount(rndElement) == 0)
                        {
                            elementList.RemoveAt(index);
                            continue;
                        }
                        var newAmount = _quantaPool.AddQuanta(rndElement, -1);
                        EventBus<QuantaChangeVisualEvent>.Raise(new QuantaChangeVisualEvent(newAmount, rndElement, _owner));
                        amount--;
                    }
                }
            }
            else
            {
                var newAmount = _quantaPool.AddQuanta(element, isAdd ? amount : -amount);
                EventBus<QuantaChangeVisualEvent>.Raise(new QuantaChangeVisualEvent(newAmount, element, _owner));
            }
        }

        public int GetQuantaForElement(Element element) => element.Equals(Element.Other)
            ? _quantaPool.GetFullQuantaCount()
            : _quantaPool.GetQuantaAmount(element);
        
        public bool HasEnoughQuanta(Element element, int amount) => GetQuantaForElement(element) >= amount;

    }
}