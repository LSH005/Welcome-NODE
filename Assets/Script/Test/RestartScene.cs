using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScene : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadCurrentScene();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ScreenTransition.ScreenTransitionGoto("MainMenu", "LoadingScreen_3", Color.black, 0f, 0.3f, 6f, 0.5f, 0f);
        }
    }

    public void ReloadCurrentScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currentSceneIndex);
    }
}