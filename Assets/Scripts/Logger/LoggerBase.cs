using TMPro;
using UnityEngine;

public abstract class LoggerBase
{
    public TextMeshProUGUI textMesh;

    public abstract void Log(string message);

    public abstract void ShowLogs();

    public abstract void SaveLogs();

    public abstract void ClearLogs();

    public static LoggerBase CreateLogger()
    {
#if UNITY_ANDROID
        if(!Application.isEditor)
            return new AndroidLogger($"{Application.persistentDataPath}/Log.txt");
        else
            return new DefaultLogger($"{Application.persistentDataPath}/Log.txt");
#else
        return new DefaultLogger($"{Application.persistentDataPath}/Log.txt");
#endif
    }
}
