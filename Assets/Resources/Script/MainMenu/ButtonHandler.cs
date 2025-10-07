using System.Collections;
using UnityEngine;

public class ButtonHandler : MonoBehaviour, I_MouseClickable
{
    public enum ButtonTypeEnum {Start, Restart, EndGame};

    public ButtonTypeEnum ButtonType;

    public void OnLeftClicked()
    {
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

    }

    void EndGameButton()
    {
        Debug.Log("P-3");
    }
    
    public void OnRightClicked()
    {
        // ��Ŭ������ �ƹ� ���� ���� ����
    }
}
