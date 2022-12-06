using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Elements.Duel.Visual;
using UnityEngine;

namespace Elements.Duel.Manager
{
    public class QuantaManager
    {
        private List<QuantaObject> quantaObjects;
        private List<QuantaDisplayer> quantaDisplayers;
        private PlayerManager owner;

        public QuantaManager(List<QuantaDisplayer> quantaDisplayers, PlayerManager owner)
        {
            this.owner = owner;
            this.quantaDisplayers = quantaDisplayers;
            quantaObjects = new List<QuantaObject>();
            for (int i = 0; i < 12; i++)
            {
                quantaObjects.Add(new QuantaObject((Element)i, 0));
            }
        }

        public IEnumerator ChangeQuanta(Element element, int amount, bool isAdd)
        {
            if (element.Equals(Element.Other))
            {
                if (isAdd)
                {
                    while (amount > 0)
                    {
                        quantaObjects[Random.Range(0, 12)].count++;
                        amount--;
                    }
                    goto UpdateAllQuanta;
                }

                System.Random rnd = new System.Random();
                List<QuantaObject> quantaList = quantaObjects.FindAll(x => x.count > 0);
                QuantaObject rndQuanta = quantaList.OrderBy(x => rnd.Next()).First();
                while (amount > 0)
                {
                    rndQuanta.count--;
                    amount--;
                    if (amount == 0) { goto UpdateAllQuanta; }
                    quantaList = quantaObjects.FindAll(x => x.count > 0);
                    rndQuanta = quantaList.OrderBy(x => rnd.Next()).First();
                }
                goto UpdateAllQuanta;
            }

            quantaObjects[(int)element].count += isAdd ? amount : -amount;
            if (quantaObjects[(int)element].count < 0) { quantaObjects[(int)element].count = 0; }
            if (quantaObjects[(int)element].count > 75) { quantaObjects[(int)element].count = 75; }
            owner.StartCoroutine(quantaDisplayers[(int)element].SetNewQuantaAmount(quantaObjects[(int)element].count.ToString()));
            yield break;

            UpdateAllQuanta:

            for (int i = 0; i < quantaDisplayers.Count; i++)
            {
                if (quantaObjects[i].count < 0) { quantaObjects[i].count = 0; }
                if (quantaObjects[i].count > 75) { quantaObjects[i].count = 75; }

                owner.StartCoroutine(quantaDisplayers[i].SetNewQuantaAmount(quantaObjects[i].count.ToString()));
            }

            yield break;
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

            return quantaObjects.Find(x => x.element == element).count >= amount;

        }
    }
}