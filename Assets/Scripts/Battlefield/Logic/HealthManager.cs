using System;
using UnityEngine;

namespace Elements.Duel.Manager
{
    public class HealthManager
    {
        private int maxHealth;
        private int currentHealth;
        private bool _isPlayer;
        public event Action<int, bool> HealthChangedEvent;

        public HealthManager(int maxHealth, bool isPlayer)
        {
            this.maxHealth = currentHealth = maxHealth;
            _isPlayer = isPlayer;
        }

        public int ModifyHealth(int amount, bool isDamage = true)
        {
            currentHealth += isDamage ? -amount : amount;

            currentHealth = currentHealth > maxHealth ? maxHealth : currentHealth;
            HealthChangedEvent?.Invoke(currentHealth, _isPlayer);
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
            HealthChangedEvent?.Invoke(currentHealth, _isPlayer);
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