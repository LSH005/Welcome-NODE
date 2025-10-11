using UnityEngine;

public class StartGame : MonoBehaviour
{
    void Start()
    {
        ScreenTransition.ScreenTransitionGoto("MainMenu", "Null", Color.black, 1f, 0f, 0f, 0.5f, 0f);
    }
}
