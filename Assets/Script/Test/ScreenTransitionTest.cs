using UnityEngine;

public class ScreenTransitionTest : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ScreenTransition.ScreenTransitionGoto("MainMenu", "LoadingScreen_3", Color.black, 0f, 1f, 3f, 1f, 0f);
        }
    }
}
