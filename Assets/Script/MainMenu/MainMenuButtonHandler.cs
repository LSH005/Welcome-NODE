using UnityEngine;

public class MainMenuButtonHandler : MonoBehaviour, I_MouseClickable
{
    public enum ButtonTypeEnum {Start, Restart, EndGame, GameData, Setting};

    public ButtonTypeEnum ButtonType;
    public GameObject SettingUI;

    public static bool isSettingOn;

    private void Start()
    {
        MainMenuButtonHandler.isSettingOn = false;

        if (SettingUI != null)
        {
            SettingUI.SetActive(false);
        }
    }

    public void OnLeftClicked()
    {
        if (ScreenTransition.isTransitioning) return;
        if (MainMenuButtonHandler.isSettingOn) return;

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
            case ButtonTypeEnum.Setting:
                SettingButton();
                break;
            case ButtonTypeEnum.GameData:
                CollectionButton();
                break;
        }
    }

    void StartButton()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        ScreenTransition.ScreenTransitionGoto("EntityMovement", "Null", Color.black, 0f, 0.2f, 0f, 0.5f, 0f);
    }

    void RestartButton()
    {
        ScreenTransition.ScreenTransitionGoto("MainMenu", "Null", Color.black, 0f, 1f, 0f, 0.5f, 0f);
    }

    void EndGameButton()
    {
        ScreenTransition.ScreenTransitionGoto("ExitGame", "Null", Color.black, 0f, 0.4f, 0f, 0.5f, 0f);
    }

    void SettingButton()
    {
        if (SettingUI != null)
        {
            MainMenuButtonHandler.isSettingOn = true;
            SettingUI.SetActive(true);
        }
    }

    void CollectionButton()
    {
        Debug.Log("CollectionButton");
    }
    
    public void OnRightClicked()
    {
        // 우클릭에서 아무 동작 하지 않음
    }
}
