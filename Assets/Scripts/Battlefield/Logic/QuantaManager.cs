using Elements.Duel.Visual;
using System.Collections.Generic;
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
                List<QuantaObject> quantaList = isAdd ? _quantaObjects : _quantaObjects.FindAll(x => x.Count > 0);
                QuantaObject rndQuanta = quantaList[Random.Range(0, quantaList.Count)];

                while (amount > 0)
                {
                    rndQuanta.UpdateQuanta(1, isAdd);
                    amount--;
                    quantaList = isAdd ? _quantaObjects : _quantaObjects.FindAll(x => x.Count > 0);
                    rndQuanta = quantaList[Random.Range(0, quantaList.Count)];
                }
            }
            else
            {
                _quantaObjects.Find(x => x.Element == element).UpdateQuanta(amount, isAdd);
            }


        }

        public int GetQuantaForElement(Element element) => element.Equals(Element.Other) ? _quantaObjects.GetFullQuantaCount() : _quantaObjects[(int)element].Count;

        public List<int> GetCurrentQuanta()
        {
            List<int> listToReturn = new List<int>();
            foreach (QuantaObject item in _quantaObjects)
            {
                listToReturn.Add(item.Count);
            }
            return listToReturn;
        }

        public bool HasEnoughQuanta(Element element, int amount) => element.Equals(Element.Other) ? _quantaObjects.GetFullQuantaCount() >= amount : _quantaObjects.Find(x => x.Element == element).Count >= amount;

    }
}