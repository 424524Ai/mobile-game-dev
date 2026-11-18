using UnityEngine;

public class DeviceTier : MonoBehaviour
{
    const int Low = 0, Mid = 1, High = 2;   // Quality level order

    private void Awake()
    {
        int tier = PlayerPrefs.GetInt("tier_override", Detect());
        QualitySettings.SetQualityLevel(tier, true);
        Application.targetFrameRate = tier == Low ? 30 : 60;
        Debug.Log($"tier={QualitySettings.names[tier]}");
    }

    static int Detect()
    {
        int ram = SystemInfo.systemMemorySize;    // MB
        int cores = SystemInfo.processorCount;
        if (ram >= 8192 && cores >= 8) return High;
        if (ram >= 4096 && cores >= 6) return Mid;
        return Low;
    }
}
