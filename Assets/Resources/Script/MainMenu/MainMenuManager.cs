using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public GameObject Buttons;
    
    void Start()
    {
        Buttons.SetActive(false);
    }

    void Update()
    {
        
    }

    public void EndAnimation()
    {
        Buttons.SetActive(true);
    }
}
