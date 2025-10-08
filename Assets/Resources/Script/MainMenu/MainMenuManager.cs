using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public GameObject Buttons;
    
    void Start()
    {
        Buttons.SetActive(false);
    }

    public void EndAnimation()
    {
        Buttons.SetActive(true);
    }
}
