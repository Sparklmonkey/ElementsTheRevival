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
            if (isDamage)
            {
                currentHealth -= amount;
            }
            else
            {
                int newHP = amount + currentHealth;
                currentHealth = newHP > maxHealth ? maxHealth : newHP;
            }

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