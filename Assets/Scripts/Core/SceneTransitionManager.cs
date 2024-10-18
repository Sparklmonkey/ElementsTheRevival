using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : SingletonMono<SceneTransitionManager>
{
    [SerializeField] private SoundManager _soundManager; 
    public void LoadScene(string sceneToLoad)
    {
        EventBusUtil.ClearAllBuses();
        _soundManager.RegisterEvents();
        SceneManager.LoadScene(sceneToLoad);
    }

    internal string GetActiveScene() => SceneManager.GetActiveScene().name;
}
