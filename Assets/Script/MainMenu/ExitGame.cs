using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ExitGame : MonoBehaviour
{
    public float exitDelay = 2f;

    void Start()
    {
        StartCoroutine(QuitAfterDelay(exitDelay));
    }
    private IEnumerator QuitAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        QuitGame();
    }

    public void QuitGame()
    {
        Application.Quit();

        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #endif
    }
}