using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private CardPlayedDisplay cardPlayedDisplay;
    
    private AiStateMachine _aiStateMachine;
    private PlayerManager _self;
    private bool _isSetup = false;
    private bool _hasStarted = false;
    
    public void SetupController(PlayerManager enemyManager, GameOverVisual gameOverVisual)
    {
        _aiStateMachine = new AiStateMachine(enemyManager, gameOverVisual);
        _self = enemyManager;
        _isSetup = true;
    }

    private void Update()
    {
        if (!_isSetup) return;

        if (_hasStarted) return;
        _hasStarted = true;
        StartCoroutine(_aiStateMachine.Update(this));
    }
}
