using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SettingManager : MonoBehaviour
{

    private Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        HideSetting();
    }

    public void SensitivitySettingButton()
    {
        Debug.Log("SensitivitySettingButton");
    }

    public void VolumeSettingButton()
    {
        Debug.Log("VolumeSettingButton");
    }

    public void ExitButton()
    {
        Debug.Log("ExitButton");
        anim.SetTrigger("ExitSetting");
    }

    public void HideSetting()
    {
        MainMenuButtonHandler.isSettingOn = false;
        gameObject.SetActive(false);
    }
}
