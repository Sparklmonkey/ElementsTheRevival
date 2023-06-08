using System;
using UnityEngine;

namespace Elements.Duel.Manager
{
    public class HealthManager
    {
        private int _maxHealth;
        private int _currentHealth;
        private bool _isPlayer;
        public event Action<int, bool> HealthChangedEvent;
        public event Action<int, bool> MaxHealthUpdatedEvent;

        public HealthManager(int maxHealth, bool isPlayer)
        {
            _maxHealth = _currentHealth = maxHealth;
            _isPlayer = isPlayer;
        }

        public int ModifyHealth(int amount, bool isDamage = true)
        {
            _currentHealth += isDamage ? -amount : amount;

            _currentHealth = _currentHealth > _maxHealth ? _maxHealth : _currentHealth;
            HealthChangedEvent?.Invoke(_currentHealth, _isPlayer);
            return _currentHealth;
        }


        public int ModifyMaxHealth(int maxHPBuff, bool isIncrease)
        {
            if (isIncrease)
            {
                _maxHealth += maxHPBuff;
            }
            else
            {
                _maxHealth -= maxHPBuff;
            }
            MaxHealthUpdatedEvent?.Invoke(_currentHealth, _isPlayer);
            return _maxHealth;
        }

        public int GetMaxHealth() => _maxHealth;

        internal bool IsMaxHealth() => _maxHealth == _currentHealth;

        public int GetCurrentHealth() => _currentHealth;
    }
}