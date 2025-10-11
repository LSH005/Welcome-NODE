using System;
using System.IO;
using UnityEngine;

[System.Serializable]
public class GameSettings
{
    public float volume = 0.5f; // ����
    public float mouseSensitivity = 0.5f;   // ���콺 ����
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
        // ����� ������ �ִ��� Ȯ��.
        if (File.Exists(savePath))
        {
            try
            {
                // ���Ͽ��� JSON �ؽ�Ʈ�� ��� �ҷ�����
                string jsonString = File.ReadAllText(savePath);

                // JSON �ؽ�Ʈ�� C# ��ü�� ��ȯ(������ȭ)�Ͽ� CurrentSettings�� �Ҵ�.
                CurrentSettings = JsonUtility.FromJson<GameSettings>(jsonString);
            }
            catch
            {
                // ���� �߻� �� �⺻ �������� �ʱ�ȭ
                CurrentSettings = new GameSettings();
            }
        }
        else
        {
            // ������ �������� ������ �⺻ �������� �ʱ�ȭ
            CurrentSettings = new GameSettings();
        }
    }

    public static void SaveSettings()
    {
        // ���� ���� ��ü�� JSON ���ڿ��� ��ȯ(����ȭ)
        string jsonString = JsonUtility.ToJson(CurrentSettings, true);

        try
        {
            // JSON ���ڿ��� ���Ͽ� ����.
            File.WriteAllText(savePath, jsonString);
        }
        catch (Exception e)
        {
            Debug.LogError($"���� ���� ���� ����: {e.Message}");
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
