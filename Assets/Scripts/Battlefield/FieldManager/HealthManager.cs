namespace Elements.Duel.Manager
{
    public class HealthManager
    {
        private int _maxHealth;
        private int _currentHealth;
        private OwnerEnum _owner;

        private EventBinding<ModifyPlayerHealthLogicEvent> _modifyPlayerHealthLogicBinding;
    
        public void OnDisable() {
            EventBus<ModifyPlayerHealthLogicEvent>.Unregister(_modifyPlayerHealthLogicBinding);
        }
        
        public HealthManager(int maxHealth, OwnerEnum owner)
        {
            _maxHealth = _currentHealth = maxHealth;
            _owner = owner;
            _modifyPlayerHealthLogicBinding = new EventBinding<ModifyPlayerHealthLogicEvent>(ModifyPlayerHealth);
            EventBus<ModifyPlayerHealthLogicEvent>.Register(_modifyPlayerHealthLogicBinding);
        }

        private void ModifyPlayerHealth(ModifyPlayerHealthLogicEvent modifyPlayerHealthLogicEvent)
        {
            if (!modifyPlayerHealthLogicEvent.Owner.Equals(_owner)) return;
            if (modifyPlayerHealthLogicEvent.IsMaxChange)
            {
                _maxHealth += modifyPlayerHealthLogicEvent.Amount;
                _currentHealth += modifyPlayerHealthLogicEvent.Amount;
            }
            else
            {
                _currentHealth += modifyPlayerHealthLogicEvent.Amount;
                _currentHealth = _currentHealth > _maxHealth ? _maxHealth : _currentHealth;
            }
            
            EventBus<ModifyPlayerHealthVisualEvent>.Raise(new ModifyPlayerHealthVisualEvent(_currentHealth, _owner, _maxHealth));
        }
        
        public int GetMaxHealth() => _maxHealth;

        internal bool IsMaxHealth() => _maxHealth == _currentHealth;

        public int GetCurrentHealth() => _currentHealth;
    }
}