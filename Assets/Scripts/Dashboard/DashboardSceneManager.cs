using UnityEngine;
using UnityEngine.SceneManagement;

public class DashboardSceneManager : MonoBehaviour
{
    public void LoadNewScene(string sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
