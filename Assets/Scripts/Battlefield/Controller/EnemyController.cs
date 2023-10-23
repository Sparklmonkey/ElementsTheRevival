using System;
using System.Collections;
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
        _aiStateMachine.DisplayCardPlayed += cardPlayedDisplay.ShowCardPlayed;
        _isSetup = true;
    }

    private void Update()
    {
        if (!_isSetup)
        {
            return;
        }

        if (_hasStarted)
        {
            return;
        }
        StartCoroutine(_aiStateMachine.Update(this));
        _hasStarted = true;
    }
}
