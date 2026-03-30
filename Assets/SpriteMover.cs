using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SplashScreen
{
public class SpriteMover : MonoBehaviour
{
    private List<Transform> _path;
    private int _currentPathIndex;
    private int _spriteIndex;
    private bool _startAnimation;
    public Action<int> StartNextSpriteAnimation;

    public void SetPath(List<Transform> path, int spriteIndex)
    {
        _path = path;
        _startAnimation = true;
        _spriteIndex = spriteIndex;
    }

    void Update()
    {
        if (!_startAnimation) return;
        if (_currentPathIndex >= _path.Count) return;
        transform.position = Vector3.MoveTowards(transform.position, _path[_currentPathIndex].position, 10 * Time.deltaTime);
        if (transform.position != _path[_currentPathIndex].position) return;
        _currentPathIndex++;
        if (_currentPathIndex == 1)
        {
            StartNextSpriteAnimation?.Invoke(_spriteIndex);
        }
    }
}
    
}
