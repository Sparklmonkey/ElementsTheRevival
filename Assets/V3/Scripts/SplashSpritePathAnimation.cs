using System;
using System.Collections.Generic;
using UnityEngine;

public class SplashSpritePathAnimation : MonoBehaviour
{
    private List<Transform> _path;
    private int _currentPathIndex;
    private int _spriteIndex;
    private bool _startAnimation;
    public Action<int> StartNextSpriteAnimation;
    private SpriteRenderer _spriteRenderer;

    public void SetPath(List<Transform> path, int spriteIndex)
    {
        _path = path;
        _startAnimation = true;
        _spriteIndex = spriteIndex;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = ImageHelper.GetElementImage(((Element)spriteIndex).ToString());
    }

    private void Update()
    {
        if (!_startAnimation) return;
        if (_currentPathIndex >= _path.Count) return;
        transform.position = Vector3.MoveTowards(transform.position, _path[_currentPathIndex].position, 10 * Time.deltaTime);
        if (transform.position != _path[_currentPathIndex].position) return;
        _currentPathIndex++;
        if (_currentPathIndex >= _path.Count)
        {
            _startAnimation = false;
        }
        if (_currentPathIndex == 1)
        {
            StartNextSpriteAnimation?.Invoke(_spriteIndex);
        }
    }
}
