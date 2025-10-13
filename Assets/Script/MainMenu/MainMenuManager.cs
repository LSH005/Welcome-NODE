using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public GameObject Buttons;
    
    void Start()
    {
        Buttons.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void EndAnimation()
    {
        Buttons.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
