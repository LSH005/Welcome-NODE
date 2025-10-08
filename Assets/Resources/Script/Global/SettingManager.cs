using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class SettingManager : MonoBehaviour
{
    [Header("InfoText")]
    public TextMeshProUGUI infoText;
    [Header("Sensitivity")]
    public GameObject sensitivityValueBox;
    public TextMeshProUGUI sensitivityValuetText;
    [Header("Volume")]
    public GameObject volumeValueBox;
    public TextMeshProUGUI volumeValuetText;

    private bool isAdjustingSensitivity;
    private bool isAdjustingVolume;
    private bool canAdjust;

    private float sensitivityValue;
    private float volumeValue;
    private float mouseDeltaX;
    private float adjustmentSpeed = 0.0025f;
    private float adjustInterval = 0.2f;
    private float lastAdjust;

    private Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        LoadSavedSetting();
        SetSensitivityValue(sensitivityValue);
        SetVolumeValue(volumeValue);

        HideSetting();
    }

    private void Update()
    {
        AdjustIntervalHandler();

        AdjustSensitivity();
        AdjustVolume();
    }

    void AdjustIntervalHandler()
    {
        if (canAdjust)
        {
            mouseDeltaX = Input.GetAxis("Mouse X");
            return;
        }

        if (lastAdjust + adjustInterval <= Time.time)
        {
            canAdjust = true;
        }
    }

    public void SensitivitySettingButton()
    {
        if (!canAdjust) return;
        isAdjustingSensitivity = true;
    }

    private void AdjustSensitivity()
    {
        if (!isAdjustingSensitivity) return;

        if (Input.GetMouseButtonDown(0))
        {
            lastAdjust = Time.time;
            canAdjust = false;
            isAdjustingSensitivity =false;
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            sensitivityValue -= 0.0001f;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            sensitivityValue += 0.0001f;
        }

        sensitivityValue += mouseDeltaX * adjustmentSpeed;
        sensitivityValue = Mathf.Clamp(sensitivityValue, 0.0001f, 2f);

        SetSensitivityValue(sensitivityValue);
    }

    public void VolumeSettingButton()
    {
        if (!canAdjust) return;
        isAdjustingVolume = true;
    }

    private void AdjustVolume()
    {
        if (!isAdjustingVolume) return;

        if (Input.GetMouseButtonDown(0))
        {
            lastAdjust = Time.time;
            canAdjust = false;
            isAdjustingVolume = false;
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            volumeValue -= 0.0001f;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            volumeValue += 0.0001f;
        }

        volumeValue += mouseDeltaX * adjustmentSpeed;
        volumeValue = Mathf.Clamp(volumeValue, 0.0001f, 2f);

        SetVolumeValue(volumeValue);
    }

    public void ExitButton()
    {
        if (!canAdjust) return;

        SaveCurrentValue();
        anim.SetTrigger("ExitSetting");
    }

    public void ResetSettingButton()
    {
        if (!canAdjust) return;

        volumeValue = 0.5f;
        sensitivityValue = 0.5f;

        SetVolumeValue(volumeValue);
        SetSensitivityValue(sensitivityValue);
    }

    public void HideSetting()
    {
        MainMenuButtonHandler.isSettingOn = false;
        gameObject.SetActive(false);
    }

    private void LoadSavedSetting()
    {
        sensitivityValue = SettingDataManager.CurrentSettings.mouseSensitivity;
        volumeValue = SettingDataManager.CurrentSettings.volume;
    }

    private void SetSensitivityValue(float newValue)
    {
        sensitivityValueBox.transform.localScale = new Vector2(newValue * 0.5f, 1);
        sensitivityValuetText.text = (newValue * 100).ToString("F2") + "%";
    }

    private void SetVolumeValue(float newValue)
    {
        volumeValueBox.transform.localScale = new Vector2(newValue * 0.5f, 1);
        volumeValuetText.text = (newValue * 100).ToString("F2") + "%";
    }

    private void SaveCurrentValue()
    {
        volumeValue = Mathf.Floor(volumeValue * 10000) / 10000;
        sensitivityValue = Mathf.Floor(sensitivityValue * 10000) / 10000;

        SettingDataManager.SetVolume(volumeValue);
        SettingDataManager.SetMouseSensitivity(sensitivityValue);
        SettingDataManager.SaveSettings();
    }
}
