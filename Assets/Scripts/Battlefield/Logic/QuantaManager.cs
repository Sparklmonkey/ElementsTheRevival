using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Elements.Duel.Visual;
using UnityEngine;

namespace Elements.Duel.Manager
{
    public class QuantaManager
    {
        private List<QuantaObject> _quantaObjects;
        private List<QuantaDisplayer> _quantaDisplayers;
        private PlayerManager _owner;

        public QuantaManager(List<QuantaDisplayer> quantaDisplayers, PlayerManager owner)
        {
            _owner = owner;
            _quantaDisplayers = quantaDisplayers;
            _quantaObjects = new List<QuantaObject>();
            for (int i = 0; i < 12; i++)
            {
                _quantaObjects.Add(new QuantaObject((Element)i, 0));
                _quantaObjects[i].OnQuantaChange += _quantaDisplayers[i].QuantaChanged;
            }
        }

        public void ChangeQuanta(Element element, int amount, bool isAdd)
        {
            if (element.Equals(Element.Other))
            {
                System.Random rnd = new();
                List<QuantaObject> quantaList = isAdd ? _quantaObjects : _quantaObjects.FindAll(x => x.count > 0);
                QuantaObject rndQuanta = quantaList.OrderBy(x => rnd.Next()).First();

                while (amount > 0)
                {
                    rndQuanta.UpdateQuanta(1, isAdd);
                    amount--;
                    quantaList = _quantaObjects.FindAll(x => x.count > 0);
                    rndQuanta = quantaList.OrderBy(x => rnd.Next()).First();
                }
            }
            else
            {
                _quantaObjects.Find(x => x.element == element).UpdateQuanta(amount, isAdd);
            }

       
        }

        public int GetQuantaForElement(Element element) => element.Equals(Element.Other) ? _quantaObjects.GetFullQuantaCount() : _quantaObjects[(int)element].count;

        public List<int> GetCurrentQuanta()
        {
            List<int> listToReturn = new List<int>();
            foreach (QuantaObject item in _quantaObjects)
            {
                listToReturn.Add(item.count);
            }
            return listToReturn;
        }

        public bool HasEnoughQuanta(Element element, int amount) => element.Equals(Element.Other) ? _quantaObjects.GetFullQuantaCount() >= amount : _quantaObjects.Find(x => x.element == element).count >= amount;

    }
}