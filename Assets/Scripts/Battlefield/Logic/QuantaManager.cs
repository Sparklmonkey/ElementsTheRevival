using System.Collections.Generic;
using UnityEngine;

namespace Elements.Duel.Manager
{
    public class QuantaManager
    {
        private List<QuantaObject> quantaObjects;

        public QuantaManager()
        {
            quantaObjects = new List<QuantaObject>();
            for (int i = 0; i < 12; i++)
            {
                quantaObjects.Add(new QuantaObject((Element)i, 0));
            }
        }

        public void AddQuanta(Element element, int value)
        {
            if (element.Equals(Element.Other))
            {
                int cost = value * 3;
                int rndIndex = Random.Range(0, 12);
                while (cost > 0)
                {
                    quantaObjects[rndIndex].count++;
                    cost--;
                }
            }
            foreach (QuantaObject quanta in quantaObjects)
            {
                if (quanta.element == element)
                {
                    quanta.count += value;
                    return;
                }
            }
        }

        public List<int> ScrambleQuanta()
        {
            List<int> newQuanta = new List<int>(GetCurrentQuanta());
            newQuanta.Shuffle();
            for (int i = 0; i < newQuanta.Count; i++)
            {
                quantaObjects[i].count = newQuanta[i];
            }
            return newQuanta;
        }

        public void SpendQuanta(Element element, int value)
        {
            if (element.Equals(Element.Other))
            {
                int cost = value;
                int rndIndex = Random.Range(0, 12);
                while (cost > 0)
                {
                    if (quantaObjects[rndIndex].count > 0)
                    {
                        quantaObjects[rndIndex].count--;
                        cost--;
                    }
                }
                return;
            }
            foreach (QuantaObject quanta in quantaObjects)
            {
                if (quanta.element == element)
                {
                    quanta.count -= value;
                    if(quanta.count < 0) { quanta.count = 0; }
                    return;
                }
            }
        }

        public int GetQuantaForElement(Element element)
        {
            if (element.Equals(Element.Other))
            {
                return quantaObjects.GetFullQuantaCount();
            }
            return quantaObjects[(int)element].count;
        }

        public List<int> GetCurrentQuanta()
        {
            List<int> listToReturn = new List<int>();
            foreach (QuantaObject item in quantaObjects)
            {
                listToReturn.Add(item.count);
            }
            return listToReturn;
        }

        public bool HasEnoughQuanta(Element element, int amount)
        {
            if (element.Equals(Element.Other))
            {
                return quantaObjects.GetFullQuantaCount() >= amount;
            }

            foreach (QuantaObject quanta in quantaObjects)
            {
                if (quanta.element == element)
                {
                    return quanta.count >= amount;
                }
            }
            return false;
        }

        public int GetElementIndex(Element element)
        {
            return quantaObjects.ContainsElement(element) ?? 0;
        }
    }
}