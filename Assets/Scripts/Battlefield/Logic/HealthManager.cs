using System;
using UnityEngine;

namespace Elements.Duel.Manager
{
    public class HealthManager
    {
        private int maxHealth;
        private int currentHealth;

        public HealthManager(int maxHealth)
        {
            this.maxHealth = currentHealth = maxHealth;
        }

        public int ModifyHealth(int amount, bool isDamage = true)
        {
            currentHealth += isDamage ? -amount : amount;

            currentHealth = currentHealth > maxHealth ? maxHealth : currentHealth;
            return currentHealth;
        }

        public int GetCurrentHealth() => currentHealth;

        public int ModifyMaxHealth(int maxHPBuff, bool isIncrease)
        {
            if (isIncrease)
            {
                maxHealth += maxHPBuff;
            }
            else
            {
                maxHealth -= maxHPBuff;
            }
            return maxHealth;
        }

        public int GetMaxHealth()
        {
            return maxHealth;
        }

        internal bool IsMaxHealth()
        {
            return maxHealth <= currentHealth;
        }
    }
}