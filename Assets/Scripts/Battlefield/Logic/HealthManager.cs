using System;

namespace Elements.Duel.Manager
{
    public class HealthManager
    {
        private int _maxHealth;
        private int _currentHealth;
        private bool _isPlayer;
        public event Action<int, bool> OnHealthChangedEvent;
        public event Action<int, bool> OnMaxHealthUpdatedEvent;

        public HealthManager(int maxHealth, bool isPlayer)
        {
            _maxHealth = _currentHealth = maxHealth;
            _isPlayer = isPlayer;
        }

        public void ModifyHealth(int amount, bool isDamage = true)
        {
            _currentHealth += isDamage ? -amount : amount;

            _currentHealth = _currentHealth > _maxHealth ? _maxHealth : _currentHealth;
            OnHealthChangedEvent?.Invoke(_currentHealth, _isPlayer);
        }


        public void ModifyMaxHealth(int maxHpBuff, bool isIncrease)
        {
            if (isIncrease)
            {
                _maxHealth += maxHpBuff;
            }
            else
            {
                _maxHealth -= maxHpBuff;
            }
            OnMaxHealthUpdatedEvent?.Invoke(_maxHealth, _isPlayer);
        }

        public int GetMaxHealth() => _maxHealth;

        internal bool IsMaxHealth() => _maxHealth == _currentHealth;

        public int GetCurrentHealth() => _currentHealth;
    }
}