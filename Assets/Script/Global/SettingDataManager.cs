using System;
using System.IO;
using UnityEngine;

[System.Serializable]
public class GameSettings
{
    public float volume = 0.5f; // 볼륨
    public float mouseSensitivity = 0.5f;   // 마우스 감도
}

public class SettingDataManager : MonoBehaviour
{
    public static GameSettings CurrentSettings { get; private set; }
    private static string savePath => Path.Combine(Application.persistentDataPath, "settings.json");

    private void Awake()
    {
        LoadSettings();
    }

    public static void LoadSettings()
    {
        // 저장된 파일이 있는지 확인.
        if (File.Exists(savePath))
        {
            try
            {
                // 파일에서 JSON 텍스트를 모두 불러오기
                string jsonString = File.ReadAllText(savePath);

                // JSON 텍스트를 C# 객체로 변환(역직렬화)하여 CurrentSettings에 할당.
                CurrentSettings = JsonUtility.FromJson<GameSettings>(jsonString);
            }
            catch
            {
                // 오류 발생 시 기본 설정으로 초기화
                CurrentSettings = new GameSettings();
            }
        }
        else
        {
            // 설정이 존재하지 않으면 기본 설정으로 초기화
            CurrentSettings = new GameSettings();
        }
    }

    public static void SaveSettings()
    {
        // 현재 설정 객체를 JSON 문자열로 변환(직렬화)
        string jsonString = JsonUtility.ToJson(CurrentSettings, true);

        try
        {
            // JSON 문자열을 파일에 저장.
            File.WriteAllText(savePath, jsonString);
        }
        catch (Exception e)
        {
            Debug.LogError($"설정 파일 저장 실패: {e.Message}");
        }
    }

    public static void ResetSettingsToDefault()
    {
        CurrentSettings = new GameSettings();
    }

    public static void SetVolume(float newValue)
    {
        CurrentSettings.volume = newValue;
    }

    public static void SetMouseSensitivity(float newValue)
    {
        CurrentSettings.mouseSensitivity = newValue;
    }
}
