using System.Collections;
using UnityEngine;

public class ButtonHandler : MonoBehaviour, I_MouseClickable
{
    public enum ButtonTypeEnum {Start, Restart, EndGame};

    public ButtonTypeEnum ButtonType;

    public void OnLeftClicked()
    {
        if (ScreenTransition.isTransitioning) return;

        switch (ButtonType)
        {
            case ButtonTypeEnum.Start:
                StartButton();
                break;
            case ButtonTypeEnum.Restart:
                RestartButton();
                break;
            case ButtonTypeEnum.EndGame:
                EndGameButton();
                break;
        }
    }

    void StartButton()
    {
        Debug.Log("P-1");
    }

    void RestartButton()
    {
        ScreenTransition.ScreenTransitionGoto("MainMenu", "Null", Color.black, 0f, 1f, 0f, 0.5f, 0f);
    }

    void EndGameButton()
    {
        Debug.Log("P-3");
    }
    
    public void OnRightClicked()
    {
        // 우클릭에서 아무 동작 하지 않음
    }
}
