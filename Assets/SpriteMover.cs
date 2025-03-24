using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SplashScreen
{
public class SpriteMover : MonoBehaviour
{
    private List<Transform> _path;
    private StartNextSpriteMover _completion;
    private bool _isFirstPosition = true;

    public void SetupSpritePath(List<Transform> path, StartNextSpriteMover completion)
    {
        _path = path;
        _completion = completion;
        StartCoroutine(MoveSpriteAlongPath());
    }

    private IEnumerator MoveSpriteAlongPath()
    {
        foreach (var t in _path)
        {
            while (transform.position != t.position)
            {
                transform.position = Vector3.MoveTowards(transform.position, t.position, 3000f * Time.deltaTime);
                yield return null;
            }

            if (!_isFirstPosition) continue;
            _completion();
            _isFirstPosition = false;
        }
    }
}
    
}
