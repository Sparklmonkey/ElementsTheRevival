using UnityEngine.SceneManagement;

public class SceneTransitionManager : SingletonMono<SceneTransitionManager>
{
    public void LoadScene(string sceneToLoad)
    {
        EventBusUtil.ClearAllBuses();
        SceneManager.LoadScene(sceneToLoad);
    }

    internal string GetActiveScene() => SceneManager.GetActiveScene().name;
}
